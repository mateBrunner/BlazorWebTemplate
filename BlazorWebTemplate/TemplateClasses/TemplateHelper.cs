using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebTemplate.TemplateClasses
{
    public class TemplateHelper
    {

        public static string GetSegmentOfSessionId( string sessionId, int segmentIndex)
        {

            var splitted = sessionId.Split( "__" );
            if ( splitted.Length < segmentIndex )
                return null;
            else
                return splitted[ segmentIndex ];

        }

        public static string ReplaceSegmentOfSessionId( string sessionId, int segmentIndex, string newValue )
        {
            var splitted = sessionId.Split( "__" );
            splitted[ segmentIndex ] = newValue;
            return String.Join( "__", splitted );
        }

    }


    public class SessionSegments
    {
        public const int APPNAME = 0;
        public const int INITDATE = 1;
        public const int USERNAME = 2;
        public const int GUIDCLIENT = 3;
        public const int LOGINDATE = 4;
        public const int GUIDSERVER = 5;
    }
}
