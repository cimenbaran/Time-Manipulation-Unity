using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeFrameClasses;

public static class TimeFrameManager 
{
    public static TimeFrame timeFrame;

    public static void CreateATimeFrame()
    {
        timeFrame = new TimeFrame();
    }

    public static TimeFrame GetTimeFrame()
    {
        return timeFrame;
    }

    public static void AddToTimeFrame(ObjectInformation objectInfo)
    {
        timeFrame.Add(objectInfo);
    }

}
