using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebTemplate.TemplateClasses
{
    public class SessionService
    {

        private Dictionary<string, SessionData> m_SessionDataDict = new Dictionary<string, SessionData>( );


        public SessionData GetSessionAdatok( string hash )
        {
            if ( m_SessionDataDict.ContainsKey( hash ) )
                return m_SessionDataDict[ hash ];

            m_SessionDataDict.Add( hash, new SessionData
            {
                ClaimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity( new[]
                    {
                        //new Claim(ClaimTypes.Name, hash.Split("||")[0]),
                        new Claim(ClaimTypes.Role, "testRole")
                    }, "authentication type" ) )
            } );

            return m_SessionDataDict[ hash ];

        }

        public async Task<string> GetHash( string windowsUser, string browser )
        {
            return "asdf";
        }

    }
}
