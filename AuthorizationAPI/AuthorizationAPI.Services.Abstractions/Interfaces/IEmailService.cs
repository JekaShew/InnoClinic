using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;

namespace AuthorizationAPI.Services.Abstractions.Interfaces;

public interface IEmailService
{
    public Task<bool> SendSingleMail(EmailMetadata emailMetadata);
    public Task<bool> SendMultipleConsumersMail(List<EmailMetadata> emailMetadataList);
}
