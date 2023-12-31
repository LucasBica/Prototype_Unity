using System;
using System.Collections.Generic;

using LB.TweenSystem.Runtime.Internal;

using UnityEngine;

namespace LB.TweenSystem.Runtime {

    public class Tween {

        #region Static Funtions

        public static uint tweenCount = 0;
        private static List<Tween> tweenList;
        private static Stack<Tween> tweenRemoved;

        public static readonly Tween INSTANT = new(0f);

        public static bool Initialized { get; private set; }

        public static bool UseTryCatch { get; private set; }

        public static bool RecycleTweens { get; private set; } = true;

        static Tween() {
            Initialize();
        }

        public static void Initialize(int tweenCapacity = 16) {
            if (!Application.isPlaying || Initialized) {
                return;
            }

            tweenList = new List<Tween>(tweenCapacity);
            tweenRemoved = new Stack<Tween>(tweenCapacity);
            TweenSingleton.Instance.OnUpdate += TweenSingleton_OnUpdate;

            Initialized = true;
        }

        private static void TweenSingleton_OnUpdate() {
            float scaleDeltaTime = UnityEngine.Time.deltaTime;
            float unscaledDeltaTime = UnityEngine.Time.unscaledDeltaTime;

            if (UseTryCatch) {
                try {
                    UpdateTweenList(scaleDeltaTime, unscaledDeltaTime);
                } catch (Exception ex) {
                    Debug.LogError($"[{nameof(Tween)}] {ex.Message}");
                }
            } else {
                UpdateTweenList(scaleDeltaTime, unscaledDeltaTime);
            }

            static void UpdateTweenList(float scaleDeltaTime, float unscaledDeltaTime) {
                Tween[] tweens = tweenList.ToArray();
                for (int i = 0; i < tweens.Length; i++) {
                    tweens[i].Update(scaleDeltaTime, unscaledDeltaTime);
                }
            }
        }

        public static Tween New(float duration) {
            if (!Application.isPlaying) {
                return null;
            }

            TweenSingleton.CreateIfNotExist();

            if (duration <= 0f) {
                Debug.LogError($"[{nameof(Tween)}] {nameof(duration)} is less that zero.");
                return null;
            }

            if (RecycleTweens && tweenRemoved.TryPop(out Tween tween)) {
                tween.Reset();
                tween.Initialize(duration);
            } else {
                tween = new Tween(duration);
            }

            Add(tween);
            return tween;
        }

