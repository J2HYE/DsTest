using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public Button button;
    public int mainSceneId;
    public int chapterId;
    

    private void Awake() 
    {
        button.onClick.AddListener(BtnAct);
    }

    private void BtnAct()
    {
        MainCutSceneManager.Instance.OnCutScene(mainSceneId, chapterId);
    }
}
