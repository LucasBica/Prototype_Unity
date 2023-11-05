using System;
using System.Collections.Generic;

using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    public class UITransitionCombinator<TView, TTransition> : UITransitionScriptableObject<TView> where TView : UIView where TTransition : UITransitionScriptableObject<TView> {

        [SerializeField] private AnimationCurve curve = default;
        [SerializeField] private float transitionTime = default;
        [Space]
        [SerializeField] protected TTransition[] transitions = default;

        public AnimationCurve Curve => curve;
        public float TransitionTime => transitionTime;

        protected override void OnValidate() {
            List<TTransition> list = new List<TTransition>(transitions.Length);

            for (int i = 0; i < transitions.Length; i++) {
                if (transitions[i] != null) {
                    list.Add(transitions[i]);
                }
            }

            transitions = list.ToArray();
        }

        public override void OnValidateView(TView view) {
            Array.ForEach(transitions, (transition) => { transition.OnValidateView(view); });
        }

        public override void OnEnableView(TView view) {
            Array.ForEach(transitions, (transition) => { transition.OnEnableView(view); });
        }

        public override void OnUpdateTransition(TView view, float time) {
            Array.ForEach(transitions, (transition) => { transition.OnUpdateTransition(view, Curve.Evaluate(time)); });
        }

        public override void OnCompleteTransition(TView view) {
            Array.ForEach(transitions, (transition) => { transition.OnCompleteTransition(view); });
        }
    }
}