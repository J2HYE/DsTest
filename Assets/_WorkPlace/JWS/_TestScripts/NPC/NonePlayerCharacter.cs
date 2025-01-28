using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NonePlayerCharacter : MonoBehaviour
{
    [SerializeField] private NPCType npcType;
    [SerializeField] public bool isMainQuest = false;
    [SerializeField] private int mainIndex = 0;
    [SerializeField] private NPCData currentNPCData = null;

    private CapsuleCollider capsuleCollider;
    private InterActText interActText = null;
    private Transform targetlook;
    private float interactionRadius = 4f;
    private float turnSpeed = 5f;
    private bool isPlayerInRange = false;
    private bool isInitNPC = false;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        interActText = GetComponentInChildren<InterActText>(true);
        interActText.gameObject.SetActive(false);
        if (capsuleCollider != null)
        {
            capsuleCollider.radius = interactionRadius;
        }
    }

    private void Update()
    {
        if (!isInitNPC)
        {
            if (currentNPCData.currentNPC != this.gameObject && QuestManager.NpcDatabase.mainQuestNpcLists.Count > 0)
            {
                GetMainNpcData();
            }
        }
        if (isPlayerInRange)
        {
            if (UIManager.Instance.IsUIWindowOpen())
            {
                if (interActText.gameObject.activeSelf) interActText.gameObject.SetActive(false);
            }
            else
            {
                SmoothLookAt(targetlook, turnSpeed);
                if (!interActText.gameObject.activeSelf) interActText.gameObject.SetActive(true);
                if (InputManager.InputActions.actions["Interact"].triggered)
                {
                    QuestManager.Instance.UpdateQuestProgress(QuestConditionType.Meet, currentNPCData.id);
                    Interact();
                }
            }
        }
    }

    void SmoothLookAt(Transform targetlook, float rotationSpeed)
    {
        Vector3 targetPosition = new Vector3(targetlook.position.x, transform.position.y, targetlook.position.z);
        Vector3 direction = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    private void GetMainNpcData()
    {
        List < NPCData> npclist = isMainQuest ? QuestManager.NpcDatabase.mainQuestNpcLists : QuestManager.NpcDatabase.npcLists;
        if (!isMainQuest)
        {
            foreach (NPCData npcData in npclist)
            {
                if (npcData.currentNPC == null && npcData.npcType == npcType)
                {
                    isInitNPC = true;
                    currentNPCData = npcData;
                    currentNPCData.currentNPC = this.gameObject;
                    break;
                }
            }
        }
        else
        {
            isInitNPC = true;
            currentNPCData = npclist[mainIndex - 1];
            currentNPCData.currentNPC = this.gameObject;
        }
        if (npcType == NPCType.상점)
        {
            npclist = QuestManager.NpcDatabase.shopNpcLists;
            isInitNPC = true;
            currentNPCData = npclist[mainIndex - 1];
            currentNPCData.currentNPC = this.gameObject;
        }

        switch (currentNPCData.npcType)
        {
            case NPCType.퀘스트:
                bool isHasQuestNew = !currentNPCData.quests.Any(quest => quest.isCompleted);
                interActText.InteractTextSetting("대화하기", 0, isHasQuestNew);
                break;
            case NPCType.상점:
                interActText.InteractTextSetting("상점열기", 0);
                break;
            default:
                capsuleCollider.radius = interactionRadius * 2.5f;
                string msg = string.Empty;
                if (currentNPCData.dialogue != null)
                {
                    msg = string.Empty;
                    foreach (string str in currentNPCData.dialogue)
                    {
                        msg += $"{str} + \n";
                    }
                }
                else
                {
                    msg = currentNPCData.description;
                }
                interActText.InteractTextSetting(msg, 1);
                break;
        }
        Canvas.ForceUpdateCanvases();
    }


    public void Interact()
    {
        if (!currentNPCData.isInteractable)
        {
            Debug.Log($"{currentNPCData.name}은(는) 상호작용이 불가능합니다.");
            return;
        }
        switch (currentNPCData.npcType)
        {
            case NPCType.상점:
                OpenShop();
                break;
            case NPCType.퀘스트:
                GiveQuest();
                break;
            default:
                HandleState();
                break;
        }

        PlayInteractionEffect();
        PlayVoice();
    }

    private void HandleState()
    {
        switch (currentNPCData.currentState)
        {
            case NPCState.중립:  // 일반 NPC 대화.
                Debug.Log($"{currentNPCData.name}은(는) 중립 상태입니다.");
                ShowDialogue();
                break;
            case NPCState.동료:
                FollowPlayer();
                break;
            case NPCState.적:
                AttackPlayer();
                break;
        }
    }

    public void ChangeState(NPCState newState)
    {
        Debug.Log($"{currentNPCData.name} 상태가 {currentNPCData.currentState}에서 {newState}(으)로 변경되었습니다.");
        currentNPCData.currentState = newState;
    }

    private void FollowPlayer()
    {
        Debug.Log($"{currentNPCData.name}은(는) 동료로서 플레이어를 따라다닙니다.");
        // 스토리 진행하다 동료로 활동하는 로직
    }

    private void AttackPlayer()
    {
        Debug.Log($"{currentNPCData.name}이(가) 적으로 변해 플레이어를 공격합니다!");
        // 스토리 진행하다 공격하는 방식의 로직
    }

    private void OpenShop()
    {
        GameStateMachine.Instance.ChangeState(GameSystemState.Shopping, currentNPCData);
    }

    private void GiveQuest()
    {
        if (currentNPCData.quests == null)
        {
            Debug.Log("퀘스트가 없습니다.");
            return;
        }
        UIManager.Instance.DisplayQuestDialogWindow(currentNPCData.name, currentNPCData.quests[0]);
        // 스토리 진행후 퀘스트를 제공하는 방식의 로직
    }

    private void ShowDialogue()
    {
        UIManager.Instance.DisplayDialogWindow(currentNPCData);
    }

    private void PlayInteractionEffect()
    {
        if (currentNPCData.interactionEffect != null)
        {
            Instantiate(currentNPCData.interactionEffect, transform.position, Quaternion.identity);
        }
    }

    private void PlayVoice()
    {
        if (currentNPCData.voiceLines != null && currentNPCData.voiceLines.Length > 0)
        {
            var clip = currentNPCData.voiceLines[Random.Range(0, currentNPCData.voiceLines.Length)];
            SoundManager.Instance.PlayClipAtPoint("QuestVoice", transform.position);
        }
    }

    private float GetHeight()
    {
        Collider collider = GetComponent<Collider>();

        if (collider != null)
        {
            float height = collider.bounds.size.y; // y축 크기가 높이
            return height;
        }
        return -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            targetlook = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            targetlook = null;
            if (interActText.gameObject.activeSelf) interActText.gameObject.SetActive(false);
            UIManager.Instance.UIClose();
        }
    }
}