using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Tellory.StackableUI.Transitions
{
    public class CompositeTransition : MonoBehaviour, ITransition
    {
        // Fields
        [SerializeField]
        private List<MonoBehaviour> m_transitionBehaviours;
        private List<ITransition> m_transitions;

        private bool m_isTransitionInitialized;

        private float m_showDuration;
        private float m_hideDuration;

        public float ShowDuration
        {
            get
            {
                CheckSetup();
                return m_showDuration;
            }
        }

        public float HideDuration
        {
            get
            {
                CheckSetup();
                return m_hideDuration;
            }
        }


        public TransitionState State { get; private set; }

        // Methods
        private void Reset()
        {
            m_transitionBehaviours = new List<MonoBehaviour>();

            var transitions = transform.GetComponents<ITransition>();

            foreach (var transition in transitions)
            {
                if (transition is MonoBehaviour behaviour && behaviour != this)
                    m_transitionBehaviours.Add(behaviour);
            }
        }

        private void CheckSetup()
        {
            if (m_isTransitionInitialized)
                return;

            if (m_transitionBehaviours == null || m_transitionBehaviours.Count == 0)
            {
                Debug.LogError("Composite Transitions doesn't have any transition assigned", this);
                return;
            }

            m_transitions = new List<ITransition>();

            foreach (var behaviour in m_transitionBehaviours)
            {
                if (behaviour == null)
                {
                    Debug.LogWarning("Transition behaviour is null", this);
                    continue;
                }

                if (behaviour == this)
                {
                    Debug.LogWarning("Transition behaviour can't be this behaviour.", this);
                    continue;
                }

                if (behaviour is not ITransition transition)
                {
                    Debug.LogWarning("Behaviour doesn't implement the ITransition interface", behaviour);
                    continue;
                }

                m_transitions.Add(transition);
            }

            m_showDuration = TransitionsUtilities.GetShowMaxDuration(m_transitions);
            m_hideDuration = TransitionsUtilities.GetHideMaxDuration(m_transitions);

            m_isTransitionInitialized = true;
        }

        public void ShowInstantly()
        {
            CheckSetup();
            ForceStop();

            foreach (var transition in m_transitions)
            {
                transition.ShowInstantly();
            }
        }

        public void HideInstantly()
        {
            CheckSetup();
            ForceStop();

            foreach (var transition in m_transitions)
            {
                transition.HideInstantly();
            }
        }

        public void Show(bool useScaledTime)
        {
            if (State != TransitionState.Idle)
                ForceStop();

            CheckSetup();

            State = TransitionState.Showing;

            foreach (var transition in m_transitions)
            {
                transition.Show(useScaledTime);
            }

            StartCoroutine(TransitionToIdleWithDelay(m_showDuration, useScaledTime));
        }

        public void Hide(bool useScaledTime)
        {
            if (State != TransitionState.Idle)
                ForceStop();

            CheckSetup();

            State = TransitionState.Hiding;

            foreach (var transition in m_transitions)
            {
                transition.Hide(useScaledTime);
            }

            StartCoroutine(TransitionToIdleWithDelay(m_hideDuration, useScaledTime));
        }

        public void ForceStop()
        {
            State = TransitionState.Idle;

            foreach (var transition in m_transitions)
            {
                transition.ForceStop();
            }
        }

        // Coroutines
        private IEnumerator TransitionToIdleWithDelay(float seconds, bool useScaledTime)
        {
            if (useScaledTime)
                yield return new WaitForSeconds(seconds);

            else
                yield return new WaitForSecondsRealtime(seconds);

            State = TransitionState.Idle;
        }
    }
}
