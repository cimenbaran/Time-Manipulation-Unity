using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeFrameClasses;

public enum TimeManager_SaveType
{
    fixedTime,
    fixedFrames
}


public class TimeManager : MonoBehaviour
{
    public bool ActivateRewind = false;

    public float milisecondsPerSave = 0.1f;

    public TimeManager_SaveType timeManagerSaveType;

    public float maximumSavesFixedTime = 5f;

    public int maximumSavesFixedFrames = 300;

    public int cooldownFrames = 300;

    public List<TimeFrame> timeFrames;


    // First Recorded TimeFrame Index at the List
    // Implemented to make the TimeFrame addition process to O(1)
    // Since removing index 0 from a List<> is O(n)
    private int firstIndex = 0;

    // Counts the iteration of rewinds
    private int rewindTimer = 0;

    // Waits until timeFrame is filled again
    private int cooldownTimer;

    public void RequestObjectInformation()
    {
        // TimeFrameManager - Create A TimeFrame
        TimeFrameManager.CreateATimeFrame();

        // TimeFrameObjectManager - Invoke InfoTime
        TimeFrameObjectManager.InfoTime.Invoke();

        // TimeFrameManager - Get The TimeFrame
        TimeFrame currentTimeFrame = TimeFrameManager.GetTimeFrame();

        if(timeFrames.Count == maximumSavesFixedFrames)
        {
            // If List is full
            timeFrames[firstIndex] = currentTimeFrame;
            firstIndex++;
            if(firstIndex == timeFrames.Count)
            {
                firstIndex = 0;
            }

        }
        else
        {
            // If List still has space
            timeFrames.Add(currentTimeFrame);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
    }

    private void OnEnable()
    {
        if (timeFrames is null)
        {
            timeFrames = new List<TimeFrame>();
        }

        cooldownTimer = cooldownFrames;
    }

    // Update is called once per frame
    void Update()
    {
        if (ActivateRewind && cooldownTimer <= 0 && rewindTimer < timeFrames.Count)
        {
            rewindTimer++;
            int lastIndex = firstIndex - rewindTimer;
            if (lastIndex < 0)
            {
                lastIndex += timeFrames.Count;
            }
            TimeFrame currentTimeFrame = timeFrames[lastIndex];
            currentTimeFrame.Rewind();


            // Used all TimeFrames;
            if (rewindTimer == timeFrames.Count)
            {
                cooldownTimer = maximumSavesFixedFrames;
                
                // Reset the rewindTimer;
                rewindTimer = 0;

                // Clean the List
                timeFrames.Clear();

            }
        }
        else
        {
            cooldownTimer = cooldownTimer <= 0 ? 0 : cooldownTimer - 1;
            RequestObjectInformation();
        }
        Debug.Log(1f / Time.deltaTime);
    }
}
