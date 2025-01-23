using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleStatusUI : MonoBehaviour
{

    private void Update()
    {
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = CharacterManager.PlayerCharacterData.characterName;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"Lv\n{CharacterManager.PlayerCharacterData.level}";
        UpdateStatusBar();
    }

    private void UpdateStatusBar()
    {
        float result = 0f;
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            TextMeshProUGUI amount = transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
            Image fillbar = amount.transform.parent.GetComponent<Image>();
            switch (i)
            {
                case 0:
                    amount.text = $"{CharacterManager.PlayerCharacterData.currentHp.ToString("F0")} / {CharacterManager.PlayerCharacterData.maxHp}";
                    result = (float)CharacterManager.PlayerCharacterData.currentHp /
                             (float)CharacterManager.PlayerCharacterData.maxHp;
                    fillbar.fillAmount = result;
                    
                    break;
                case 1:
                    amount.text = $"{CharacterManager.PlayerCharacterData.currentMp.ToString("F0")} / {CharacterManager.PlayerCharacterData.maxMp}";
                    result = (float)CharacterManager.PlayerCharacterData.currentMp /
                             (float)CharacterManager.PlayerCharacterData.maxMp;
                    fillbar.fillAmount = result;
                    break;
                case 2:
                    amount.text = $"{CharacterManager.PlayerCharacterData.staminaCurrent.ToString("F0")} / {CharacterManager.PlayerCharacterData.stamina}";
                    result = (float)CharacterManager.PlayerCharacterData.staminaCurrent /
                             (float)CharacterManager.PlayerCharacterData.stamina;
                    fillbar.fillAmount = result;
                    break;
            }
        }
    }
}
