using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainCutSceneManager : BaseManager<MainCutSceneManager>
{

    [SerializeField] private Transform timeLineManager;
    [SerializeField] private Transform fadeBgTransform;
    [SerializeField] private Transform Bg;
    
    private Image fadeBg;
    public AnimationCurve curve;
    private int mainSceneId;
    private int chapterId;

    
    public void OnCutScene(int mainSceneId, int chapterId)
    {
        if(this.mainSceneId > 0)
        {
            Debug.Log($"컷 씬 진행중 이따 신호 보내세요");    
            return;
        }
        Debug.Log($"컷 씬 시작");    
        this.mainSceneId = mainSceneId;
        this.chapterId = chapterId;
        StartCoroutine(CutSceneFadeOut());
    }

    protected override void Start() 
    {
        base.Start();
        for(int i = 0; i < timeLineManager.childCount; i++)
        {
            timeLineManager.GetChild(i).gameObject.SetActive(false);
            
        }
        fadeBg = fadeBgTransform.GetComponent<Image>();
    }
    
    IEnumerator CutSceneFadeOut()
    {
        // 1초 동안 이미지의 알파값이 a 1->0으로
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            fadeBg.color = new Color(0f, 0f, 0f, a); // (r ,g ,b 는 검은색)

            yield return 0f;
        }
        CutSceneStart();
    }

    private void CutSceneStart()
    {
        
        Debug.Log($"메인{mainSceneId}, 챕터{chapterId} 컷 씬 시작");
        fadeBgTransform.gameObject.SetActive(false);
        Bg.gameObject.SetActive(true);
        var mainScene = timeLineManager.GetChild(mainSceneId-1);
        mainScene.gameObject.SetActive(true);
        for(int i = 0; i < timeLineManager.GetChild(mainSceneId-1).childCount; i++)//혹시 모를 초기화
        {
            mainScene.GetChild(i).gameObject.SetActive(false);
        }
        mainScene.GetChild(chapterId-1).gameObject.SetActive(true);
    }
    public void CutSceneEndSignal()
    {
        
        Debug.Log($"메인{mainSceneId}컷 씬 종료됨");
        var mainScene = timeLineManager.GetChild(mainSceneId-1);
        mainScene.gameObject.SetActive(false);
        for(int i = 0; i < timeLineManager.GetChild(mainSceneId-1).childCount; i++)//혹시 모를 초기화
        {
            mainScene.GetChild(i).gameObject.SetActive(false);
        }
        mainSceneId = 0;
        chapterId = 0;
        fadeBgTransform.gameObject.SetActive(true);
        Bg.gameObject.SetActive(false);
        StartCoroutine(CutSceneFadeIn());
    }
    private IEnumerator CutSceneFadeIn()
    {
        // 1초 동안 이미지의 알파값이 a 1->0으로
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            fadeBg.color = new Color(0f, 0f, 0f, a); // (r ,g ,b 는 검은색)
            yield return 0f;
        }
    }
    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {

    }
}
