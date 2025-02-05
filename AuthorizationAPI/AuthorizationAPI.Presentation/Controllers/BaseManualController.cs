using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers
{
    public class BaseManualController : ControllerBase
    {

        public IActionResult HandlePesponseMessage(ResponseMessage? responseMessage)
        {
            //if ( responseMessage.Message.Key.Equals("200Base"))
            //    return StatusCode(200, responseMessage.Message.Value);

            if (responseMessage.Message.Key.Equals("400Base"))
                return StatusCode(400, responseMessage.Message.Value);

            //if (responseMessage.Message.Key.Equals("201Create"))
            //    return StatusCode(201, responseMessage.Message.Value);

            if (responseMessage.Message.Key.Equals("400Create"))
                return StatusCode(400, responseMessage.Message.Value);

            //if (responseMessage.Message.Key.Equals("204Delete"))
            //    return StatusCode(200, responseMessage.Message.Value);

            if (responseMessage.Message.Key.Equals("400Delete"))
                return StatusCode(400, responseMessage.Message.Value);

            //if (responseMessage.Message.Key.Equals("200Update"))
            //    return StatusCode(200, responseMessage.Message.Value);

            if (responseMessage.Message.Key.Equals("400Update"))
                return StatusCode(400, responseMessage.Message.Value);

            if (responseMessage.Message.Key.Equals("404"))
                return StatusCode(404, responseMessage.Message.Value);

            if (responseMessage.Message.Key.Equals("403"))
                return StatusCode(403, responseMessage.Message.Value);

            if (responseMessage.Message.Key.Equals("400CheckDB"))
                return StatusCode(400, responseMessage.Message.Value);

            if (responseMessage.Message.Key.Equals("400CheckCreds"))
                return StatusCode(400, responseMessage.Message.Value);

            if (responseMessage.Message.Key.Equals("400EmailRegistered"))
                return StatusCode(400, responseMessage.Message.Value);

            return StatusCode(500, responseMessage.Message.Value);
        }
    }
}
