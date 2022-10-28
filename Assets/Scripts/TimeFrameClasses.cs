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
        public Transform objectTransform;
        public TransformValues transformValues;

        public ObjectInformation(Transform transform)
        {
            objectTransform = transform;
            transformValues = new TransformValues(transform);
        }

        public void RewindObject()
        {
            if(objectTransform is not null)
            {
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