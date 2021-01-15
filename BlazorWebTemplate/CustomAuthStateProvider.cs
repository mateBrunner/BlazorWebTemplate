using BlazorWebTemplate.TemplateClasses;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebTemplate
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private IJSRuntime m_JSRuntime;
        private SessionService m_SessionService;

        public CustomAuthStateProvider( IJSRuntime jsRuntime, SessionService sessionService )
        {
            m_JSRuntime = jsRuntime;
            m_SessionService = sessionService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync( )
        {
            SessionData userAdatok = await GetSessionAdatok( );

            return await Task.FromResult( new AuthenticationState( userAdatok.ClaimsPrincipal ) );
        }

        public async void LogInUser( string username )
        {
            SessionData userAdatok = await GetSessionAdatok( );

            //( userAdatok.ClaimsPrincipal.Identity as ClaimsIdentity ).AddClaim( new Claim( ClaimTypes.Role, "role2" ) );
            await m_JSRuntime.InvokeAsync<string>( "successfulLogin", "üzenet" );
            NotifyAuthenticationStateChanged( Task.FromResult( new AuthenticationState( userAdatok.ClaimsPrincipal ) ) );
        }

        private async Task<SessionData> GetSessionAdatok( )
        {
            //string browser = await m_JSRuntime.InvokeAsync<string>( "getBrowserInfo" );
            var machineName = System.Environment.MachineName;
            string hash = await m_SessionService.GetHash( machineName, "browser" );

            return m_SessionService.GetSessionAdatok( hash );
        }

    }
}
