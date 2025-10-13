using UnityEngine;

namespace Tellory.StackableUI.Navigation
{
    public abstract class BackNavigationBase : MonoBehaviour
    {
        // Fields
        [SerializeField]
        private MonoBehaviour m_backHandlerBehaviour;

        private IViewBackHandler m_backHandler;
        public IViewBackHandler BackHandler
        {
            get => m_backHandler;
            set => m_backHandler = value;
        }

        // Methods
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            m_backHandler = m_backHandlerBehaviour as IViewBackHandler;

            if (m_backHandler == null && m_backHandlerBehaviour != null)
                Debug.LogWarning($"Assigned behaviour doesn't implement IBackHandler", this);
        }

        public virtual void AttemptNavigateBack()
        {
            if (m_backHandler == null)
                return;

            m_backHandler.NavigateBack();
        }
    }
}