using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CameraPose
{
    public Vector3 position;
    public Vector3 rotation;
    public float transitionTime;

    public CameraPose(Vector3 position, Vector3 rotation, float transitionTime)
    {
        this.position = position;
        this.rotation = rotation;
        this.transitionTime = transitionTime;
    }
}

[System.Serializable]
public class CameraPoseList
{
    public CameraTransitionType transitionType;
    public List<CameraPose> poseList;
}