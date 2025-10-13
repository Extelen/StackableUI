using System.Collections.Generic;
using UnityEngine;

using Tellory.StackableUI.Transitions;

internal static class TransitionsUtilities
{
    // Methods
    public static float GetShowMaxDuration(List<ITransition> transitions)
    {
        float maxDuration = 0f;

        if (transitions == null)
            return maxDuration;

        foreach (var transition in transitions)
        {
            if (transition == null)
                continue;

            if (transition.ShowDuration <= maxDuration)
                continue;

            maxDuration = transition.ShowDuration;
        }

        return maxDuration;
    }

    public static float GetHideMaxDuration(List<ITransition> transitions)
    {
        float maxDuration = 0f;

        if (transitions == null)
            return maxDuration;

        foreach (var transition in transitions)
        {
            if (transition == null)
                continue;

            if (transition.HideDuration <= maxDuration)
                continue;

            maxDuration = transition.HideDuration;
        }

        return maxDuration;
    }
}
