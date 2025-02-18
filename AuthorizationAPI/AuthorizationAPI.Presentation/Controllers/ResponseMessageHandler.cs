using AuthorizationAPI.Shared.Constants;
using CommonLibrary.Response;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

public class ResponseMessageHandler : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleResponseMessage(ResponseMessage? responseMessage)
    {
        if (responseMessage.Message.Key.Equals(MessageConstants.Base400))
        {
            return new FailMessage(responseMessage.Message.Value, 400);
        }
        
        if (responseMessage.Message.Key.Equals(MessageConstants.Create400))
        {
            return new FailMessage(responseMessage.Message.Value, 400);
        }
        
        if (responseMessage.Message.Key.Equals(MessageConstants.Delete400))
        {
            return new FailMessage(responseMessage.Message.Value, 400);
        }
        
        if (responseMessage.Message.Key.Equals(MessageConstants.Update400))
        {
            return new FailMessage(responseMessage.Message.Value, 400);
        }
       
        if (responseMessage.Message.Key.Equals(MessageConstants.Base404))
        {
            return new FailMessage(responseMessage.Message.Value, 404);
        }
             
        if (responseMessage.Message.Key.Equals(MessageConstants.Base403))
        {
            return new FailMessage(responseMessage.Message.Value, 403);
        }
              
        if (responseMessage.Message.Key.Equals(MessageConstants.CheckDB400))
        {
            return new FailMessage(responseMessage.Message.Value, 400);
        }
        
        if (responseMessage.Message.Key.Equals(MessageConstants.CheckCreds400))
        {
            return new FailMessage(responseMessage.Message.Value, 400);
        }
        
        if (responseMessage.Message.Key.Equals(MessageConstants.EmailRegistered400))
        {
            return new FailMessage(responseMessage.Message.Value, 400);
        }

        if (responseMessage.Message.Key.Equals(MessageConstants.FailEmailVerificationMessage))
        {
            return new FailMessage(responseMessage.Message.Value, 500);
        }

        if (responseMessage.Message.Key.Equals(MessageConstants.FailSendEmailMessage500))
        {
            return new FailMessage(responseMessage.Message.Value, 500);
        }

        return new FailMessage(responseMessage.Message.Value, 500);
    }
}
