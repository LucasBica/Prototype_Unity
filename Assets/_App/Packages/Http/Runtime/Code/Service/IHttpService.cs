using System;

using LB.Http.Models;

namespace LB.Http.Runtime {

    public interface IHttpService {

        public void SendRequest<T>(Request request, Action<T> onSuccess, Action<HttpError> onError);

        public void SendRequest(Request request, Action<byte[]> onSuccess, Action<HttpError> onError);
    }
}