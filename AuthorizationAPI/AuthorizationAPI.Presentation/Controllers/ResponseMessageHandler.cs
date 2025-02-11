using AuthorizationAPI.Shared.Constants;
using CommonLibrary.Response.FailMesssages;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

public class ResponseMessageHandler : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleResponseMessage(ResponseMessage? responseMessage)
    {
        if (responseMessage.Message.Key.Equals(MessageConstants.Base400))
            return BadRequest(new BadRequestMessage(responseMessage.Message.Value));

        if (responseMessage.Message.Key.Equals(MessageConstants.Create400))
            return BadRequest(new BadRequestMessage(responseMessage.Message.Value));

        if (responseMessage.Message.Key.Equals(MessageConstants.Delete400))
            return BadRequest(new BadRequestMessage(responseMessage.Message.Value));

        if (responseMessage.Message.Key.Equals(MessageConstants.Update400))
            return BadRequest(new BadRequestMessage(responseMessage.Message.Value));

        if (responseMessage.Message.Key.Equals(MessageConstants.Base404))
            return NotFound(new NotFoundMessage(responseMessage.Message.Value)); 

        if (responseMessage.Message.Key.Equals(MessageConstants.Base403))
            return StatusCode(403, new ForbiddenMessage(responseMessage.Message.Value));

        if (responseMessage.Message.Key.Equals(MessageConstants.CheckDB400))
            return BadRequest(new BadRequestMessage(responseMessage.Message.Value));

        if (responseMessage.Message.Key.Equals(MessageConstants.CheckCreds400))
            return BadRequest(new BadRequestMessage(responseMessage.Message.Value));

        if (responseMessage.Message.Key.Equals(MessageConstants.EmailRegistered400))
            return BadRequest(new BadRequestMessage(responseMessage.Message.Value));

        return StatusCode(500, new FailMessage(responseMessage.Message.Value));
    }
}
