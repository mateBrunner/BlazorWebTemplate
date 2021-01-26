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

            //lekérdezzük a sessionId-t. Ha windows-os aut. van, és a window.name üres, akkor generálunk
            var data = await m_JSRuntime.InvokeAsync<string>( "GetClientData" );

            if ( data == null )
                return await Task.FromResult( new AuthenticationState( new ClaimsPrincipal(
                new ClaimsIdentity( new[]
                {
                    new Claim(ClaimTypes.Role, "testRole")
                }, "authentication type" ) )) );

            ClientData clientData = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientData>( data );

            SessionData sessionAdatok = m_SessionService.GetSessionAdatok( clientData );

            return await Task.FromResult( new AuthenticationState( sessionAdatok.ClaimsPrincipal ) );
        }


    }
}
