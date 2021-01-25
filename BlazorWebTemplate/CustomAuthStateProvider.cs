using BlazorWebTemplate.TemplateClasses;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
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

            var currentTime = DateTime.Now.ToString( "yyyyMMdd-HHmmssfff" );

            //lekérdezzük a sessionId-t. Ha windows-os aut. van, és a window.name üres, akkor generálunk
            var data = await m_JSRuntime.InvokeAsync<string>( "GetClientData" );
            ClientData clientData = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientData>( data );

            SessionData sessionAdatok = m_SessionService.GetSessionAdatok( clientData );

            return await Task.FromResult( new AuthenticationState( sessionAdatok.ClaimsPrincipal ) );
        }


    }
}
