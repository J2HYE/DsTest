using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SequenceCollection", menuName = "Ds Project/StorySequenceCollection")]
public class StorySequenceCollection : ScriptableObject
{
    public List<SequenceData> sequences; // 여러 개의 스토리 시퀀스 관리
}
