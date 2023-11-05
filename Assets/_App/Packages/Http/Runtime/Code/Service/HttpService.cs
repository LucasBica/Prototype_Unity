using System;
using System.Collections.Generic;
using System.Text;

using Cysharp.Threading.Tasks;

using LB.Http.Models;

using Newtonsoft.Json;

using UnityEngine;
using UnityEngine.Networking;

namespace LB.Http.Runtime {

    public class HttpService : IHttpService {

        protected readonly JsonSerializerSettings jsonSerializerSettings;
        protected readonly Func<HttpError> funcHttpError;

        public HttpService(JsonSerializerSettings jsonSerializerSettings, Func<HttpError> funcHttpError) {
            this.funcHttpError = funcHttpError;
        }

        public virtual void SendRequest<T>(Request request, Action<T> onSuccess, Action<HttpError> onError) {
            if (request == null) {
                HttpError httpError = funcHttpError.Invoke();
                httpError.error.details = $"[{nameof(HttpService)}] {nameof(Request)} is null";
                onError?.Invoke(httpError);
                return;
            }

            if (!request.HeadersContainsKey("Accept")) {
                request.SetHeader("Accept", "application/json");
            }

            if (!request.HeadersContainsKey("Content-Type")) {
                request.SetHeader("Content-Type", "application/json");
            }

            SendRequest(request,
                (req, res) => {
                    if (res.isSuccess) {
                        Debug.Log($"[{nameof(HttpService)}] Success: {req.Endpoint}\n{res}");

                        bool isSuccessDeserialization = false;
                        T response = default;

                        try {
                            response = JsonConvert.DeserializeObject<T>(res.text, jsonSerializerSettings);
                            isSuccessDeserialization = true;
                        } catch (Exception ex) {
                            HttpError httpError = funcHttpError.Invoke();
                            httpError.error.details = $"The function DeserializeObject throw an exception:\n{ex.Message}";
                            onError?.Invoke(httpError);
                        } finally {
                            if (isSuccessDeserialization) {
                                onSuccess?.Invoke(response);
                            }
                        }

                    } else {
                        Debug.LogError($"[{nameof(HttpService)}] Error: {req.Endpoint}\n{res}");

                        HttpError httpError = null;

                        try {
                            if (res.isConnectionSuccess) {
                                httpError = JsonConvert.DeserializeObject<HttpError>(res.text);
                            } else {
                                httpError = funcHttpError.Invoke();
                                httpError.error.details = res.text;
                            }
                        } catch (Exception ex) {
                            httpError = funcHttpError.Invoke();
                            httpError.error.details = ex == null ? res.text : ex.Message;
                        }

                        onError?.Invoke(httpError);
                    }
                }
            );
        }

        public virtual void SendRequest(Request request, Action<byte[]> onSuccess, Action<HttpError> onError) {
            if (request == null) {
                HttpError httpError = funcHttpError.Invoke();
                httpError.error.details = $"[{nameof(HttpService)}] {nameof(Request)} is null";
                onError?.Invoke(httpError);
                return;
            }

            SendRequest(request,
                (req, res) => {
                    if (res.isSuccess) {
                        Debug.Log($"[{nameof(HttpService)}] Success: {req.Endpoint}\n{res}");
                        onSuccess?.Invoke(res.data);

                    } else {
                        Debug.LogError($"[{nameof(HttpService)}] Error: {req.Endpoint}\n{res}");

                        HttpError httpError = null;

                        try {
                            if (res.isConnectionSuccess) {
                                httpError = JsonConvert.DeserializeObject<HttpError>(res.text);
                            } else {
                                httpError = funcHttpError.Invoke();
                                httpError.error.details = res.text;
                            }
                        } catch (Exception ex) {
                            httpError = funcHttpError.Invoke();
                            httpError.error.details = ex == null ? res.text : ex.Message;
                        }

                        onError?.Invoke(httpError);
                    }
                }
            );
        }

        public virtual void SendRequest(Request request, Action<Request, Response> OnResponse) {
            UnityWebRequest unityWebRequest = CreateUnityWebRequest(request);
            UniTask.Create(async () => {
                UnityWebRequestAsyncOperation asyncOperation = unityWebRequest.SendWebRequest();

                while (!asyncOperation.isDone) {
                    await UniTask.Yield();
                    request.Progress = asyncOperation.progress;
                }

                request.IsComplete = true;

                unityWebRequest = asyncOperation.webRequest;
                DownloadHandler downloadHandler = unityWebRequest.downloadHandler;

                bool isSuccess = unityWebRequest.result == UnityWebRequest.Result.Success;
                bool isConnectionSuccess = unityWebRequest.result == UnityWebRequest.Result.Success || unityWebRequest.result == UnityWebRequest.Result.ProtocolError;
                string text = isSuccess ? unityWebRequest.downloadHandler.text : isConnectionSuccess ? unityWebRequest.downloadHandler.text : unityWebRequest.error;
                byte[] data = unityWebRequest.downloadHandler.data;

                Response response = new(isSuccess, isConnectionSuccess, text, data);

                OnResponse?.Invoke(request, response);
            });
        }

        protected virtual UnityWebRequest CreateUnityWebRequest(Request request) {
            UnityWebRequest unityWebRequest = new() {
                url = request.Endpoint,
                method = request.HttpMethodType.ToString().ToUpper(),
                downloadHandler = new DownloadHandlerBuffer(),
                uploadHandler = new UploadHandlerRaw(request.HasBody && request.IsBodyText ? Encoding.UTF8.GetBytes(request.BodyAsText) : request.BodyAsData),
                timeout = request.Timeout
            };

            Dictionary<string, string> headers = request.Headers;

            if (headers != null) {
                foreach (KeyValuePair<string, string> kvp in headers) {
                    unityWebRequest.SetRequestHeader(kvp.Key, kvp.Value);
                }
            }

            return unityWebRequest;
        }
    }
}