using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Extensions;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Encodings.Web;

namespace AuthorizationAPI.Services.Services;

public class FluentEmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IFluentEmailFactory _fluentEmailFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMemoryCache _memoryCache;
    private readonly FromEmailsSettings _fromEmailsSettings;
    private readonly IValidator<UserEmailDTO> _userEmailValidator;
    public FluentEmailService(
            IFluentEmail fluentEmail,
            IFluentEmailFactory fluentEmailFactory,
            IAuthorizationService authorizationService,
            IMemoryCache memoryCache,
            IValidator<UserEmailDTO> usrEmailValidator)
    {
        _fluentEmail = fluentEmail;
        _fluentEmailFactory = fluentEmailFactory;
        _authorizationService = authorizationService;
        _memoryCache = memoryCache;
        _userEmailValidator = usrEmailValidator;
    }

    public async Task<ResponseMessage> SendVerificationLetterToEmail(string email)
    {
        // Setup Confirmation Message with confirm Link 
        var currentDateTimeString = DateTime.UtcNow.ToString();
        var confirmEmailToken = _authorizationService.GenerateEmailConfirmationTokenByEmailAndDateTime(email, currentDateTimeString);
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

    public async Task<ResponseMessage> SendUserEmail(IEnumerable<UserEmailDTO> userEmailDTOs,Guid roleId)
    {
        
        foreach(var userEmailDTO in userEmailDTOs)
        {
            var validationResult = await _userEmailValidator.ValidateAsync(userEmailDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
            }

            if (roleId is null)
            {
                var emailMetaData = new EmailMetaData(
                        userEmailDTO.ToAddress, 
                        userEmailDTO.Subject, 
                        userEmailDTO.Subject, 
                        roleId);
            }
        }

        if(userEmailDTOs.Count() == 1)
        {
            var singelEmailMetaData = new EmailMetaData(userEmailDTOs.);

            var sendSingle = await SendSingleMail()
        }
        
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
