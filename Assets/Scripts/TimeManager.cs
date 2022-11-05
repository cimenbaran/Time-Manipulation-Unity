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

    public float cooldownFixedTime = 5f;


    private int maximumSavesFixedFrames;

    private int cooldownFrames;

    public List<TimeFrame> timeFrames;

    private float fixedUpdateFrames;



    // Track of rewind ending
    private bool wasLastFrameAtRewind = false;

    // First Recorded TimeFrame Index at the List
    // Implemented to make the TimeFrame addition process to O(1)
    // Since removing index 0 from a List<> is O(n)
    private int firstIndex = 0;

    // Counts the iteration of rewinds
    private int rewindTimer = 0;

    // Waits until timeFrame is filled again
    private int cooldownTimer;

    // How many frames we have recorded so far
    // timeFrames.Count is not reliable since we implemented a system to reduce O(n) to O(1)
    // So when the list is full, it won't give us how many frames we currently hold
    private int frameCount = 0;

    private void RequestObjectInformation()
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

        if(frameCount < maximumSavesFixedFrames)
        {
            frameCount++;
        }

    }

    private void AtRewindEnded()
    {
        TimeFrameObjectManager.RewindEnded.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;

        float radius = 10f;
        Vector3 explosionPos =  new Vector3(3f, 6f, 3f);
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
        
            if (rb != null)
                rb.AddExplosionForce(500f, explosionPos, radius, 3.0F);
        }
    }

    private void Awake()
    {
        fixedUpdateFrames = 1f / Time.fixedDeltaTime;
        maximumSavesFixedFrames = (int)(maximumSavesFixedTime * fixedUpdateFrames);
        cooldownFrames = (int)(cooldownFixedTime * fixedUpdateFrames);
    }

    private void OnEnable()
    {
        if (timeFrames is null)
        {
            timeFrames = new List<TimeFrame>();
        }

        cooldownTimer = cooldownFrames;
    }

    void FixedUpdate()
    {
        if (ActivateRewind && cooldownTimer <= 0 && rewindTimer < frameCount)
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
            if (rewindTimer == frameCount) 
            {
                // Reset Cooldown
                cooldownTimer = cooldownFrames;
            }
            wasLastFrameAtRewind = true;
        }
        else
        {
            if (wasLastFrameAtRewind)
            {
                frameCount -= rewindTimer;
                firstIndex -= rewindTimer;
                if(firstIndex < 0)
                {
                    firstIndex += maximumSavesFixedFrames;
                }
                rewindTimer = 0;
                wasLastFrameAtRewind = false;
                AtRewindEnded();
            }
            cooldownTimer = cooldownTimer <= 0 ? 0 : cooldownTimer - 1;
            RequestObjectInformation();
        }
    }


    // Update is called once per frame
    void Update()
    {
        maximumSavesFixedFrames = (int)(maximumSavesFixedTime * fixedUpdateFrames);
        cooldownFrames = (int)(cooldownFixedTime * fixedUpdateFrames);
        //Debug.Log(1f / Time.deltaTime);
        //Debug.Log(maximumSavesFixedFrames);
    }
}