        private static void Add(Tween tween) {
            if (tween == null) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(Add)}] Cannot add this tween because is null");
                return;
            }

            if (tweenList.Contains(tween)) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(Add)}] Cannot add this tween because already was added. {nameof(Tween)} Id: {tween.Id}");
                return;
            }

            Initialize();
            tweenList.Add(tween);
        }

        private static void Remove(Tween tween) {
            if (tween == null) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(Add)}] Cannot remove this tween because is null");
                return;
            }

            if (!tweenList.Contains(tween)) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(Add)}] Cannot remove this tween because is not contained in the list. {nameof(Tween)} Id: {tween.Id}");
                return;
            }

            tweenList.Remove(tween);

            if (RecycleTweens) {
                tweenRemoved.Push(tween);
            }
        }

        public static void StopAll(bool immediatly = true, bool callOnComplete = false, StopTypes stopType = StopTypes.Interrupt) {
            if (!Application.isPlaying) {
                return;
            }

            for (int i = 0; i < tweenList.Count; i++) {
                Tween.Stop(tweenList[i], immediatly, callOnComplete, stopType);
            }

            tweenList.Clear();
        }

        /// <summary>
        /// This method will stop interpolation only if it has the same id. Do not pass the id as a parameter using 'tween.Id', you should use the id that was saved when the tween was created.
        /// </summary>
        /// <param name="id">Id of the tween that want stop</param>
        /// <param name="tween">Tween that had or has the id passed previously</param>
        /// <param name="stopType">The way to stop it</param>
        /// <param name="immediatly">Stop it exactly now or use a interpolation to do it</param>
        /// <param name="callOnComplete">Call the event OnComplete if pass true</param>
        /// <returns></returns>
        public static Tween StopIfHasSameId(uint id, Tween tween, bool immediatly = true, bool callOnComplete = false, StopTypes stopType = StopTypes.Interrupt) {
            if (!Application.isPlaying) {
                return null;
            }

            if (tween == null || id == 0 || id != tween.Id) {
                return null;
            }

            if (!tweenList.Contains(tween)) {
                return null;
            }

            return Tween.Stop(tween, immediatly, callOnComplete, stopType);
        }

        /// <summary>
        /// Attention! This method could stop a tween that is used by another object, to avoid unexpected behaviours, use <see cref="StopIfHasSameId(uint, Tween, StopTypes, bool, bool)"/>
        /// </summary>
        public static Tween Stop(Tween tween, bool immediatly = true, bool callOnComplete = false, StopTypes stopType = StopTypes.Interrupt) {
            if (!Application.isPlaying) {
                return null;
            }

            if (tween == null || !tween.IsPlaying) {
                return null;
            }

            tween.Stop(stopType, immediatly, callOnComplete);
            return tween;
        }

        #endregion Static Functions

        #region Object Functions

        private event Action<Tween> OnUpdate;
        private event Action<Tween> OnCompleteLoop;
        private event Action<Tween> OnCompletePingPong;
        private event Action<Tween> OnComplete;

        public uint Id { get; private set; }

        public float Duration { get; private set; }

        public TweenTypes TweenType { get; private set; }

        public int LoopCount { get; private set; } = 1; // -1 is infinite, 0 is a not supported value, 1 is linear and the rest positive values are the loopCount.

        public bool IsPingPong { get; private set; }

        public bool UseUnscaledDeltaTime { get; private set; }

        public float DeltaMultiplier { get; private set; } = 1f;

        public float ElapsedTime { get; private set; }

        public float TweenedTime { get; private set; }

        public int LoopTimes { get; private set; }

        public float Delay { get; private set; }

        public float RemainingDelay { get; private set; }

        public float Time { get; private set; }

        public StopTypes StopType { get; private set; }

        public bool IsStopping { get; private set; }

        public bool IsStopped { get; private set; }

        public bool IsComplete { get; private set; }

        public bool CallOnCompleteWhenStop { get; private set; }

        public bool IsPlaying => Id > 0 && Duration > 0f && !IsStopped && !IsComplete;

        public bool InstanceInitialized => Id > 0 && Duration > 0f;

        private Tween(float duration) {
            Initialize(duration);
        }

        private void Initialize(float duration) {
            if (duration <= 0f) {
                return;
            }

            Duration = duration;
            Id = ++tweenCount;
        }

        private void Reset() {
            OnUpdate = null;
            OnCompleteLoop = null;
            OnCompletePingPong = null;
            OnComplete = null;

            Id = 0;
            Duration = 0f;
            TweenType = TweenTypes.Linear;
            LoopCount = 1;
            IsPingPong = false;
            DeltaMultiplier = 1f;
            ElapsedTime = 0f;
            TweenedTime = 0f;
            LoopTimes = 0;
            Delay = 0f;
            RemainingDelay = 0f;
            Time = 0f;
            StopType = StopTypes.Interrupt;
            IsStopping = false;
            IsStopped = false;
            IsComplete = false;
        }

        public Tween SetDelay(float delay) {
            if (!InstanceInitialized) {
                return null;
            }

            Delay = delay;
            RemainingDelay = delay;
            return this;
        }

        public Tween SetLinear() {
            if (!InstanceInitialized) {
                return null;
            }

            if (!IsPlaying) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(SetLinear)}] Cannot {nameof(SetLinear)} because {nameof(IsPlaying)} is false.");
                return this;
            }

            if (IsStopping) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(SetLinear)}] Cannot {nameof(SetLinear)} because {nameof(IsStopping)} is true.");
                return this;
            }

            LoopCount = 0;
            IsPingPong = false;
            TweenType = GetTweenType();
            return this;
        }

        public Tween SetLoopCount(int loopCount) {
            if (!InstanceInitialized) {
                return null;
            }

            if (!IsPlaying) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(SetLoopCount)}] Cannot {nameof(SetLoopCount)} because {nameof(IsPlaying)} is false.");
                return this;
            }

            if (IsStopping) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(SetLoopCount)}] Cannot {nameof(SetLoopCount)} because {nameof(IsStopping)} is true.");
                return this;
            }

            loopCount = loopCount < 0 ? -1 : loopCount > 1 ? loopCount : 1;
            LoopCount = loopCount;
            TweenType = GetTweenType();
            return this;
        }

        public Tween SetUseUnscaledDeltaTime(bool value) {
            if (!InstanceInitialized) {
                return null;
            }

            UseUnscaledDeltaTime = value;

            return this;
        }

        public Tween SetPingPong(bool active) {
            if (!InstanceInitialized) {
                return null;
            }

            if (!IsPlaying) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(SetPingPong)}] Cannot {nameof(SetPingPong)} because {nameof(IsPlaying)} is false.");
                return this;
            }

            if (IsStopping) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(SetPingPong)}] Cannot {nameof(SetPingPong)} because {nameof(IsStopping)} is true.");
                return this;
            }

            IsPingPong = active;
            TweenType = GetTweenType();
            return this;
        }

        public Tween SetUpdateAction(Action<Tween> action) {
            if (!InstanceInitialized) {
                return null;
            }

            if (!IsPlaying) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(SetCompleteAction)}] Cannot add an action {nameof(OnUpdate)} because it is not playing.");
                return this;
            }

            OnUpdate += action;
            Update(0f, 0f);
            return this;
        }

        public Tween SetCompleteLoopAction(Action<Tween> action) {
            if (!InstanceInitialized) {
                return null;
            }

            if (!IsPlaying) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(SetCompleteLoopAction)}] Cannot add an action {nameof(OnCompleteLoop)} because it is not playing.");
                return this;
            }

            OnCompleteLoop += action;
            return this;
        }

        public Tween SetCompletePingPongAction(Action<Tween> action) {
            if (!InstanceInitialized) {
                return null;
            }

            if (!IsPlaying) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(SetCompletePingPongAction)}] Cannot add an action {nameof(OnCompletePingPong)} because it is not playing.");
                return this;
            }

            OnCompletePingPong += action;
            return this;
        }

        public Tween SetCompleteAction(Action<Tween> action) {
            if (!InstanceInitialized) {
                return null;
            }

            if (!IsPlaying) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(SetCompleteAction)}] Cannot add an action {nameof(OnComplete)} because it is not playing.");
                return this;
            }

            OnComplete += action;
            return this;
        }

        public void Stop(StopTypes stopType = StopTypes.Interrupt, bool immediatly = true, bool callOnComplete = false) {
            if (!InstanceInitialized) {
                return;
            }

            if (!IsPlaying) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(Stop)}] Cannot {nameof(Stop)} because {nameof(IsPlaying)} is false.");
                return;
            }

            if (IsStopping) {
                Debug.LogError($"[{nameof(Tween)}.{nameof(Stop)}] Cannot {nameof(Stop)} because {nameof(IsStopping)} is true.");
                return;
            }

            StopType = stopType;
            LoopCount = 0;

            switch (stopType) {
                case StopTypes.Interrupt:
                    immediatly = true;
                    break;
                case StopTypes.OnZero:
                    if (immediatly) Time = 0f;
                    else DeltaMultiplier = TweenType == TweenTypes.PingPong && LoopTimes % 2 == 1 ? 1f : (-1f);
                    break;
                case StopTypes.OnOne:
                    if (immediatly) Time = 1f;
                    else DeltaMultiplier = TweenType == TweenTypes.PingPong && LoopTimes % 2 == 1 ? (-1f) : 1f;
                    break;
                case StopTypes.Closer:
                    if (immediatly) Time = Time >= 0.5f ? 1f : 0f;
                    else DeltaMultiplier = Time < 0.5f ? (TweenType == TweenTypes.PingPong && LoopTimes % 2 == 1 ? 1f : (-1f)) : (TweenType == TweenTypes.PingPong && LoopTimes % 2 == 1 ? (-1f) : 1f);
                    break;
                case StopTypes.CompleteLoop:
                    if (immediatly) Time = LoopCount % 2 == 0 ? 1f : 0f;
                    else LoopCount = Mathf.Max(LoopTimes + 1, 1);
                    break;
                default:
                    Debug.LogError($"[{nameof(Tween)}.{nameof(Stop)}] {nameof(StopTypes)} not implemented: {stopType}");
                    break;
            }

            if (immediatly) {
                Tween.Remove(this);
                IsStopped = true;
                OnUpdate?.Invoke(this);

                if (callOnComplete) {
                    OnComplete?.Invoke(this);
                }
            } else {
                IsStopping = true;
                CallOnCompleteWhenStop = callOnComplete;
            }
        }

        public void Update(float scaleDeltaTime, float unscaledDeltaTime) {
            if (!InstanceInitialized) {
                return;
            }

            float deltaTime = UseUnscaledDeltaTime ? unscaledDeltaTime : scaleDeltaTime;

            if (RemainingDelay > 0f) {
                RemainingDelay -= deltaTime;

                if (RemainingDelay > 0f) {
                    return;
                }

                if (RemainingDelay < 0f) {
                    deltaTime = -RemainingDelay;
                } else {
                    deltaTime = 0f;
                }

                RemainingDelay = 0f;
            }

            deltaTime *= DeltaMultiplier; // This can be only 1 or -1. It is used to make a smooth stop.

            int lastLoopTimes = LoopTimes;

            ElapsedTime += deltaTime;
            TweenedTime = GetTweenedTime();
            LoopTimes = (int)(ElapsedTime / Duration);

            bool callOnCompleteLoop = false;
            bool callOnCompletePingPong = false;
            bool callOnComplete = false;

            switch (TweenType) {
                case TweenTypes.Linear:
                    callOnComplete = Time == 1f;
                    break;
                case TweenTypes.Loop:
                    callOnCompleteLoop = LoopTimes != lastLoopTimes || (DeltaMultiplier < 0f && ElapsedTime <= 0f);
                    callOnComplete = callOnCompleteLoop && ((LoopCount >= 0 && LoopTimes >= LoopCount) || IsStopping);
                    break;
                case TweenTypes.PingPong:
                    bool isLoopTimesChanged = LoopTimes != lastLoopTimes || (DeltaMultiplier < 0f && ElapsedTime <= 0f);
                    bool didManyLoopsInOneFrame = Mathf.Abs(LoopTimes - lastLoopTimes) > 1; // This will be true when the tween.Duration is less that the deltaTime.
                    bool isOdd = LoopTimes % 2 == 0;
                    callOnCompleteLoop = isLoopTimesChanged && (!isOdd || didManyLoopsInOneFrame);
                    callOnCompletePingPong = isLoopTimesChanged && (isOdd || didManyLoopsInOneFrame);
                    callOnComplete = (callOnCompleteLoop || callOnCompletePingPong) && (LoopTimes >= LoopCount || IsStopping);
                    break;
                default:
                    Debug.LogError($"[{nameof(Tween)}.{nameof(Update)}] {nameof(TweenTypes)} not implemented: {TweenType}");
                    break;
            }

            if (callOnComplete) {
                Tween.Remove(this);
                IsStopped = IsStopping;
                IsStopping = false;
                IsComplete = true;

                if (IsStopped) {
                    callOnComplete = CallOnCompleteWhenStop;
                }

                Time = IsStopped ? Time : Time >= 0.5f ? 1f : 0f;
            } else {
                Time = TweenedTime / Duration;
            }

            OnUpdate?.Invoke(this);

            if (callOnCompleteLoop && !IsStopped || (callOnCompleteLoop && IsStopped && CallOnCompleteWhenStop)) {
                OnCompleteLoop?.Invoke(this);
            }

            if (callOnCompletePingPong && !IsStopped || (callOnCompletePingPong && IsStopped && CallOnCompleteWhenStop)) {
                OnCompletePingPong?.Invoke(this);
            }

            if (callOnComplete && !IsStopped || (callOnComplete && IsStopped && CallOnCompleteWhenStop)) {
                OnComplete?.Invoke(this);
            }
        }

        private TweenTypes GetTweenType() {
            if (!InstanceInitialized) {
                return default;
            }

            if (IsPingPong) {
                return TweenTypes.PingPong;
            } else if (LoopCount < 0 || LoopCount > 1) {
                return TweenTypes.Loop;
            } else {
                return TweenTypes.Linear;
            }
        }

        private float GetTweenedTime() {
            if (!InstanceInitialized) {
                return 0f;
            }

            switch (TweenType) {
                case TweenTypes.Linear:
                    return ElapsedTime > Duration ? Duration : ElapsedTime;
                case TweenTypes.Loop:
                    return ElapsedTime % Duration;
                case TweenTypes.PingPong:
                    float c = (ElapsedTime - Duration) / (Duration * 2f);
                    return Mathf.Abs((c - Mathf.Floor(c)) * Duration * 2f - Duration);
                default:
                    Debug.LogError($"[{nameof(Tween)}.{nameof(GetTweenedTime)}] {nameof(TweenTypes)} not implemented: {TweenType}");
                    return 0f;
            }
        }

        #endregion Object Functions

        public enum TweenTypes {
            Linear,
            Loop,
            PingPong,
        }

        public enum StopTypes {
            Interrupt,
            OnZero,
            OnOne,
            Closer,
            CompleteLoop,
        }
    }
}