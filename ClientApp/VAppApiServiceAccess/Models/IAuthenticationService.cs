using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAppApiServiceAccess.Models
{
    public interface IAuthenticationService
    {
        bool Authenticate(string userName, string password);
        string AuthenticationToken { get; }

        bool IsAuthenticated { get; }
        Claim Claim { get; }
    }
}
