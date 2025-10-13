using UnityEngine;

namespace Tellory.StackableUI.Transitions
{
    [DefaultExecutionOrder(-1)]
    public class PositionTransition : TransitionBase
    {
        // Enums
        private enum Direction
        {
            TopToBottom,
            BottomToTop,
            RightToLeft,
            LeftToRight
        }

        // Fields
        [SerializeField]
        private RectTransform m_rectTransform;

        [SerializeField]
        private Direction m_transitionDirection = Direction.BottomToTop;

        [SerializeField]
        private float m_movementStrength = 32;

        private bool m_initialized;

        private Vector2 m_targetPosition;
        private Vector2 m_hiddenPosition;

        // Methods
        protected override void Awake()
        {
            base.Awake();
            CheckInitialization();
        }

        private void CheckInitialization()
        {
            if (m_initialized)
                return;

            m_targetPosition = m_rectTransform.anchoredPosition;
            m_hiddenPosition = GetHiddenPosition();

            m_initialized = true;
        }

        private void Reset()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        protected override void ApplyShowState()
        {
            CheckInitialization();
            m_rectTransform.anchoredPosition = m_targetPosition;
        }

        protected override void ApplyHideState()
        {
            CheckInitialization();
            m_rectTransform.anchoredPosition = m_hiddenPosition;
        }

        protected override void ApplyTransitionValue(float normalizedValue)
        {
            m_rectTransform.anchoredPosition = Vector2.Lerp(m_hiddenPosition, m_targetPosition, normalizedValue);
        }

        private Vector2 GetHiddenPosition()
        {
            Vector2 direction = m_transitionDirection switch
            {
                Direction.TopToBottom => Vector2.up,
                Direction.BottomToTop => Vector2.down,
                Direction.RightToLeft => Vector2.right,
                Direction.LeftToRight => Vector2.left,
                _ => Vector2.zero,
            };

            return m_targetPosition + (direction * m_movementStrength);
        }
    }
}