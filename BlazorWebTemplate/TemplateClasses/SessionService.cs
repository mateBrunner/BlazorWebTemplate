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


        public SessionData GetSessionAdatok( ClientData client )
        {
            if ( m_SessionDataDict.ContainsKey( client.SessionId ) )
                return m_SessionDataDict[ client.SessionId ];

            m_SessionDataDict.Add( client.SessionId, new SessionData
            {
                ClaimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity( new[]
                    {
                        //new Claim(ClaimTypes.Name, hash.Split("||")[0]),
                        new Claim(ClaimTypes.Role, "testRole")
                    }, "authentication type" ) )
            } );

            return m_SessionDataDict[ client.SessionId ];
        }

        public SessionData GetSessionAdatok( string sessionId )
        {
            if ( m_SessionDataDict.ContainsKey( sessionId ) )
                return m_SessionDataDict[ sessionId ];

            m_SessionDataDict.Add( sessionId, new SessionData
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

        public string LogInUser( ClientData clientData, SessionData newUserData )
        {

            m_SessionDataDict.Remove( clientData.SessionId );

            var date = DateTime.Now.ToString( "yyyyMMddTHHmmssfff" );
            string newSessionId = $"{clientData.SessionId}__{date}__{Guid.NewGuid()}";
            newSessionId = newSessionId.Replace( "anonym", newUserData.Username );

            m_SessionDataDict.Add( newSessionId, newUserData );

            return newSessionId;
        }

        //public void LogInOtherTab( LoginData data )
        //{

        //    if ( data.FirstLoginSessionId == data.SessionId )
        //        return;

        //    var sessionData = m_SessionDataDict[ data.FirstLoginSessionId ];
        //    var oldSessionData = m_SessionDataDict[ data.OldSessionId ];
        //    m_SessionDataDict.Remove( data.OldSessionId );

        //    sessionData.ClaimsPrincipal = oldSessionData.ClaimsPrincipal;
        //    sessionData.JsModules = oldSessionData.JsModules;
        //    sessionData.Username = oldSessionData.Username;

        //    m_SessionDataDict[ data.SessionId ] = sessionData;

        //}

        public bool IsLoggedIn(string sessionId)
        {
            return TemplateHelper.GetSegmentOfSessionId( sessionId, SessionSegments.GUIDSERVER ) != null;
        }

        public void ChangeSessionId( ChangeSessionIdData data )
        {

            var sessionData = m_SessionDataDict[ data.OldSessionId ];
            m_SessionDataDict.Remove( data.OldSessionId );
            m_SessionDataDict[ data.NewSessionId ] = sessionData;
        }

    }
}
