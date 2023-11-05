using System;
using System.Collections.Generic;

using LB.Authentication.Models;
using LB.Core.Runtime.Utilities;
using LB.Http.Models;

namespace LB.Authentication.Runtime {

    public class AuthenticationService : IAuthenticationService {

        protected IUsernameValidator usernameValidator;
        protected IEmailValidator emailValidator;
        protected IPasswordValidator passwordValidator;
        protected IAuthenticationRequester authenticationRequester;

        public SignInResponse SignInResponse { get; private set; }

        public Dictionary<string, string> AuthorizationHeader {
            get {
                if (SignInResponse == null || string.IsNullOrEmpty(SignInResponse.jwt)) {
                    return null;
                }

                return new Dictionary<string, string> {
                    { "Authorization", $"Bearer {SignInResponse.jwt}" }
                };
            }
        }

        public AuthenticationService(IUsernameValidator usernameValidator, IEmailValidator emailValidator, IPasswordValidator passwordValidator, IAuthenticationRequester authenticationRequester) {
            this.usernameValidator = usernameValidator;
            this.emailValidator = emailValidator;
            this.passwordValidator = passwordValidator;
            this.authenticationRequester = authenticationRequester;
        }

        public virtual void SignUp(SignUpRequest data, Action<SignInResponse> OnSuccess, Action<HttpError> OnError) {
            if (!usernameValidator.IsValid(data.username, out string message)) {
                OnError?.Invoke(new HttpError(null, new InfoError(-1, "Error", message, null)));
                return;
            }

            if (!emailValidator.IsValid(data.email, out message)) {
                OnError?.Invoke(new HttpError(null, new InfoError(-1, "Error", message, null)));
                return;
            }

            if (!passwordValidator.IsValid(data.password, out message)) {
                OnError?.Invoke(new HttpError(null, new InfoError(-1, "Error", message, null)));
                return;
            }

            authenticationRequester.SignUp(data, (signInResponse) => {
                SignInResponse = signInResponse;
                OnSuccess?.Invoke(SignInResponse);
            }, OnError);
        }

        public virtual void SignIn(SignInRequest data, Action<SignInResponse> OnSuccess, Action<HttpError> OnError) {
            if (!emailValidator.IsValid(data.identifier, out string message)) {
                OnError?.Invoke(new HttpError(null, new InfoError(-1, "Error", message, null)));
                return;
            }

            if (!passwordValidator.IsValid(data.password, out message)) {
                OnError?.Invoke(new HttpError(null, new InfoError(-1, "Error", message, null)));
                return;
            }

            authenticationRequester.SignIn(data, (signInResponse) => {
                SignInResponse = signInResponse;
                OnSuccess?.Invoke(SignInResponse);
            }, OnError);
        }

        public virtual void SignOut() {
            SignInResponse.jwt = string.Empty; // This
            SignInResponse.user = null; // and this, are to ensure that any class with a reference of the SignInResponse lose the user data. Anyway, they can save a direct reference to the user data.
            SignInResponse = null; // This is the unic line neccesary to do a SignOut.
        }

        public virtual void DeleteAccount() {

        }

        public virtual void RecoveryPassword(RecoveryPasswordRequest data, Action<object> OnSuccess, Action<HttpError> OnError) {
            if (!emailValidator.IsValid(data.email, out string message)) {
                OnError?.Invoke(new HttpError(null, new InfoError(-1, "Error", message, null)));
                return;
            }

            authenticationRequester.RecoveryPassword(data, OnSuccess, OnError);
        }

        public virtual void GetUserMe(Action<User> OnSuccess, Action<HttpError> OnError) {
            authenticationRequester.GetUserMe(OnSuccess, OnError);
        }
    }
}