using BlazorWebTemplate.TemplateClasses;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebTemplate
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private IJSRuntime m_JSRuntime;
        private SessionService m_SessionService;
        private IConfiguration m_config;

        public CustomAuthStateProvider( IJSRuntime jsRuntime, SessionService sessionService, IConfiguration config )
        {
            m_JSRuntime = jsRuntime;
            m_SessionService = sessionService;
            m_config = config;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync( )
        {

            string userName = null;
            if ( m_config[ $"CustomOptions:AuthenticationType" ] == "windows" )
                userName = System.Environment.MachineName;

            var currentTime = DateTime.Now.ToString( "yyyyMMddHHmmss" );

            //lekérdezzük a sessionId-t. Ha windows-os aut. van, és a window.name üres, akkor generálunk
            var sessionId = await m_JSRuntime.InvokeAsync<string>( "checkSessionId", userName, currentTime );

            UserData userAdatok = m_SessionService.GetSessionAdatok( sessionId );

            return await Task.FromResult( new AuthenticationState( userAdatok.ClaimsPrincipal ) );
        }


    }
}
