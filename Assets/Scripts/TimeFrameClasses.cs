using System.Collections.Generic;
using UnityEngine;

namespace TimeFrameClasses
{
    public struct TransformValues
    {
        public Vector3 pos;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformValues(Transform transform)
        {
            pos = transform.position;
            rotation = transform.rotation;
            scale = transform.localScale;
        }
    }

    public class ObjectInformation
    {
        public TimeFrameAffectedObject obj;
        public Rigidbody objectRigidbody;
        public Transform objectTransform;
        public TransformValues transformValues;

        public ObjectInformation(Transform transform, Rigidbody rigidbody, TimeFrameAffectedObject timeFrameAffectedObject)
        {
            objectTransform = transform;
            objectRigidbody = rigidbody;
            obj = timeFrameAffectedObject;
            transformValues = new TransformValues(transform);
        }

        public void RewindObject()
        {
            if(objectTransform is not null)
            {
                objectRigidbody.isKinematic = true;
                if (obj.isInitialVelocityProtected || obj.isNewVelocityProtected)
                {
                    obj.lastTransformValues = new TransformValues(objectTransform);
                }
                objectTransform.position = transformValues.pos;
                objectTransform.rotation = transformValues.rotation;
                objectTransform.localScale = transformValues.scale;
            }
        }
    }

    public class TimeFrame
    {
        public List<ObjectInformation> objectInformationList;

        public TimeFrame()
        {
            objectInformationList = new List<ObjectInformation>();
        }

        public void Add(ObjectInformation objectInfo)
        {
            objectInformationList.Add(objectInfo);
        }

        public void Rewind()
        {
            foreach(ObjectInformation objectInfo in objectInformationList)
            {
                objectInfo.RewindObject();
            }
        }
    }
}