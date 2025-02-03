using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SequenceData", menuName = "Ds Project/SequenceData")]
public class SequenceData : ScriptableObject
{
    [Header ("스토리 설정")]
    public int sequenceIndex;
    public int sequenceCount;

    [Header("스토리 타겟설정")]
    public string targetObject;

    [Header("기본 카메라 모드설정")]
    public CameraType cameraType;

    [Header("대화설정")]
    public List<DialogData> sequencesDialog;

    [Header("스토리 완료 여부")]
    public bool isComplete;

    [Header("타입설정")]
    public CharacterActionType characterAction;
    public SequenceTriggerType triggerType;

    [Header("스토리 진행순서 설정")]
    public List<StoryEventType> events;

    [Header("카메라 이동효과 설정")]
    public CameraPoseList cameraPoseList;
    
    [Header("카메라 효과 설정")]
    public CameraEffectData cameraEffect;

    [Header("이동효과 설정")]
    public CharacterMoveData moveData;

    [Header("사운드효과 설정")]
    public SoundData soundData;

    [Header("화면효과 설정")]
    public ScreenEffectData screenEffect;
}