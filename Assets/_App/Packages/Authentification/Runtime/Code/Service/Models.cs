using System;

using LB.Core.Runtime.Utilities;
using LB.Http.Models;

using Newtonsoft.Json;

namespace LB.Authentication.Models {

    [Serializable]
    public class SignUpRequest {

        public string username;
        public string email;
        public string password;

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }
    }

    public class SignInRequest {

        public string identifier;
        public string password;

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }
    }

    public class SignInResponse {

        public string jwt;
        public User user;

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }
    }

    public class RecoveryPasswordRequest {

        public string email;

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }
    }

    public class User : EntryModel {

        public int id;
        public string username;
        public string email;
        public string provider;
        public bool confirmed;
        public bool blocked;

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }
    }
}
