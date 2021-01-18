using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebTemplate.TemplateClasses
{
    public class SessionService
    {

        private Dictionary<string, UserData> m_SessionDataDict = new Dictionary<string, UserData>( );


        public UserData GetSessionAdatok( string sessionId )
        {
            if ( m_SessionDataDict.ContainsKey( sessionId ) )
                return m_SessionDataDict[ sessionId ];

            m_SessionDataDict.Add( sessionId, new UserData
            {
                ClaimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity( new[]
                    {
                        //new Claim(ClaimTypes.Name, hash.Split("||")[0]),
                        new Claim(ClaimTypes.Role, "testRole")
                    }, "authentication type" ) )
            } );

            return m_SessionDataDict[ sessionId ];

        }

        public void LogInUser( string sessionId, UserData user )
        {
            m_SessionDataDict.Add( sessionId, user );
        }

        public async Task<string> GetHash( string windowsUser, string browser )
        {
            return "asdf";
        }

    }
}
