using System;
using System.Collections.Generic;

using UnityEngine;

namespace LB.TweenSystem.Runtime.Components {

    public class TweenTransform : TweenComponent<Transform> {

        [Header("Settings")]
        [SerializeField] protected TransformProperty[] properties = default;

        public Dictionary<TransformPropertyTypes, Action<Tween, TransformProperty>> propertyActionPair;

        protected virtual void Awake() {
            propertyActionPair = new() {
                {TransformPropertyTypes.GlobalPosition, OnGlobalPosition },
                {TransformPropertyTypes.LocalPosition, OnLocalPosition },
                {TransformPropertyTypes.GlobalRotation, OnGlobalRotation },
                {TransformPropertyTypes.LocalRotation, OnLocalRotation },
                {TransformPropertyTypes.LocalScale, OnLocalScale }
            };
        }

        protected override void OnUpdate(Tween tween) {
            for (int i = 0; i < properties.Length; i++) {
                propertyActionPair[properties[i].propertyType]?.Invoke(tween, properties[i]);
            }
        }

        public void OnGlobalPosition(Tween tween, TransformProperty property) {
            target.position = Vector3.LerpUnclamped(property.from, property.to, property.curve.Evaluate(tween.TweenedTime));
        }

        public void OnLocalPosition(Tween tween, TransformProperty property) {
            target.localPosition = Vector3.LerpUnclamped(property.from, property.to, property.curve.Evaluate(tween.TweenedTime));
        }

        public void OnGlobalRotation(Tween tween, TransformProperty property) {
            target.eulerAngles = Vector3.LerpUnclamped(property.from, property.to, property.curve.Evaluate(tween.TweenedTime));
        }

        public void OnLocalRotation(Tween tween, TransformProperty property) {
            target.localEulerAngles = Vector3.LerpUnclamped(property.from, property.to, property.curve.Evaluate(tween.TweenedTime));
        }

        public void OnLocalScale(Tween tween, TransformProperty property) {
            target.localScale = Vector3.LerpUnclamped(property.from, property.to, property.curve.Evaluate(tween.TweenedTime));
        }

        [System.Serializable]
        public class TransformProperty : ComponentProperty<TransformPropertyTypes, Vector3> {

        }

        public enum TransformPropertyTypes {
            GlobalPosition,
            LocalPosition,
            GlobalRotation,
            LocalRotation,
            LocalScale
        }
    }
}