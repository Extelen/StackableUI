using System;
using System.Collections;
using UnityEngine;

namespace Tellory.StackableUI.Extensions
{
    internal static class MonoBehaviourExtensions
    {
        public static Coroutine ExecuteOnSeconds(this MonoBehaviour monoBehaviour, float seconds, Action action, bool useScaledTime = true)
        {
            seconds = Mathf.Max(0, seconds);

            if (seconds == 0)
            {
                action?.Invoke();
                return null;
            }

            return monoBehaviour.StartCoroutine(ExecuteOnSecondsCoroutine(seconds, action, useScaledTime));
        }

        private static IEnumerator ExecuteOnSecondsCoroutine(float seconds, Action action, bool useScaledTime)
        {
            if (useScaledTime)
                yield return new WaitForSeconds(seconds);
            else
                yield return new WaitForSecondsRealtime(seconds);

            action?.Invoke();
        }
    }
}
