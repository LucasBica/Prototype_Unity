using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace LB.Http.Runtime {

    public class Request {

        protected RequestBuilder builder;

        public event Action<Request, float> OnProgressValueChanged;
        public event Action<Request, bool> OnCompleted;

        public HttpMethodTypes HttpMethodType => builder.MethodType;

        public string Endpoint => builder.Endpoint;

        public Dictionary<string, string> Headers => new(builder.Headers);

        public int Timeout => builder.Timeout;

        public bool HasBody => builder.Body != null;

        public bool IsBodyText => builder.Body is string;

        public bool IsBodyData => builder.Body is byte[];

        public string BodyAsText => (string)builder.Body;

        public byte[] BodyAsData => (byte[])builder.Body;

        public object Body => builder.Body;

        protected float progress = 0f;
        public float Progress {
            get => progress;
            set {
                if (value == progress) {
                    return;
                }

                progress = value;
                OnProgressValueChanged?.Invoke(this, progress);
            }
        }

        protected bool isComplete = false;
        public bool IsComplete {
            get => isComplete;
            set {
                if (value == isComplete) {
                    return;
                }

                isComplete = value;
                OnCompleted?.Invoke(this, isComplete);
            }
        }

        public Request(RequestBuilder builder) {
            this.builder = builder;
        }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }

        public bool HeadersContainsKey(string key) {
            return builder.Headers != null && builder.Headers.ContainsKey(key);
        }

        public void SetHeader(string key, string value) {
            builder.AddHeader(key, value);
        }
    }
}