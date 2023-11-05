using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

using UnityEngine;

namespace LB.FileLoader.Runtime {

    public interface IFileLoaderService {

        public void GetFromCacheOrDownload(MediaFile file, Action<byte[]> onSuccess, Action<HttpError> onError);

        public Sprite BytesToSprite(byte[] bytes);
    }
}