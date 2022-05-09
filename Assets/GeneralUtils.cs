using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GeneralUtils
{
    public static IEnumerator CoroutineTask(Task task, Action onCompletedCallback = null)
    {
        yield return new WaitUntil(() => task.IsCompleted);
        onCompletedCallback?.Invoke();
    }

    public static IEnumerator CoroutineTask<T>(Task<T> task, Action<T> onCompletedCallback = null)
    {
        yield return new WaitUntil(() => task.IsCompleted);
        onCompletedCallback?.Invoke(task.Result);
    }

}

public static class GeneralUtilsExtensions
{
    public static IEnumerator ToCoroutine(this Task task, Action onCompletedCallback = null)
    {
        return GeneralUtils.CoroutineTask(task, onCompletedCallback);
    }

    public static IEnumerator ToCoroutine<T>(this Task<T> task, Action<T> onCompletedCallback = null)
    {
        return GeneralUtils.CoroutineTask<T>(task, onCompletedCallback);
    }
}
