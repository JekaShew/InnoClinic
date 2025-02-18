using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces;

public interface IEmailService
{
    public Task<ResponseMessage> SendUserEmail(IEnumerable<UserEmailDTO> userEmailDTOs, Guid userId, Guid roleId = default );
    public Task<ResponseMessage> SendVerificationLetterToEmail(string email);
    public Task<bool> SendSingleMail(EmailMetaData emailMetadata);
    public Task<bool> SendMultipleConsumersMail(IEnumerable<EmailMetaData> emailMetadataList);
    public string GenerateEmailConfirmationTokenByEmailAndDateTime(string email, string dateTimeString);
}
