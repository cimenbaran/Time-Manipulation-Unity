using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeFrameClasses;

public static class TimeFrameObjectManager 
{
    public delegate void InfoTimeDelegate();
    public static InfoTimeDelegate InfoTime;

    public delegate void InfoRewindEndedDelegate();
    public static InfoRewindEndedDelegate RewindEnded;
}
