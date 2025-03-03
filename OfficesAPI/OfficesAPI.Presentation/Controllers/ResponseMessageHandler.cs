using CommonLibrary.Response;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;
using OfficesAPI.Shared.Constnts;

namespace OfficesAPI.Presentation.Controllers;

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

        return new FailMessage(responseMessage.Message.Value, 500);
    }
}
