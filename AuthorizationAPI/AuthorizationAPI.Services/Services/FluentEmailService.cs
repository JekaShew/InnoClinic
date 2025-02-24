using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Extensions;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Encodings.Web;

namespace AuthorizationAPI.Services.Services;

public class FluentEmailService : IEmailService
{
    private readonly FromEmailsSettings _fromEmailsSettings;
    private readonly IFluentEmail _fluentEmail;
    private readonly IFluentEmailFactory _fluentEmailFactory;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ICommonService _commonService;
    private readonly IMemoryCache _memoryCache;
    private readonly IValidator<UserEmailDTO> _userEmailValidator;
    public FluentEmailService(
            IOptions<FromEmailsSettings> options,
            IFluentEmail fluentEmail,
            IFluentEmailFactory fluentEmailFactory,
            IMemoryCache memoryCache,
            IValidator<UserEmailDTO> usrEmailValidator,
            IRepositoryManager repositoryManager,
            ICommonService commonService)
    {
        _fromEmailsSettings = options.Value;
        _fluentEmail = fluentEmail;
        _fluentEmailFactory = fluentEmailFactory;
        _memoryCache = memoryCache;
        _userEmailValidator = usrEmailValidator;
        _repositoryManager = repositoryManager;
        _commonService = commonService;
    }

    public async Task<ResponseMessage> SendVerificationLetterToEmail(string email)
    {
        // Setup Confirmation Message with confirm Link 
        var currentDateTimeString = DateTime.UtcNow.ToString();
        var confirmEmailToken = GenerateEmailConfirmationTokenByEmailAndDateTime(email, currentDateTimeString);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSize(1)
            .SetPriority(CacheItemPriority.High);
        _memoryCache.Set(email, currentDateTimeString, cacheEntryOptions);

        var callbackParameters = new Dictionary<string, string>
        {
            {"token", confirmEmailToken },
            {"email", email}
        };
        // how to grab domain address and url of action to confirmEmail
        var callback = QueryHelpers.AddQueryString("http://localhost:5000/api/Users/verifyemail", callbackParameters);

        // Send Email
        var verificationEmailMetadata = new EmailMetaData(
                email,
                EmailTemplates.VerificationTemplate.Key,
                $"{EmailTemplates.VerificationTemplate.Value} <a href='{HtmlEncoder.Default.Encode(callback)}'>{callback}</a>.");
        var sendEmail = await SendSingleMail(verificationEmailMetadata);
        if (!sendEmail)
        {
            return new ResponseMessage(MessageConstants.FailEmailVerificationMessage, false);
        }

        return new ResponseMessage(MessageConstants.SuccessUpdateMessage, true);
    }

    public async Task<ResponseMessage> SendUserEmail(IEnumerable<UserEmailDTO> userEmailDTOs,Guid userId, Guid roleId = default)
    {
        var emailMetaDatas = new List<EmailMetaData>();
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        bool isAdmin = currentUserInfo.Role.Equals(RoleConstants.Administrator);
        bool isDoctor = currentUserInfo.Role.Equals(RoleConstants.Doctor);

        foreach (var userEmailDTO in userEmailDTOs)
        {
            var validationResult = await _userEmailValidator.ValidateAsync(userEmailDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
            }
            
            if (roleId.Equals(Guid.Empty) && isAdmin)
            {
                var emailMetaData = new EmailMetaData(
                        userEmailDTO.ToAddress, 
                        userEmailDTO.Subject, 
                        userEmailDTO.Subject);

                emailMetaDatas.Add(emailMetaData);
                continue;
            }

            if (roleId.Equals(DBConstants.DoctorRoleId) && isDoctor)
            {
                var emailMetaData = new EmailMetaData(
                        userEmailDTO.ToAddress,
                        userEmailDTO.Subject,
                        userEmailDTO.Subject,
                        roleId);

                emailMetaDatas.Add(emailMetaData);
                continue;
            }

            if (roleId.Equals(DBConstants.AdministratorRoleId) && isAdmin)
            {
                var emailMetaData = new EmailMetaData(
                        userEmailDTO.ToAddress,
                        userEmailDTO.Subject,
                        userEmailDTO.Subject,
                        roleId);

                emailMetaDatas.Add(emailMetaData);
                continue;
            }

            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        bool response = 
                userEmailDTOs.Count() == 1 ? 
                    response = await SendSingleMail(emailMetaDatas.FirstOrDefault()) : 
                    response = await SendMultipleConsumersMail(emailMetaDatas);
        if(!response)
        {
            return new ResponseMessage(MessageConstants.FailSendEmailMessage500, false);
        }

        return new ResponseMessage(MessageConstants.SuccessCreateMessage, true);
    }

    public async Task<bool> SendMultipleConsumersMail(IEnumerable<EmailMetaData> emailMetadataList)
    {
        List<bool> responsesSuccess = new List<bool>();
        foreach(var emailMetadata in emailMetadataList)
        {
            var response = await _fluentEmailFactory
                .Create()
                .To(emailMetadata.ToAddress)
                .Subject(emailMetadata.Subject)
                .Body(emailMetadata.Body)
                .SendAsync();

            responsesSuccess.Add(response.Successful);
        }

        return responsesSuccess.Any(rs => !rs) ? false : true;
    }

    public async Task<bool> SendSingleMail(EmailMetaData emailMetadata)
    {
        var response = await FromAddressHandler(emailMetadata);
        if(response is null)
        {
            return false;
        }

        return response.Successful;
    }

    public string GenerateEmailConfirmationTokenByEmailAndDateTime(string email, string dateTimeString)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{email}{dateTimeString}"));
        var emailConfirmationToken = Convert.ToBase64String(symmetricSecurityKey.Key);

        return emailConfirmationToken;
    }

    private async Task<SendResponse> FromAddressHandler(EmailMetaData emailMetadata)
    {
        var response = new SendResponse();
        if (emailMetadata.FromAdress.Equals(DBConstants.AdministratorRoleId))
        {
            return response = await _fluentEmail
            .SetFrom(_fromEmailsSettings.Administrator)
            .To(emailMetadata.ToAddress)
            .Subject(emailMetadata.Subject)
            .Body(emailMetadata.Body)
            .SendAsync();
        }

        if (emailMetadata.FromAdress.Equals(DBConstants.DoctorRoleId))
        {
            return response = await _fluentEmail
            .SetFrom(_fromEmailsSettings.Doctor)
            .To(emailMetadata.ToAddress)
            .Subject(emailMetadata.Subject)
            .Body(emailMetadata.Body)
            .SendAsync();
        }

        return response = await _fluentEmail
                .To(emailMetadata.ToAddress)
                .Subject(emailMetadata.Subject)
                .Body(emailMetadata.Body)
                .SendAsync();
    }
}
