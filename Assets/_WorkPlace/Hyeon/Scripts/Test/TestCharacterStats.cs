using System.Xml.Linq;
using UnityEditor.Playables;
using UnityEngine;

[System.Serializable]
public class TestCharacterStats
{
    public CharacterData playerCharacterData;

    public float characterWalkSpeed;
    public float characterSprintSpeed;
    public TestCharacterStats ()
    {
        //playerCharacterData = new CharacterData(
        //    "Player", CharacterType.Player, 10, 10, 10, 10,
        //3f, 1f, 100f, 5f);
        //this.characterSprintSpeed = 4f;
        //this.characterWalkSpeed = 2f;
    }
}
