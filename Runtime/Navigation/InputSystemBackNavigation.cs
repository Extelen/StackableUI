using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace Tellory.StackableUI.Navigation
{
    public class InputSystemBackNavigation : BackNavigationBase
    {
        // Fields
        [SerializeField]
        private InputActionReference m_backActionReference;

        // Methods
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            m_backActionReference.action.canceled += OnBackActionRelease;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            m_backActionReference.action.canceled -= OnBackActionRelease;
        }

        private void OnBackActionRelease(InputAction.CallbackContext context)
        {
            AttemptNavigateBack();
        }
    }
}
#endif