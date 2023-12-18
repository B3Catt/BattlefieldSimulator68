using UnityEngine;
using System.Collections;
using System;

public class Timer
{
    //Class used to set a timer parallel to the main code and call functins

    public static IEnumerator Start(float duration, Action callback)
    {
        return Start(duration, false, callback);
    }

    public static IEnumerator Start(float duration, bool repeat, Action callback)
    {
        do
        {
            yield return new WaitForSeconds(duration);

            if (callback != null)
                callback();

        } while (repeat);
    }

    public static IEnumerator StartRealtime(float time, System.Action callback)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }

        if (callback != null) callback();
    }

    public static IEnumerator NextFrame(Action callback)
    {
        yield return new WaitForEndOfFrame();

        if (callback != null)
            callback();
    }
}
