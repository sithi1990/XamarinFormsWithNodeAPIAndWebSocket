using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAppApiServiceAccess.Models
{
    public class AuthenticationResponse : ResponseBase
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        public string AuthenticatedUserName { get; set; }
    }
}
