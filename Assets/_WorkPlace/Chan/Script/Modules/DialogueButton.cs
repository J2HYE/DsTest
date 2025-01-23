using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
    }

    void init()
    {
        text.text = "";
        

    }
}
