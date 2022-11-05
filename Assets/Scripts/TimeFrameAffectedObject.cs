using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeFrameClasses;

public class TimeFrameAffectedObject : MonoBehaviour
{
    // Objects can protect their velocity at the end of rewind
    // Velocity determined depending on their last frame and this frame's position
    public bool isNewVelocityProtected = false;

    public bool isInitialVelocityProtected = false;

    [HideInInspector]
    public TransformValues lastTransformValues;

    private Rigidbody objectRigidbody;

    private void Awake()
    {
        objectRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (isNewVelocityProtected)
        {
            objectRigidbody.AddForce(new Vector3(-500, 0, 0));
        }
    }
    private void OnEnable()
    {
        TimeFrameObjectManager.InfoTime += SendObjectInformation_To_TimeFrameManager;
        TimeFrameObjectManager.RewindEnded += EndOfRewind_Actions;
    }

    private void OnDisable()
    {
        TimeFrameObjectManager.InfoTime -= SendObjectInformation_To_TimeFrameManager;     
        TimeFrameObjectManager.RewindEnded -= EndOfRewind_Actions;
    }


    private void SendObjectInformation_To_TimeFrameManager()
    {
        ObjectInformation objectInfo = new ObjectInformation(transform, objectRigidbody, this);
        TimeFrameManager.AddToTimeFrame(objectInfo);
    }

    private void EndOfRewind_Actions()
    {
        objectRigidbody.isKinematic = false;
        if(isInitialVelocityProtected || isNewVelocityProtected)
        {
            // Position change
            Vector3 posChange = lastTransformValues.pos - transform.position;

            // Calculate last Velocity
            Vector3 lastVelocity = posChange / Time.fixedDeltaTime;

            // Calculate required acceleration to maintain current velocity
            Vector3 acceleration = lastVelocity / Time.fixedDeltaTime;
            //Debug.Log(lastTransformValues.pos);
            //Debug.Log(posChange);
            //Debug.Log(lastVelocity);
            //
            //Debug.Log(acceleration);
            // Apply force
            if(isNewVelocityProtected)
            {
                acceleration *= -1;
            }
            objectRigidbody.AddForce(acceleration);


            // Rotation Change
            Quaternion rotationChange = lastTransformValues.rotation* Quaternion.Inverse(transform.rotation);
            
            if(isNewVelocityProtected)
            {
                rotationChange = Quaternion.Inverse(rotationChange);
            }


            // Check if there is any rotation
            if (rotationChange != Quaternion.identity) 
            {
                // Calculate 
                Vector3 torque = rotationChange.eulerAngles / Time.fixedDeltaTime;

                // Apply Torque
                objectRigidbody.AddTorque(torque);
            }
        }

        if(isNewVelocityProtected)
        {
            // Position change
            Vector3 posChange = transform.position - lastTransformValues.pos;

            // Calculate last Velocity
            Vector3 lastVelocity = posChange / Time.fixedDeltaTime;

            // Calculate required acceleration to maintain current velocity
            Vector3 acceleration = lastVelocity / Time.fixedDeltaTime;
            //Debug.Log(lastTransformValues.pos);
            //Debug.Log(posChange);
            //Debug.Log(lastVelocity);
            //
            //Debug.Log(acceleration);
            // Apply force
            objectRigidbody.AddForce(acceleration);


            // Rotation Change
            Quaternion rotationChange = transform.rotation * Quaternion.Inverse(lastTransformValues.rotation);
            //Debug.Log(rotationChange);
            //Debug.Log(Quaternion.identity);
            // Check if there is any rotation
            if (rotationChange != Quaternion.identity)
            {
                // Calculate 
                Vector3 torque = rotationChange.eulerAngles / Time.fixedDeltaTime;

                // Apply Torque
                objectRigidbody.AddTorque(torque);
            }
        }
    }
}
