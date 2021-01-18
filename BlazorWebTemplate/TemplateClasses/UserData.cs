using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebTemplate.TemplateClasses
{
    public class UserData
    {

        public string Username { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        public List<string> JsModules { get; set; }

    }
}
