using BlazorWebTemplate.TemplateClasses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Principal;

namespace BlazorWebTemplate.Controllers
{


    [Route( "template" )]
    //[Authorize( AuthenticationSchemes = IISServerDefaults.AuthenticationScheme, Roles = "testRole" )]
    public class TemplateController : Controller
    {

        private SessionService m_SessionService;
        private ICustomAuthService m_AuthService;

        public TemplateController( SessionService sessionService, ICustomAuthService authService )
        {
            m_SessionService = sessionService;
            m_AuthService = authService;
        }


        [HttpPost]
        [Route( "changeSessionId" )]
        public IActionResult ChangeSessionId( [FromBody] ChangeSessionIdData data )
        {
            var res = m_SessionService.ChangeSessionId( data );

            var jsonString = JsonConvert.SerializeObject( res );

            return Ok( jsonString );
        }

        [HttpPost]
        [Route( "windowsUser" )]
        public IActionResult GetWindowsUser( [FromBody] ChangeSessionIdData data )
        {

            var sessionId = data.OldSessionId;

            var windowsUser = HttpContext.User.Identity.Name;
            windowsUser = windowsUser.Replace( '\\', '_');

            sessionId = TemplateHelper.ReplaceSegmentOfSessionId( sessionId, SessionSegments.USERNAME, windowsUser );

            var clientData = new ClientData( ) { SessionId = sessionId };

            var userData = m_AuthService.TryWindowsLogin( sessionId );
            m_SessionService.LogInWindowsUser( clientData, userData );

            return Ok( sessionId );
        }

    }
}
