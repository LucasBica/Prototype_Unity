using System.Collections.Generic;

namespace LB.Http.Runtime {

    public struct RequestBuilder {

        public HttpMethodTypes MethodType { get; private set; }
        public string Endpoint { get; private set; }
        public Dictionary<string, string> Parameters { get; private set; }
        public Dictionary<string, string> Headers { get; private set; }
        public object Body { get; private set; }
        public int Timeout { get; private set; }

        public RequestBuilder(HttpMethodTypes methodType, string endpoint) {
            MethodType = methodType;
            Endpoint = endpoint;

            Parameters = null;
            Headers = null;
            Body = null;
            Timeout = 10;
        }

        public RequestBuilder AddParameters(Dictionary<string, string> parameters) {
            if (parameters == null) {
                return this;
            }

            foreach (KeyValuePair<string, string> kvp in parameters) {
                SetParameter(kvp.Key, kvp.Value);
            }

            return this;
        }

        public RequestBuilder SetParameters(Dictionary<string, string> parameters) {
            Parameters = parameters;
            return this;
        }

        public RequestBuilder SetParameter(string key, string value) {
            Parameters ??= new Dictionary<string, string>();

            if (Parameters.ContainsKey(key)) {
                Parameters[key] = value;
            } else {
                Parameters.Add(key, value);
            }

            return this;
        }

        public RequestBuilder AddHeaders(Dictionary<string, string> headers) {
            if (headers == null) {
                return this;
            }

            foreach (KeyValuePair<string, string> kvp in headers) {
                AddHeader(kvp.Key, kvp.Value);
            }

            return this;
        }

        public RequestBuilder SetHeaders(Dictionary<string, string> headers) {
            Headers = headers;
            return this;
        }

        public RequestBuilder AddHeader(string key, string value) {
            Headers ??= new Dictionary<string, string>();

            if (Headers.ContainsKey(key)) {
                Headers[key] = value;
            } else {
                Headers.Add(key, value);
            }

            return this;
        }

        public RequestBuilder SetBody(string body) {
            Body = body;
            return this;
        }

        public RequestBuilder SetBody(byte[] body) {
            Body = body;
            return this;
        }

        public RequestBuilder SetTimeout(int timeout) {
            Timeout = timeout;
            return this;
        }

        public Request Build() {
            return new Request(this);
        }
    }
}