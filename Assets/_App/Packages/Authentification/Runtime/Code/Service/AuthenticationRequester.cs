using System;

using LB.Authentication.Models;
using LB.Http.Models;
using LB.Http.Runtime;

using Newtonsoft.Json;

namespace LB.Authentication.Runtime {

    public class AuthenticationRequester : IAuthenticationRequester {

        protected string host;
        protected IHttpService httpService;

        public AuthenticationRequester(string host, IHttpService httpService) {
            this.host = host;
            this.httpService = httpService;
        }

        public virtual void SignUp(SignUpRequest data, Action<SignInResponse> onSuccess, Action<HttpError> onError) {
            Request request = new RequestBuilder(HttpMethodTypes.Post, host + "auth/local/register")
                .SetBody(JsonConvert.SerializeObject(data))
                .Build();

            httpService.SendRequest(request, onSuccess, onError);
        }

        public virtual void SignIn(SignInRequest data, Action<SignInResponse> onSuccess, Action<HttpError> onError) {
            Request request = new RequestBuilder(HttpMethodTypes.Post, host + "auth/local")
                .SetBody(JsonConvert.SerializeObject(data))
                .Build();

            httpService.SendRequest(request, onSuccess, onError);
        }

        public virtual void RecoveryPassword(RecoveryPasswordRequest data, Action<object> onSuccess, Action<HttpError> onError) {
            Request request = new RequestBuilder(HttpMethodTypes.Post, host + "auth/forgot-password")
                .SetBody(JsonConvert.SerializeObject(data))
                .Build();

            httpService.SendRequest(request, onSuccess, onError);
        }

        public virtual void GetUserMe(Action<User> onSuccess, Action<HttpError> onError) {
            Request request = new RequestBuilder(HttpMethodTypes.Get, host + "auth/me")
                .Build();

            httpService.SendRequest(request, onSuccess, onError);
        }
    }
}