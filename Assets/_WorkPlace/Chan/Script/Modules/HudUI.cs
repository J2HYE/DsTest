using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudUI : MonoBehaviour
{
    [SerializeField] private Image hp;
    [SerializeField] private Image mp;
    [SerializeField] private Image sta;
    [SerializeField] private TextMeshProUGUI history;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
