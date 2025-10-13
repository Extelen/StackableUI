using UnityEngine;

using Tellory.StackableUI.Navigation;

namespace Tellory.StackableUI.Views
{
    public abstract class StackableViewBase : StackNesterBase, IView, IViewBackHandler
    {
        // Fields
        [SerializeField]
        private string m_identifier;
        public string Identifier
        {
            get => m_identifier;
            set => m_identifier = value;
        }

        [SerializeField]
        private bool m_isBackNavigationAllowed = true;
        public bool IsBackNavigationAllowed
        {
            get => m_isBackNavigationAllowed;
            set => m_isBackNavigationAllowed = value;
        }

        public IViewNester ParentView { get; set; }

        // Methods
        protected override void Reset()
        {
            base.Reset();
            m_identifier = name;
        }

        public bool NavigateBack()
        {
            if (!m_isBackNavigationAllowed)
                return false;

            CloseView();
            return true;
        }

        public virtual void CloseView()
        {
            OnBackPerformed();

            ParentView?.OnNestedViewClose();
            ParentView = null;
        }

        public abstract void ShowViewInstantly();
        public abstract void HideViewInstantly();

        public abstract void OnBackPerformed();

    }
}