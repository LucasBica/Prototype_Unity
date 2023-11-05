using System;

using LB.Core.Runtime;
using LB.Http.Models;
using LB.Http.Runtime;
using LB.Http.Runtime.Strapi;

using UnityEngine;

namespace LB.FileLoader.Runtime {

    public class FileLoaderService : IFileLoaderService {

        private readonly string prefixUrl;
        private readonly IHttpService httpService;
        private readonly ICacheAsBytesService cacheAsBytesService;

        private readonly Func<HttpError> funcHttpError;

        public FileLoaderService(string prefixUrl, IHttpService httpService, ICacheAsBytesService cacheAsBytesService, Func<HttpError> funcHttpError) {
            this.prefixUrl = prefixUrl;
            this.httpService = httpService;
            this.cacheAsBytesService = cacheAsBytesService;

            this.funcHttpError = funcHttpError;
        }

        public void GetFromCacheOrDownload(MediaFile file, Action<byte[]> onSuccess, Action<HttpError> onError) {
            if (cacheAsBytesService != null && cacheAsBytesService.HasKey(file.data.attributes.hash)) {
                byte[] bytes = cacheAsBytesService.Get(file.data.attributes.hash);
                if (bytes != null && bytes.Length > 0) {
                    onSuccess?.Invoke(bytes);
                }
            }

            Request request = new RequestBuilder(HttpMethodTypes.Get, prefixUrl + file.data.attributes.url)
                .Build();

            httpService.SendRequest(request,
                (response) => {
                    if (response != null && response.Length > 0) {
                        cacheAsBytesService?.Set(file.data.attributes.hash, response);
                        onSuccess?.Invoke(response);
                    } else {
                        HttpError httpError = funcHttpError.Invoke();
                        httpError.error.details = "The response is null or a byte array with a invalid length value";
                        onError?.Invoke(httpError);
                    }
                },
                onError
            );
        }

        public Sprite BytesToSprite(byte[] bytes) {
            Texture2D texture2D = new(1, 1);
            texture2D.LoadImage(bytes);

            return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(texture2D.width / 2, texture2D.height / 2));
        }
    }
}