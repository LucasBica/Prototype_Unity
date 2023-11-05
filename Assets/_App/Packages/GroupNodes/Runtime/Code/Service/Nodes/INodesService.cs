using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public interface INodesService {

        public void Get(int id, Action<StrapiResponse<Data<NodeEntity>>> onSuccess, Action<HttpError> onError);
    }
}