using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeFrameClasses;

public class TimeFrameAffectedObject : MonoBehaviour
{

    private void OnEnable()
    {
        TimeFrameObjectManager.InfoTime += SendObjectInformation_To_TimeFrameManager;
    }

    private void OnDisable()
    {
        TimeFrameObjectManager.InfoTime -= SendObjectInformation_To_TimeFrameManager;        
    }


    private void SendObjectInformation_To_TimeFrameManager()
    {
        ObjectInformation objectInfo = new ObjectInformation(transform);
        TimeFrameManager.AddToTimeFrame(objectInfo);
    }
}
