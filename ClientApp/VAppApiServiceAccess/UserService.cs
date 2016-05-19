using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAppApiServiceAccess.Common;
using VAppApiServiceAccess.Models;

namespace VAppApiServiceAccess
{
    public class UserService
    {

        private const string GET_USERS_URL = "http://localhost:8079/api/usercoordinates";
        private const string CREATE_USER_URL = "http://localhost:8079/api/createuser";
        private const string UPDATE_USER_COORDINATE_URL = "http://localhost:8079/api/updatecoordinates";

        IAuthenticationService authenticationService;

        public UserService(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public UserService()
        {
        }
        public UserCoordinatesResponse ViewUserCoordinates()
        {
            HttpRequestHandler requestHandler = new HttpRequestHandler();
            requestHandler.AccessToken = authenticationService.AuthenticationToken;
            requestHandler.Method = "GET";
            requestHandler.Url = GET_USERS_URL;
            var userCoordinateResponse=requestHandler.SendRequest<UserCoordinatesResponse>();
            return userCoordinateResponse;
        }

        public CreateUserResponse CreateUser(User user)
        {
            HttpRequestHandler requestHandler = new HttpRequestHandler();
            requestHandler.Method = "POST";
            requestHandler.Url = CREATE_USER_URL;
            return requestHandler.SendRequest<User, CreateUserResponse>(user);
        }

        public UpdateUserCoordinateResponse UpdateUserCoordinates(UserCoordinate userCoordinate)
        {
            HttpRequestHandler requestHandler = new HttpRequestHandler();
            requestHandler.AccessToken = authenticationService.AuthenticationToken;
            requestHandler.Method = "POST";
            requestHandler.Url = UPDATE_USER_COORDINATE_URL;
            return requestHandler.SendRequest<UserCoordinate, UpdateUserCoordinateResponse>(userCoordinate);
        }


    }
}
