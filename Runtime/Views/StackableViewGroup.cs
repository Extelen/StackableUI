using System.Collections.Generic;
using UnityEngine;

using Tellory.StackableUI.Extensions;

namespace Tellory.StackableUI.Views
{
    public class StackableViewGroup : StackableViewBase, IAnimatedView
    {
        // Fields
        [SerializeField]
        private MonoBehaviour[] m_viewBehaviours;
        private List<IView> m_views;

        private bool m_hasViewsBeenInitialized;

        // Properties
        private float m_showAnimationDuration;
        public float ShowAnimationDuration
        {
            get
            {
                TrySetupSubViews();
                return m_showAnimationDuration;
            }
        }

        private float m_hideAnimationDuration;
        public float HideAnimationDuration
        {
            get
            {
                TrySetupSubViews();
                return m_hideAnimationDuration;
            }
        }

        private bool m_isUsingScaledTime;
        public bool IsUsingScaledTime
        {
            get
            {
                TrySetupSubViews();
                return m_isUsingScaledTime;
            }
        }

        private Coroutine m_disableOnTransitionEnd;

        // Methods
        protected override void Awake()
        {
            TrySetupSubViews();
            base.Awake();
        }

        private void TrySetupSubViews()
        {
            if (m_hasViewsBeenInitialized)
                return;

            m_views = new List<IView>();

            float maxShowAnimationDuration = 0;
            float maxHideAnimationDuration = 0;

            foreach (var behaviour in m_viewBehaviours)
            {
                if (behaviour == this)
                {
                    Debug.LogError("Behaviour can't be itself (Stack Overflow).", this);
                    continue;
                }

                if (behaviour is IView view)
                    m_views.Add(view);

                else
                {
                    Debug.LogError("Behaviour doesn't have the IView interface", behaviour);
                    continue;
                }

                if (behaviour is IAnimatedView animatedView)
                {
                    if (animatedView.ShowAnimationDuration > maxShowAnimationDuration)
                        maxShowAnimationDuration = animatedView.ShowAnimationDuration;

                    if (animatedView.HideAnimationDuration > maxHideAnimationDuration)
                        maxHideAnimationDuration = animatedView.HideAnimationDuration;

                    if (animatedView.IsUsingScaledTime)
                        m_isUsingScaledTime = true;
                }
            }

            m_showAnimationDuration = maxShowAnimationDuration;
            m_hideAnimationDuration = maxHideAnimationDuration;

            m_hasViewsBeenInitialized = true;
        }

        public override void ShowViewInstantly()
        {
            gameObject.SetActive(true);
            TrySetupSubViews();

            m_views.ForEach(c => c.ShowViewInstantly());

            CancelDeactivation();
        }

        public override void HideViewInstantly()
        {
            gameObject.SetActive(false);
            TrySetupSubViews();

            m_views.ForEach(c => c.HideViewInstantly());
        }

        public virtual void ShowView()
        {
            gameObject.SetActive(true);
            TrySetupSubViews();

            foreach (var view in m_views)
            {
                if (view is IAnimatedView animatedView)
                    animatedView.ShowView();

                else
                    view.ShowViewInstantly();
            }

            CancelDeactivation();
        }

        public virtual void HideView()
        {
            if (!gameObject.activeSelf)
            {
                HideViewInstantly();
                return;
            }

            TrySetupSubViews();

            bool useScaledTime = false;

            foreach (var view in m_views)
            {
                if (view is IAnimatedView animatedView)
                {
                    animatedView.HideView();

                    if (useScaledTime == false && animatedView.IsUsingScaledTime)
                        useScaledTime = true;
                }

                else
                    view.HideViewInstantly();
            }

            m_disableOnTransitionEnd = this.ExecuteOnSeconds(HideAnimationDuration, () => gameObject.SetActive(false), useScaledTime);
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