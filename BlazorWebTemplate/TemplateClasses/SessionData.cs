using System.Collections.Generic;
using System.Security.Claims;

namespace BlazorWebTemplate.TemplateClasses
{
    public class SessionData
    {

        public string Username { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        public List<string> JsModules { get; set; }

    }
}
