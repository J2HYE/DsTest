using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SpawnData
{
    public string spwanName;                // 스폰 데이터 이름
    public SpawnnerType spwanType;          // 스폰 타입
    public SpawnStyle spawnStyle;           // 스폰 스타일
    public Vector2 spaawnSize;              // 스폰 범위
    public Vector3 spawnPosition;        // 스폰 위치
    public List<string> spawnObjects;   // 스폰 대상 프리팹 리스트
    public bool autoSpawn = true;           // 자동 스폰 여부
    public float initialDelay = 60f;         // 재 스폰 딜레이
    public float spawnInterval = 0.05f;        // 스폰 주기
    public int maxSpawnCount = 10;           // 최대 스폰 개수
    public float detectionDistance = 100;    // 감지거리
    public TriggerType triggerType;         // 스폰 트리거조건
    public Func<bool> triggerCondition;     // 스폰 조건 (함수)
    public int activeObjectCount;
}
