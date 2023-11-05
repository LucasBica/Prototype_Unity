using System;
using System.Collections.Generic;

using LB.Authentication.Models;
using LB.Http.Models;

namespace LB.Authentication.Runtime {

    public interface IAuthenticationService {

        public SignInResponse SignInResponse { get; }

        public Dictionary<string, string> AuthorizationHeader { get; }

        public void SignUp(SignUpRequest data, Action<SignInResponse> OnSuccess, Action<HttpError> OnError);

        public void SignIn(SignInRequest data, Action<SignInResponse> OnSuccess, Action<HttpError> OnError);

        public void SignOut();

        public void DeleteAccount();

        public void RecoveryPassword(RecoveryPasswordRequest data, Action<object> OnSuccess, Action<HttpError> OnError);

        public void GetUserMe(Action<User> OnSuccess, Action<HttpError> OnError);
    }
}