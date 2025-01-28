using System;
using UnityEngine;

[Serializable]
public class Skills
{
    public SkillType skillType;
    public string skillName;
    public float damage;
    public float cooldown;
    public float energyCost;
    public GameObject effectPrefab;
    public string activeTriggerName;
    public float particleDelay;

    public BasicTimer cooldownTimer;

    public void Initialize()
    {
        cooldownTimer = new BasicTimer(cooldown);
    }
}

public enum SkillType
{
    Physical,
    Magic,
    Support
}