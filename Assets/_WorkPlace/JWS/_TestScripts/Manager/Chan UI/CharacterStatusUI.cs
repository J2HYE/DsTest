using TMPro;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusUI : MonoBehaviour
{
    private enum CharType { Player, Dragon }
    private CharType chraterType;
    [SerializeField] private Transform character_Stat;
    [SerializeField] private Transform statsNamePosition;
    [SerializeField] private Transform statsValuePosition;
    private TextMeshProUGUI[] statsNames;
    private string[] defaultNames;
    private TextMeshProUGUI[] statsValues;
    private Button[] buttons;
    private int currentButtonIndex = 0;

    void Awake()
    {
        statsNames = statsNamePosition.GetComponentsInChildren<TextMeshProUGUI>();
        statsValues = statsValuePosition.GetComponentsInChildren<TextMeshProUGUI>();
        buttons = transform.GetComponentsInChildren<Button>();
        defaultNames = new string[statsNames.Length];
        for (int i = 0; statsNames.Length > i; i++)
        {
            defaultNames[i] = statsNames[i].text;
        }
    }

    private void OnEnable()
    {
        AddButtonListeners();
    }

    private void OnDisable()
    {
        RemoveButtonListeners();
    }

    private void Start()
    {
        OnButtonClick(currentButtonIndex);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateUI()
    {
        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogWarning("버튼이 설정되지 않았거나 버튼 배열이 비어있습니다.");
            return;
        }
        switch (currentButtonIndex)
        {
            case 2:
                GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu);
                break;

            default:
                ShowCharacterInfo();
                break;
        }
    }

    private void ShowCharacterInfo()
    {
        switch (chraterType)
        {
            case CharType.Player:
                CameraManager.Instance.HandleUIviewState(GameManager.playerTransform);
                GetPlayerStatsValue();
                break;
            case CharType.Dragon:
                CameraManager.Instance.HandleUIviewState(GameManager.DragonTransform);
                GetDragonStatsValue();
                break;
        }
    }

    public void AddButtonListeners()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    public void RemoveButtonListeners()
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void OnButtonClick(int buttonIndex)
    {
        currentButtonIndex = buttonIndex;
        if (currentButtonIndex >= 0 && currentButtonIndex < 2)
        {
            Animator buttonAnimator = buttons[buttonIndex].animator;
            buttonAnimator.SetTrigger("Hover");
            chraterType = (CharType)buttonIndex;
        }
        UpdateUI();
    }

    private void GetPlayerStatsValue()
    {
        PlayerData playerData = CharacterManager.PlayerCharacterData;
        for (int i = 0; statsNames.Length > i; i++)
        {
            statsNames[i].text = defaultNames[i];
        }
        statsValues[0].text = playerData.currentHp.ToString();
        statsValues[1].text = playerData.currentMp.ToString();
        statsValues[2].text = playerData.staminaCurrent.ToString();
        statsValues[3].text = playerData.strength.ToString();
        statsValues[4].text = playerData.intelligence.ToString();
        statsValues[5].text = playerData.agility.ToString();
        statsValues[6].text = playerData.physicalDamage.ToString();
        statsValues[7].text = playerData.magicDamage.ToString();
        statsValues[8].text = playerData.physicalDefense.ToString();
        statsValues[9].text = playerData.magicDefense.ToString();
   
    }

    private void GetDragonStatsValue()
    {
        DragonData dragonData = CharacterManager.DragonData;

        statsNames[0].text = "Name";
        statsNames[1].text = "Lv";
        statsNames[2].text = "SPD";
        statsNames[6].text = dragonData.attackSpeed.ToString();
        statsNames[7].text = dragonData.bondExperience.ToString();
        statsNames[8].text = "";
        statsNames[9].text = "";

        statsValues[0].text = dragonData.characterName.ToString();
        statsValues[1].text = dragonData.bondLevel.ToString(); ;
        statsValues[2].text = dragonData.speed.ToString(); 
        statsValues[3].text = dragonData.strength.ToString();
        statsValues[4].text = dragonData.intelligence.ToString();
        statsValues[5].text = dragonData.strength.ToString();
        statsValues[6].text = dragonData.attackSpeed.ToString();
        statsValues[7].text = dragonData.bondExperience.ToString();
        statsValues[8].text = "";
        statsValues[9].text = "";
    }
}
