using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public interface IGroupNodesRequester {

        public void Get(int id, Action<StrapiResponse<Data<GroupNodeEntity>>> onSuccess, Action<HttpError> onError);
    }
}