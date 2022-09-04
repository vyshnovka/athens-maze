using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    //useful timed event
    public static IEnumerator TimedEvent(Action target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.Invoke();
    }
}
