using System;

using Newtonsoft.Json;

namespace LB.Http.Models {

    public class EntryModel {

        public DateTime createdAt;
        public DateTime updatedAt;

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class HttpError {

        public object data;
        public InfoError error;

        public HttpError(object data, InfoError error) {
            this.data = data;
            this.error = error;
        }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class InfoError {

        public int status;
        public string name;
        public string message;
        public object details;

        public InfoError(int status, string name, string message, object details) {
            this.status = status;
            this.name = name;
            this.message = message;
            this.details = details;
        }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}