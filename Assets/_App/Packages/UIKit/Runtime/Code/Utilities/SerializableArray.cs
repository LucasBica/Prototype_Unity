using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    [System.Serializable]
    public class SerializableArray<T> {

        [SerializeField] public T[] array = new T[0];

        public T this[int i] => array[i];

        public int Length => array.Length;
    }
}