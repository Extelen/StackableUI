using UnityEngine;

namespace Tellory.StackableUI.Transitions
{
    [System.Serializable]
    public class AnimationTransitionCurve
    {
        // Fields
        [SerializeField]
        private float m_duration = 0.25f;
        public float Duration => m_duration;

        [SerializeField]
        private AnimationCurve m_animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve AnimationCurve => m_animationCurve;

        // Methods
        public float Evaluate(float time)
        {
            return m_animationCurve.Evaluate(time / m_duration);
        }
    }
}