using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAppApiServiceAccess.Models
{
    public class Claim
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
    }
}
