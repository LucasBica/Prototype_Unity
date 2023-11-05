using UnityEngine;

namespace LB.TweenSystem.Runtime.Components {

    public abstract class TweenComponent<T> : MonoBehaviour where T : Component {

        [Header("Settings")]
        [SerializeField] protected bool playOnEnable = default;
        [SerializeField] protected float duration = default;
        [SerializeField] protected float delay = default;
        [SerializeField] protected int loopCount = -1;
        [SerializeField] protected bool isPingPong = default;

        [Header("References")]
        [SerializeField] protected T target = default;

        protected uint tweenId;
        protected Tween tween;

        protected virtual void OnEnable() {
            if (playOnEnable) {
                Play();
            }
        }

        protected virtual void OnDisable() {
            Stop();
        }

        public virtual void Play() {
            Stop();
            tween = Tween.New(duration).SetLoopCount(loopCount).SetPingPong(isPingPong).SetDelay(delay).SetUpdateAction(OnUpdate).SetCompleteAction(OnComplete);
            tweenId = tween.Id;
        }

        public virtual void Stop() {
            if (tween != null && tween.IsPlaying) {
                tween = Tween.StopIfHasSameId(tweenId, tween);
            }
        }

        protected virtual void OnUpdate(Tween tween) { }

        protected virtual void OnComplete(Tween tween) { }

        [System.Serializable]
        public class ComponentProperty<T1, T2> {
            public T1 propertyType;
            public T2 from;
            public T2 to;
            public AnimationCurve curve;
        }
    }
}