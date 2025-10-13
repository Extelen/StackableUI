using UnityEngine;

namespace Tellory.StackableUI.Transitions
{
    public class OpacityTransition : TransitionBase
    {
        // Fields
        [SerializeField]
        private CanvasGroup m_canvasGroup;

        [SerializeField]
        private bool m_controlInteractivity = true;

        // Methods
        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        private void Reset()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();

            if (m_canvasGroup == null)
                m_canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        protected override void ApplyShowState()
        {
            m_canvasGroup.alpha = 1f;

            if (m_controlInteractivity)
                EnableInteraction();
        }

        protected override void ApplyHideState()
        {
            m_canvasGroup.alpha = 0f;

            if (m_controlInteractivity)
                DisableInteraction();
        }

        protected override void ApplyTransitionValue(float normalizedValue)
        {
            m_canvasGroup.alpha = normalizedValue;
        }

        protected override void OnHideStarted()
        {
            base.OnHideStarted();

            if (m_controlInteractivity)
                DisableInteraction();
        }

        protected virtual void EnableInteraction()
        {
            m_canvasGroup.interactable = true;
            m_canvasGroup.blocksRaycasts = true;
        }

        protected virtual void DisableInteraction()
        {
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
        }
    }
}
