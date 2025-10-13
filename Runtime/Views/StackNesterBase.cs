using System.Collections.Generic;
using UnityEngine;

namespace Tellory.StackableUI.Views
{
    public abstract class StackNesterBase : MonoBehaviour, IViewNester
    {
        // Fields
        [SerializeField]
        private bool m_showDefaultViewInstantly;

        [SerializeField]
        private MonoBehaviour m_defaultNestedView;
        public MonoBehaviour DefaultNestedView
        {
            get => m_defaultNestedView;
        }

        [SerializeField]
        private bool m_openDefaultViewAtNestedClose = true;

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
            TryShowDefaultView();
        }

        protected virtual void OnDisable()
        {
            CloseAllViewsInstantly();
        }

        protected virtual void Start() { }
        protected virtual void Reset() { }

        private void TryShowDefaultView()
        {
            if (m_defaultNestedView == null)
                return;

            SwitchView(m_defaultNestedView, m_showDefaultViewInstantly);
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

        public void SwitchView(string identifier)
        {
            SwitchView(identifier, false);
        }

        public void SwitchView(MonoBehaviour behaviour)
        {
            SwitchView(behaviour, false);
        }

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
            if (m_openDefaultViewAtNestedClose && m_defaultNestedView)
            {
                if ((m_defaultNestedView as IView) != NestedView)
                {
                    SwitchView(m_defaultNestedView);
                    return;
                }
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