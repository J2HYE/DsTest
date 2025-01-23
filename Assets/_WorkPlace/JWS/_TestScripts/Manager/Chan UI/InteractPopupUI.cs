using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractPopupUI : MonoBehaviour
{
    private GameObject[] interactPannels;
    private Transform panelParent;
    private Dictionary<GameObject, (TextMeshProUGUI, TextMeshProUGUI)> interactPannelsDict = new Dictionary<GameObject, (TextMeshProUGUI, TextMeshProUGUI)>();
    [SerializeField] private Queue<GameObject> inactivePanels = new Queue<GameObject>();
    [SerializeField] private List<GameObject> activePanels = new List<GameObject>();


    void Start()
    {
        panelParent = transform.GetChild(0).GetChild(0);
        int childCount = panelParent.childCount;
        interactPannels = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            GameObject panel = panelParent.GetChild(i).gameObject;
            interactPannels[i] = panel;
            TextMeshProUGUI[] textComponents = panel.GetComponentsInChildren<TextMeshProUGUI>(true);
            if (textComponents.Length >= 2)
            {
                interactPannelsDict.Add(panel, (textComponents[0], textComponents[1]));
            }
            else
            {
                Debug.LogWarning($"패널 '{panel.name}'에서 TextMeshProUGUI 컴포넌트가 부족합니다.");
            }
            panel.SetActive(false);
            inactivePanels.Enqueue(panel);
        }
    }

    public void UpdateInteractText(string name, string comment, bool isOn)
    {
        if (isOn)
        {
            if (inactivePanels.Count > 0)
            {
                GameObject panel = inactivePanels.Dequeue();
                activePanels.Add(panel);
                var texts = interactPannelsDict[panel];
                texts.Item1.text = name;
                texts.Item2.text = comment;
                panel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("활성화할 수 있는 패널이 없습니다!");
            }
        }
        else
        {
            GameObject panelToDisable = activePanels.Find(panel =>
                interactPannelsDict[panel].Item1.text == name);

            if (panelToDisable != null)
            {
                panelToDisable.SetActive(false);
                activePanels.Remove(panelToDisable);
                inactivePanels.Enqueue(panelToDisable);
            }
            else
            {
                Debug.LogWarning($"'{name}'과 일치하는 활성 패널을 찾을 수 없습니다.");
            }
        }
    }
}
