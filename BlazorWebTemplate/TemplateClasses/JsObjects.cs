using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebTemplate.TemplateClasses
{
    public class ClientData
    {

        [JsonProperty( "sessionId" )]
        public string SessionId { get; set; }

        [JsonProperty( "ip" )]
        public string IpAddress { get; set; }

    }
}
