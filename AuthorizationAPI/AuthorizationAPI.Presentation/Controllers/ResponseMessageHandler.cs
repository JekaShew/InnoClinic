using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

public class ResponseMessageHandler : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleResponseMessage(ResponseMessage? responseMessage)
    {
        if (responseMessage.Message.Key.Equals("400Base"))
            return BadRequest(responseMessage.Message.Value);

        if (responseMessage.Message.Key.Equals("400Create"))
            return BadRequest(responseMessage.Message.Value);

        if (responseMessage.Message.Key.Equals("400Delete"))
            return BadRequest(responseMessage.Message.Value);

        if (responseMessage.Message.Key.Equals("400Update"))
            return BadRequest(responseMessage.Message.Value);

        if (responseMessage.Message.Key.Equals("404"))
            return NotFound(responseMessage.Message.Value); 

        if (responseMessage.Message.Key.Equals("403"))
            return StatusCode(403, responseMessage.Message.Value);

        if (responseMessage.Message.Key.Equals("400CheckDB"))
            return BadRequest(responseMessage.Message.Value);

        if (responseMessage.Message.Key.Equals("400CheckCreds"))
            return BadRequest(responseMessage.Message.Value);

        if (responseMessage.Message.Key.Equals("400EmailRegistered"))
            return BadRequest(responseMessage.Message.Value);

        return StatusCode(500, responseMessage.Message.Value);
    }
}
