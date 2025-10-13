using System.Collections.Generic;
using UnityEngine;

using Tellory.StackableUI.Extensions;
using Tellory.StackableUI.Transitions;

namespace Tellory.StackableUI.Views
{
    public class StackableView : StackableViewBase, IAnimatedView
    {
        // Fields
        [SerializeField]
        private MonoBehaviour m_transitionBehaviour;
        private ITransition m_transition;

        [SerializeField]
        private bool m_useScaledTime;
        public bool IsUsingScaledTime
        {
            get => m_useScaledTime;
        }

        private bool m_hasTransitionBeenInitialized;

        // Properties
        private float m_showAnimationDuration = -1;
        public float ShowAnimationDuration
        {
            get
            {
                CheckTransitionSetup();
                return m_showAnimationDuration;
            }
        }

        private float m_hideAnimationDuration = -1;
        public float HideAnimationDuration
        {
            get
            {
                CheckTransitionSetup();
                return m_hideAnimationDuration;
            }
        }

        private Coroutine m_disableOnTransitionEnd;

        // Methods
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            CheckTransitionSetup();
            base.Awake();
        }

        private void CheckTransitionSetup()
        {
            if (m_hasTransitionBeenInitialized)
                return;

            if (m_transitionBehaviour is not ITransition transition)
            {
                Debug.LogError("Transition behaviour doesn't implement ITransition");
                return;
            }

            m_transition = transition;

            m_showAnimationDuration = m_transition.ShowDuration;
            m_hideAnimationDuration = m_transition.HideDuration;

            m_hasTransitionBeenInitialized = true;
        }

        public override void ShowViewInstantly()
        {
            gameObject.SetActive(true);
            CheckTransitionSetup();

            m_transition.ShowInstantly();

            CancelDeactivation();
        }

        public override void HideViewInstantly()
        {
            gameObject.SetActive(false);
            CheckTransitionSetup();

            m_transition?.HideInstantly();
        }

        public virtual void ShowView()
        {
            gameObject.SetActive(true);
            CheckTransitionSetup();

            m_transition?.Show(m_useScaledTime);
            CancelDeactivation();
        }

        public virtual void HideView()
        {
            if (!gameObject.activeSelf)
            {
                HideViewInstantly();
                return;
            }

            CheckTransitionSetup();
            m_transition.Hide(m_useScaledTime);

            m_disableOnTransitionEnd = this.ExecuteOnSeconds(HideAnimationDuration, () => gameObject.SetActive(false), m_useScaledTime);
        }

        private void CancelDeactivation()
        {
            if (m_disableOnTransitionEnd == null)
                return;

            StopCoroutine(m_disableOnTransitionEnd);
            m_disableOnTransitionEnd = null;
        }

        public override void OnBackPerformed()
        {
            HideView();
        }

        public override void OnNestedViewClose()
        {
            base.OnNestedViewClose();

            if (NestedView == null)
                NavigateBack();
        }
    }
}