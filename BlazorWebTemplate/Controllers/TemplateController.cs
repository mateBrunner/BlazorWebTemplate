using BlazorWebTemplate.TemplateClasses;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebTemplate.Controllers
{


    [Route( "template" )]
    //[Authorize( AuthenticationSchemes = IISServerDefaults.AuthenticationScheme, Roles = "testRole" )]
    public class TemplateController : Controller
    {

        private SessionService m_SessionService;

        public TemplateController(SessionService sessionService)
        {
            m_SessionService = sessionService;
        }


        [HttpPost]
        [Route( "changeSessionId" )]
        public IActionResult ReplaceSessionId( [FromBody] ChangeSessionIdData data )
        {
            m_SessionService.ChangeSessionId( data );

            return Ok();
        }
    }
}
