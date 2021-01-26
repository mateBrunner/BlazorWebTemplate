using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebTemplate.TemplateClasses
{
    public interface ICustomAuthService
    {

        SessionData TryLogin( string username, string password );

        SessionData TryWindowsLogin( string username );

    }
}
