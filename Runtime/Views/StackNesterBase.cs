using System.Collections.Generic;
using UnityEngine;

namespace Tellory.StackableUI.Views
{
    public abstract class StackNesterBase : MonoBehaviour, IViewNester
    {
        // Fields
        [SerializeField]
        private bool m_showInitialViewInstantly;

        [SerializeField]
        private MonoBehaviour m_initialView;
        public MonoBehaviour InitialView
        {
            get => m_initialView;
        }

        [SerializeField]
        private MonoBehaviour m_baseView;
        public MonoBehaviour BaseView
        {
            get => m_baseView;
        }

        [SerializeField]
        private bool m_useInitialViewAsBase = true;

        [SerializeField]
        private List<MonoBehaviour> m_nestedViewBehaviours;

        private List<IView> m_nestedViews;
        private bool m_hasNestedViewsBeenInitialized;

        // Properties
        public IView NestedView { get; protected set; }

        // Methods
        protected virtual void Awake()
        {
            TrySetupViews();
        }

        protected virtual void OnEnable()
        {
            CloseAllViewsInstantly();
            TryShowInitialView();
        }

        protected virtual void OnDisable()
        {
            CloseAllViewsInstantly();
        }

        protected virtual void Start() { }
        protected virtual void Reset() { }

        private void TryShowInitialView()
        {
            if (m_initialView == null)
                return;

            SwitchView(m_initialView, m_showInitialViewInstantly);
        }

        protected void TrySetupViews()
        {
            if (m_hasNestedViewsBeenInitialized)
                return;

            m_nestedViews = new List<IView>();

            foreach (var behaviour in m_nestedViewBehaviours)
            {
                if (behaviour == null)
                {
                    Debug.LogWarning("Behaviour is null", this);
                    continue;
                }

                if (behaviour == this)
                {
                    Debug.LogError("Behaviour can't be itself (Stack Overflow).", this);
                    continue;
                }

                if (behaviour is IView view)
                {
                    m_nestedViews.Add(view);
                }
            }

            m_hasNestedViewsBeenInitialized = true;
        }

        public void SwitchView(string identifier) => SwitchView(identifier, false);
        public void SwitchView(IView view) => SwitchView(view, false);
        public void SwitchView(IView view, bool performInstantly) => SwitchView(view.Identifier, performInstantly);
        public void SwitchView(MonoBehaviour behaviour) => SwitchView(behaviour, false);

        public void SwitchView(MonoBehaviour behaviour, bool performInstantly)
        {
            if (behaviour is IView view)
                SwitchView(view.Identifier, performInstantly);

            else
                Debug.LogWarning("Behaviour doesn't have IView", behaviour);
        }

        public GameObject SwitchView(string identifier, bool performInstantly)
        {
            IView selectedView = null;
            TrySetupViews();

            foreach (var item in m_nestedViewBehaviours)
            {
                IView view = (IView)item;

                if (view.Identifier != identifier)
                {
                    if (!performInstantly && view is IAnimatedView animatedView)
                        animatedView.HideView();

                    else
                        view.HideViewInstantly();

                    continue;
                }

                if (selectedView != null)
                {
                    Debug.LogWarning($"View with name: {view.Identifier} is not unique.", this);
                    continue;
                }

                selectedView = view;
            }

            if (selectedView != null)
            {
                if (!performInstantly && selectedView is IAnimatedView animatedView)
                    animatedView.ShowView();

                else
                    selectedView.ShowViewInstantly();
            }

            NestedView = selectedView;
            selectedView.ParentView = this;

            return (selectedView as MonoBehaviour).gameObject;
        }

        public virtual void OnNestedViewClose()
        {
            MonoBehaviour desiredView = m_useInitialViewAsBase ? m_initialView : m_baseView;

            if (desiredView && (desiredView as IView) != NestedView)
            {
                SwitchView(desiredView);
                return;
            }

            NestedView = null;
        }

        public void CloseAllViewsInstantly()
        {
            TrySetupViews();

            foreach (var view in m_nestedViews)
            {
                view.HideViewInstantly();
            }

            NestedView = null;
        }
    }
}