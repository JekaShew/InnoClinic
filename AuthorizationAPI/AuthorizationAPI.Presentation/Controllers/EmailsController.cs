using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailsController : ControllerBase
{
    // foreach check Authorize
    // foreach check Roles
    // where email metadata should be created??
    // responses?

    // for multy message noreply ONLY
    // send subject string;  send body string 


    // implement send single from doctor custom message with subject ONLY DOCTOR
    // implement send single from administrator custom message with subject ONLY ADMINISTRATOR
    // implement send single from noreply custom message with subject ONLY ADMINISTRATOR
    // implement send multy custom message from NoReply with subject ONLY ADMINISTRATOR
}
