using TMPro;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private GameObject statPrefab1; // 첫 번째 프리팹
    [SerializeField] private GameObject statPrefab2; // 두 번째 프리팹
    [SerializeField] private Transform statPanel;    // 스탯 슬롯들이 추가될 부모 오브젝트

    private PlayerData playerData; //

    private void Start()
    {
        // 필요한 필드 확인
        if (statPrefab1 == null || statPrefab2 == null || statPanel == null)
        {
            Debug.LogError("StatUI에 필요한 프리팹 또는 부모 오브젝트가 연결되지 않았습니다.");
            return;
        }

        // CharacterManager에서 PlayerCharacterData 가져오기
        playerData = CharacterManager.PlayerCharacterData;

        if (playerData == null)
        {
            Debug.LogError("PlayerCharacterData가 null입니다. CharacterManager를 확인하세요.");
            return;
        }

        // 스탯 슬롯 생성
        GenerateStatSlots();
    }

    // 스탯 슬롯 생성 함수
    private void GenerateStatSlots()
    {
        var stats = new (string statName, string value)[]
        {
            ("Strength", playerData.strength.ToString()),
            ("Vitality", playerData.vitality.ToString()),
            ("Agility", playerData.agility.ToString()),
            ("Intelligence", playerData.intelligence.ToString()),
            ("Stamina", $"{playerData.staminaCurrent}/{playerData.stamina}"),
            ("Physical Defense", playerData.physicalDefense.ToString()),
            ("Magic Defense", playerData.magicDefense.ToString()),
            ("Physical Damage", playerData.physicalDamage.ToString()),
            ("Magic Damage", playerData.magicDamage.ToString())
        };

        // 번갈아 가면서 생성
        for (int i = 0; i < stats.Length; i++)
        {
            GameObject prefabToUse = i % 2 == 0 ? statPrefab1 : statPrefab2;
            var slot = Instantiate(prefabToUse, statPanel);

            var texts = slot.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                texts[0].text = stats[i].statName; // 스탯 이름 적용
                texts[1].text = stats[i].value;   // 스탯 값 적용 
            }
        }
    }
}
