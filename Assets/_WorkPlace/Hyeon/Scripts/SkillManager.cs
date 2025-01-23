using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : BaseManager<SkillManager>
{
    [SerializeField] private SkillList skillDatabase;

    private Dictionary<string, Skills> skillDictionary = new Dictionary<string, Skills>();
    [SerializeField] private List<Skills> currentUsedSkills = new List<Skills>();

    private Animator animator;
    [SerializeField] private GameObject skillImage;
    [SerializeField] private Transform skillPanel;
    public bool isActivating;
    private int currentMp;

    protected override void OnEnable()
    {
        base.OnEnable();

    }
    protected override void Awake()
    {
        base.Awake();
        InitializeSkillDictionary();

    }

    protected override void Start()
    {
        base.Start();
        animator = GameManager.playerTransform.GetComponent<PlayerController>().PlayerAnimator;
        UpdateCurrentMana();
    }

    private void Update()
    {
        CheckSkillCoolTime();
    }

    private void InitializeSkillDictionary()
    {
        foreach(var skill in skillDatabase.skillList)
        {
            if (!skillDictionary.ContainsKey(skill.skillName))
            {
                skillDictionary.Add(skill.skillName, skill);
                skill.Initialize();
                Debug.Log($"skillDatabase: {skillDictionary.ContainsKey(skill.skillName)}");
            }
            else
            {
                Debug.Log($"중복된 스킬 이름 : {skill.skillName}이(가) 데이터베이스에 이미 존재합니다.");
            }
        }
        
    }

    public Skills GetSkill(string skillName)
    {
        if(skillDictionary.TryGetValue(skillName, out var skill))
        {
            return skill;
        }

        return null;
    }

    public void ActivateSkill(string skillName, GameObject target = null)
    {
        if(target == null)
        {
            target = GameManager.playerTransform.gameObject;
        }
        Skills skill = GetSkill(skillName);
        isActivating = activeSkill.ContainsKey(skill);
        UpdateCurrentMana();
        if (isActivating)
        {
            Debug.Log("isActivating true");
            return;
        }
        else
        {
            CharacterManager.PlayerCharacterData.UseMp(skill.energyCost);
            UpdateCurrentMana();
            TimerManager.Instance.StartTimer(skill.cooldownTimer);
            animator.SetTrigger(skill.activeTriggerName);
            if (skill.effectPrefab != null)
            {
                Quaternion playerRotation = Quaternion.LookRotation(GameManager.playerTransform.forward);
                var effect = Instantiate(skill.effectPrefab, target.transform.position, playerRotation);
                if (skill.particleDelay > 0)
                {
                    var particleEffect = effect.GetComponent<ParticleSystem>().main;
                    particleEffect.startDelay = skill.particleDelay;
                }

                Destroy(effect, 5f);

            }
            //Debug.Log($"ActivateSkill: {skill.skillName}");
            currentUsedSkills.Add(skill);
        }
    }

    private Dictionary<Skills, Image> activeSkill = new Dictionary<Skills, Image>();
    public void CheckSkillCoolTime()
    {
        if (currentUsedSkills.Count > 0)
        {
            for(int i = currentUsedSkills.Count - 1; i>=0; i--)
            {
                var skill = currentUsedSkills[i];
                if (skill.cooldownTimer.RemainingPercent <= 0)
                {
                    if (activeSkill.ContainsKey(skill))
                    {
                        Destroy(activeSkill[skill].gameObject);
                        activeSkill.Remove(skill);
                        currentUsedSkills.Remove(skill);
                    }

                }
                else
                {
                    Image skillCoolImage = null;
                    TextMeshProUGUI skillCoolName = null;
                    if (!activeSkill.ContainsKey(skill))
                    {
                        var _skillImage = Instantiate(skillImage, skillPanel);
                        skillCoolImage = _skillImage.GetComponent<Image>();
                        skillCoolName = _skillImage.GetComponentInChildren<TextMeshProUGUI>();
                        activeSkill.Add(skill, skillCoolImage);
                    }
                    else
                    {
                        skillCoolImage = activeSkill[skill];
                        skillCoolName = activeSkill[skill].GetComponentInChildren<TextMeshProUGUI>();
                    }

                    skillCoolImage.fillAmount = skill.cooldownTimer.RemainingPercent;
                    skillCoolName.text = skill.skillName;
                }
            }
        }
    }

    public bool CheckMana(string skillName)
    {
        Skills skill = GetSkill(skillName);
        if (currentMp < skill.energyCost)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void UpdateCurrentMana()
    {
        currentMp = CharacterManager.PlayerCharacterData.currentMp;
    }

    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {

    }
}
