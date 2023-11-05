using Newtonsoft.Json;

namespace LB.Http.Runtime {

    public class Response {

        public bool isSuccess;
        public bool isConnectionSuccess;
        public string text;
        public byte[] data;

        public Response(bool isSuccess, bool isConnectionSuccess, string text, byte[] data) {
            this.isSuccess = isSuccess;
            this.isConnectionSuccess = isConnectionSuccess;
            this.text = text;
            this.data = data;
        }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}