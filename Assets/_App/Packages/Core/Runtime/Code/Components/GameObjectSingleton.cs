using UnityEngine;

namespace LB.Core.Runtime.Components {

    public class GameObjectSingleton : MonoBehaviour {

        public void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}