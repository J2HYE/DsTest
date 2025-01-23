using UnityEngine;
using UnityEngine.UI;

public class DialogueBtn : MonoBehaviour
{
    [SerializeField] private Button dialogueBtn;
    [SerializeField] private Image dialogueImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueBtn.onClick.AddListener(DialogueBtnAct);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DialogueBtnAct()
    {

    }
}
