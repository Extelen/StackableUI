using System.Collections.Generic;
using UnityEngine;

using Tellory.StackableUI.Navigation;
using Tellory.StackableUI.Views;

namespace Tellory.StackableUI.Manager
{
    /// <summary>
    /// Manager that handles nested view navigation.
    /// Searches recursively through child views and closes the deepest nested view.
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class NestedViewsManager : StackNesterBase, IViewBackHandler
    {
        // Methods
        /// <summary>
        /// Navigates back by finding the deepest nested view and closing it.
        /// Searches recursively through the view hierarchy.
        /// </summary>
        public virtual bool NavigateBack()
        {
            TrySetupViews();

            // Find the deepest nested view
            IViewNester deepestNester = FindDeepestNestedView(this, out int depth);

            if (deepestNester == null || depth == 0)
                return false;

            if (deepestNester.NestedView == DefaultNestedView as IViewNester)
                return false;

            if (deepestNester.NestedView is not IViewBackHandler backHandler)
            {
                Debug.LogWarning("Deepest nested view doesn't implement IViewBackHandler");
                return false;
            }

            // Close the deepest nested view
            return backHandler.NavigateBack();
        }

        /// <summary>
        /// Recursively finds the deepest nested view in the hierarchy.
        /// </summary>
        /// <param name="nester">The current nester to search from.</param>
        /// <param name="currentDepth">Output parameter with the depth of the deepest view found.</param>
        /// <returns>The deepest IViewNester with an active nested view, or null if none found.</returns>
        protected IViewNester FindDeepestNestedView(IViewNester nester, out int currentDepth)
        {
            if (nester == null || nester.NestedView == null)
            {
                currentDepth = 0;
                return null;
            }

            // Check if current nested view is also a nester
            if (nester.NestedView is IViewNester childNester)
            {
                // Recursively search deeper
                IViewNester deeperNester = FindDeepestNestedView(childNester, out int childDepth);

                if (deeperNester != null)
                {
                    currentDepth = childDepth + 1;
                    return deeperNester;
                }
            }

            // This is the deepest level with a nested view
            currentDepth = 1;
            return nester;
        }
    }
}
