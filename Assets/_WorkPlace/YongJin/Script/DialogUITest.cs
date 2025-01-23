using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class DialogUITest : MonoBehaviour
{    
    [SerializeField]private TMP_Text dialogueName;   // 이름 출력칸
    [SerializeField]private TMP_Text dialogueText; // 대사 출력칸
    private float typingSpeed = 0.05f; // 텍스트 출력 딜레이



    private List<string> dialogueList = new List<string>(); // JSON에서 불러온 대사 리스트
    private List<string> dialogueNameList = new List<string>(); // JSON에서 불러온 대사 리스트
    private int currentNPCDesaIndex = 0; // 현재 출력중인 일반 대사 인덱스 
    
    private Coroutine typingCoroutine;
    

    #region Npc일반대사
    public void StartDialogue(List<string> dialogueNameList, List<string> dialogueList)
    {
        if(currentNPCDesaIndex == 0)
        {
            DialogueStart();            
        }
        dialogueName.text = dialogueNameList[currentNPCDesaIndex];

        this.dialogueList = dialogueList;
        this.dialogueNameList = dialogueNameList;
        if(currentNPCDesaIndex >= dialogueList.Count)
        {
            DialogueExit();
            return;
        }
        if(typingCoroutine == null)
        {
            string typingDialogyeData = dialogueList[currentNPCDesaIndex];
            //UIManager.SystemGameMessage($"대사 출력 {dialogueNameList[currentNPCDesaIndex]}:{dialogueList[currentNPCDesaIndex]}", MessageTag.퀘스트);
            typingCoroutine = StartCoroutine(TypeNpcDialogue(typingDialogyeData));
        }
        else
        {
            SkipNpcDialogue();
        }
    }
     private IEnumerator TypeNpcDialogue(string dialgue)
    {
        dialogueText.text = "";
        
        foreach(char i in dialgue.ToCharArray())
        {
            dialogueText.text += i;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(1.5f);
        SkipNpcDialogue();

    }
    private void SkipNpcDialogue()
    {
        if(typingCoroutine != null) StopCoroutine(typingCoroutine);            
        dialogueText.text = dialogueList[currentNPCDesaIndex];
        currentNPCDesaIndex++;
        typingCoroutine = null;
        if(currentNPCDesaIndex >= dialogueList.Count)
        {
            Debug.Log("스킵 끝");
            DialogueExit();
        }
        else
        {
            string typingDialogyeData = dialogueList[currentNPCDesaIndex];
            //UIManager.SystemGameMessage($"대사 출력 {dialogueNameList[currentNPCDesaIndex]}:{dialogueList[currentNPCDesaIndex]}", MessageTag.퀘스트);
            typingCoroutine = StartCoroutine(TypeNpcDialogue(typingDialogyeData));
        }
        
    }

    

    #endregion
    
    
    
    
    public void DialogueStart()
    {
        gameObject.SetActive(true);
       
    }
    public void DialogueExit()
    {
        if(typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = null;
        gameObject.SetActive(false);
        currentNPCDesaIndex = 0;
    }
    


  


   


    
    

    
    
    
}
