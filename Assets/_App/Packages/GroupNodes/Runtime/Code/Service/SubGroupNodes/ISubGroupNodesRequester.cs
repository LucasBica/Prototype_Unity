using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public interface ISubGroupNodesRequester {

        public void Get(int id, Action<StrapiResponse<Data<SubGroupNodesEntity>>> onSuccess, Action<HttpError> onError);
    }
}