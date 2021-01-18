using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebTemplate.TemplateClasses
{
    public class CustomAuthService : ICustomAuthService
    {

        public UserData TryLogin(string username, string password)
        {

            bool isValid = true;

            if ( !isValid )
                return null;

            return GetUserDataByUser( username );
        }

        public UserData TryWindowsLogin(string username)
        {
            return GetUserDataByUser( username );
        }

        public UserData GetUserDataByUser( string username )
        {
            UserData userData = new UserData( )
            {
                Username = username,
                ClaimsPrincipal = getClaimsByUser( username )
            };

            userData.JsModules = getJsModulesByUser( username );

            return userData;
        }

        private ClaimsPrincipal getClaimsByUser(string username)
        {
            ClaimsPrincipal princ = new ClaimsPrincipal(
                new ClaimsIdentity( new[]
                {
                    new Claim(ClaimTypes.Role, "testRole")
                }, "authentication type" ) );
            return princ;
        }

        private List<string> getJsModulesByUser(string username)
        {
            return new List<string>( ) { JsModules.AUTHENTICATED, "testModule" };
        }


    }
}
