using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemList", menuName = "Ds Project/ItemList")]
public class ItemList : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
}
