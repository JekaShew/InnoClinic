using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces;

public interface IEmailService
{
    public Task<ResponseMessage> SendVerificationLetterToEmail(string email);
    public Task<bool> SendSingleMail(EmailMetadata emailMetadata);
    public Task<bool> SendMultipleConsumersMail(List<EmailMetadata> emailMetadataList);
}
