using UnityEngine;

namespace Tellory.StackableUI.Navigation
{
    public class InputManagerBackNavigation : BackNavigationBase
    {
        // Fields
        [SerializeField]
        private KeyCode m_keyCode;

        // Methods
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(m_keyCode))
                AttemptNavigateBack();
        }
    }
}