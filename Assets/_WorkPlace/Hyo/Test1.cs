using System;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    public MonsterData monster;

    public void Start()
    {
        Debug.Log(monster.ToStringForTMPro());
    }
}
