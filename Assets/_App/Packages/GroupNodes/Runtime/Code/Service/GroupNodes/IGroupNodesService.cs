using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public interface IGroupNodesService {

        public void Get(int id, Action<StrapiResponse<Data<GroupNodeEntity>>> onSuccess, Action<HttpError> onError);

        public void Populate(Data<SubGroupNodesEntity> subGroupNodesEntity, Action<Data<SubGroupNodesEntity>> onSuccess, Action<HttpError> onError);

        public void Populate(Data<NodeEntity> nodeEntity, Action<Data<NodeEntity>> onSuccess, Action<HttpError> onError);
    }
}