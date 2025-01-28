using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraPoseSetting", menuName = "Ds Project/CameraPose")]
public class CameraPoseSetting : ScriptableObject
{
    public List<CameraPose> poseList;
    public List<CameraPoseList> poseTransitionList;
}