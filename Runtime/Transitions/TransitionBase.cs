using System.Collections;
using UnityEngine;

namespace Tellory.StackableUI.Transitions
{
    public abstract class TransitionBase : MonoBehaviour, ITransition
    {
        // Events
        public event System.Action OnShowStartedEvent;
        public event System.Action OnShowCompletedEvent;
        public event System.Action OnHideStartedEvent;
        public event System.Action OnHideCompletedEvent;

        // Fields
        [SerializeField]
        private AnimationTransitionCurve m_showAnimation;

        [SerializeField]
        private AnimationTransitionCurve m_hideAnimation;

        private Coroutine m_currentTransition;

        public float ShowDuration => m_showAnimation.Duration;
        public float HideDuration => m_hideAnimation.Duration;

        public TransitionState State
        {
            get;
            private set;
        }

        // Methods
        protected virtual void Awake() { }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable()
        {
            HideInstantly();
        }

        public void Show(bool useScaledTime)
        {
            if (State != TransitionState.Idle)
                ForceStop();

            m_currentTransition = StartCoroutine(DoTransition(true, useScaledTime));
        }

        public void ForceStop()
        {
            if (m_currentTransition == null)
                return;

            State = TransitionState.Idle;
            StopCoroutine(m_currentTransition);
            m_currentTransition = null;
        }

        public void Hide(bool useScaledTime)
        {
            if (State != TransitionState.Idle)
                ForceStop();

            m_currentTransition = StartCoroutine(DoTransition(false, useScaledTime));
        }

        public void ShowInstantly()
        {
            ForceStop();
            ApplyShowState();
        }

        public void HideInstantly()
        {
            ForceStop();
            ApplyHideState();
        }

        protected abstract void ApplyShowState();
        protected abstract void ApplyHideState();
        protected abstract void ApplyTransitionValue(float normalizedValue);

        protected virtual void OnShowStarted()
        {
            OnShowStartedEvent?.Invoke();
        }

        protected virtual void OnShowCompleted()
        {
            OnShowCompletedEvent?.Invoke();
        }

        protected virtual void OnHideStarted()
        {
            OnHideStartedEvent?.Invoke();
        }

        protected virtual void OnHideCompleted()
        {
            OnHideCompletedEvent?.Invoke();
        }

        // Coroutines
        private IEnumerator DoTransition(bool show, bool useScaledTime)
        {
            State = show ? TransitionState.Showing : TransitionState.Hiding;

            if (show)
                OnShowStarted();

            else
                OnHideStarted();

            AnimationTransitionCurve selectedTransitionTime = show ? m_showAnimation : m_hideAnimation;
            float duration = selectedTransitionTime.Duration;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float normalizedValue = selectedTransitionTime.Evaluate(elapsed);
                ApplyTransitionValue(show ? normalizedValue : 1f - normalizedValue);

                if (useScaledTime)
                    elapsed += Time.deltaTime;

                else
                    elapsed += Time.unscaledDeltaTime;

                yield return null;
            }

            if (show)
            {
                ApplyShowState();
                OnShowCompleted();
            }
            else
            {
                ApplyHideState();
                OnHideCompleted();
            }

            State = TransitionState.Idle;
        }
    }
}
