using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using FluentEmail.Core;

namespace AuthorizationAPI.Services.Services;

public class FluentEmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IFluentEmailFactory _fluentEmailFactory;
    public FluentEmailService(
            IFluentEmail fluentEmail, 
            IFluentEmailFactory fluentEmailFactory)
    {
        _fluentEmail = fluentEmail;
        _fluentEmailFactory = fluentEmailFactory;
    }

    public async Task<bool> SendMultipleConsumersMail(List<EmailMetadata> emailMetadataList)
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

    public async Task<bool> SendSingleMail(EmailMetadata emailMetadata)
    {
        var response = await _fluentEmail
                .To(emailMetadata.ToAddress)
                .Subject(emailMetadata.Subject)
                .Body(emailMetadata.Body)
                .SendAsync();
        
        return response.Successful;
    }
}
