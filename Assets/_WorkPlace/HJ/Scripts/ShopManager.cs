using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : BaseManager<ShopManager>
{
    #region Variables
    [SerializeField] private List<ShopData> shops = new List<ShopData>();
    public static IReadOnlyList<ShopData> shopsDataList => Instance.shops;

    private int PlayerMoney
    {
        get { return CharacterManager.PlayerCharacterData.gold; }
    }
    #endregion




    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {

    }
}
