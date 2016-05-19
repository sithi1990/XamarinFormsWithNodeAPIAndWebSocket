using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using VAppApiServiceAccess.Common;
using VAppApiServiceAccess.Models;

namespace VAppApiServiceAccess
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string AUTHENCATION_URL = "http://localhost:8079/api/authenticate";

        public string authenticationToken=String.Empty;
        public Claim Claim { get; set; }
        public bool IsAuthenticated
        {
            get;
            set;
        }
        public string AuthenticationToken
        {
            get { return authenticationToken; }
        }

        public bool Authenticate(string userName, string password)
        {
            HttpRequestHandler requestHandler = new HttpRequestHandler();
            requestHandler.Url = AUTHENCATION_URL;
            requestHandler.Method = "POST";
            var user = new User() {UserName=userName,Password=password};
            var authenticationResult=requestHandler.SendRequest<User, AuthenticationResponse>(user);

            if(authenticationResult.IsSuccess)
            {
                authenticationToken = authenticationResult.Token;
                UpdateClaimDetails(authenticationToken);              
            }
            IsAuthenticated = authenticationResult.IsSuccess;
            return authenticationResult.IsSuccess;
        }

        private void UpdateClaimDetails(string token)
        {
            var secretKey = "vappsecret";
            try
            {
                string jsonPayload = JWT.JsonWebToken.Decode(token, secretKey);
                Claim = JsonConvert.DeserializeObject<Claim>(jsonPayload);
            }
            catch (JWT.SignatureVerificationException)
            {
                Console.WriteLine("Invalid token!");
            }
 
        }



        
    }
}
