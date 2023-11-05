using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Serializables {

    public abstract class UIColorVertexBase : ScriptableObject {

        public abstract void ModifyMesh(Rect rect, VertexHelper vertexHelper);
    }
}