using UnityEngine;

public class TestMonster : MonoBehaviour
{
    public MonsterData monsterData;

    private void Start()
    {
        monsterData = CharacterManager.Instance.CreateCharacterFromTemplate("MophanS");
    }
}
