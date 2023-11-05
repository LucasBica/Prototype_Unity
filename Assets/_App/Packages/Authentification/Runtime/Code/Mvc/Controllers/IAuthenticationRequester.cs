using System;

using LB.Authentication.Models;
using LB.Http.Models;

namespace LB.Authentication.Runtime {

    public interface IAuthenticationRequester {

        public void SignUp(SignUpRequest data, Action<SignInResponse> onSuccess, Action<HttpError> onError);

        public void SignIn(SignInRequest data, Action<SignInResponse> onSuccess, Action<HttpError> onError);

        public void RecoveryPassword(RecoveryPasswordRequest data, Action<object> onSuccess, Action<HttpError> onError);

        public void GetUserMe(Action<User> onSuccess, Action<HttpError> onError);
    }
}