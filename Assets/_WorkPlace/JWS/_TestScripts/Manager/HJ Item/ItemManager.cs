using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine;

public class ItemManager : BaseManager<ItemManager>
{
    [SerializeField] private ItemList itemList;
    [SerializeField] private List<Sprite> itemSpriteList = new List<Sprite>();
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private float dropItemDestroyTime = 180f;      // 드랍아이템 사라지는 시간.
    [SerializeField] private GameObject[] ItemEffectPrefab;

    public static float DropItemDestroyTime => Instance.dropItemDestroyTime;


    public static List<Item> ItemDatabase => Instance.itemList.itemList;
    private Dictionary<string, Sprite> itemSpriteDictionary = new Dictionary<string, Sprite>(); // 스프라이트 딕셔너리

    protected override void Awake()
    {
        base.Awake();
        ItemGenerater itemGenerater = new ItemGenerater();
        // itemList.itemList.Clear();
        Addressables.LoadAssetsAsync<Sprite>("ItemSprites", sprite =>
        {
            if (!itemSpriteDictionary.ContainsKey(sprite.name))
            {
                itemSpriteDictionary[sprite.name] = sprite; // 스프라이트 딕셔너리에 추가
                itemSpriteList.Add(sprite); // 스프라이트 리스트에도 추가
                // Item item = itemGenerater.GenerateItem(sprite.name);
                // if (item != null) itemList.itemList.Add(item);
            }
        }).Completed += handle =>
        {
            Debug.Log($"{itemSpriteList.Count}개의 아이템 스프라이트를 로드했습니다.");
        };

        if (itemList.itemList.Count <= 0)
        {
            GenerateData generateData = new GenerateData();
            generateData.InitializeItems(itemList);
        }
        foreach (var item in itemList.itemList)
        {
            if (item.quantity == 0 ) item.quantity = 1;
            if (item.type == ItemType.무기 || item.type == ItemType.방어구)
            {
                item.itemStat.Initialize();
            }
        }
    }

    public void AddItemLogic(string itemId, int quantity = 1)
    {
        var item = GetItemById(itemId);
        item.quantity = quantity;
        if (item != null)
        {
            InventoryManager.Instance.AddItemLogic(item);
        }
        else
        {
            Debug.LogWarning($"[ItemManager] 아이템 ID '{itemId}'를 데이터베이스에서 찾을 수 없습니다.");
        }
    }

    public void RemoveItemLogic(string itemId, int quantity = 1)
    {
        var item = GetItemById(itemId);
        if (item != null)
        {
            InventoryManager.Instance.RemoveItemLogic(item.id, quantity);
            Debug.Log($"[ItemManager] 아이템 '{item.name}' {quantity}개 제거 완료");
        }
        else
        {
            Debug.LogWarning($"[ItemManager] 아이템 ID '{itemId}'를 데이터베이스에서 찾을 수 없습니다.");
        }
    }

    public Item GetItemById(string itemId)
    {
        var item = ItemDatabase.FirstOrDefault(i => i.id == itemId);
        if (item == null)
        {
            Debug.LogWarning($"[ItemManager] ID: {itemId} 아이템을 찾을 수 없습니다.");
        }
        return item;
    }

    public void UseItem(Item item, int quantity = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("[InventoryManager] 사용할 아이템이 존재하지 않습니다.");
            return;
        }
        if (InventoryManager.Instance.GetItemQuantity(item.id) < quantity)
        {
            Debug.LogWarning($"[InventoryManager] '{item.name}'의 수량이 부족합니다. 요청: {quantity}, 현재: {InventoryManager.Instance.GetItemQuantity(item.id)}");
            return;
        }
        UIManager.SystemGameMessage($"[InventoryManager] '{item.name}' 아이템 {quantity}개 사용", MessageTag.아이템_사용);
        InventoryManager.Instance.RemoveItemLogic(item.id, quantity);

        // TODO
        // 아이템 사용 효과 적용 (예: 체력 회복 등) 효과 구현연결
        // ApplyItemEffect(item, quantity); 이런식의 연결함수 구현이 필요.

        ItemEffectManager.Instance.ApplyItemEffect(item, quantity);
    }

    public void DropItem(Item item, int quantity = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("[InventoryManager] 버릴 아이템이 존재하지 않습니다.");
            return;
        }
        if (InventoryManager.Instance.GetItemQuantity(item.id) < quantity)
        {
            Debug.LogWarning($"[InventoryManager] '{item.name}'의 수량이 부족합니다. 요청: {quantity}, 현재: {InventoryManager.Instance.GetItemQuantity(item.id)}");
            return;
        }
        UIManager.SystemGameMessage($"[InventoryManager] '{item.name}' 아이템 {quantity}개 버림", MessageTag.아이템_버림);
        InventoryManager.Instance.RemoveItemLogic(item.id, quantity);

        // TODO
        // 추가 드롭 연출(필요 시)
        // CreateDroppedItemInWorld(item, quantity); ; 이런식의 연결함수 구현이 필요.
    }

    //아이템 구매
    public void PurchaseItem(Item buyItem, int quantity = 1)
    {
        int amount = buyItem.costValue * quantity;
        Debug.Log("플레이어 : " + amount + " , " + buyItem.costValue + " , " + quantity);
        if (CharacterManager.PlayerCharacterData.gold < amount)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.InfoMessage, "보유한 금액 부족합니다.", true);
            return;
        }
        if (InventoryManager.Instance.CanAddInventoryItem(buyItem.id, quantity) && CharacterManager.PlayerCharacterData.UseGold(amount))
        {
            AddItemLogic(buyItem.id, quantity);
        }
    }

    public void SellItem(Item selltem, float valueReductionRate, int quantity = 1)
    {
        int totalQuantity = InventoryManager.InventoryList.Where(i => i.id == selltem.id).Sum(i => i.quantity);
        if (totalQuantity < quantity)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.InfoMessage, "아이템 수량 부족.", true);
            return;
        }
        int amount = ((int)(selltem.costValue * valueReductionRate) * quantity);
        CharacterManager.PlayerCharacterData.AddGold(amount);
        InventoryManager.Instance.RemoveItemLogic(selltem.id, quantity);
    }

    public Sprite GetItemSprite(string spriteName)
    {
        if (itemSpriteDictionary.TryGetValue(spriteName, out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning($"'{spriteName}'에 해당하는 스프라이트를 찾을 수 없습니다.");
        return null;
    }

    public GameObject SpawnItemBox(Vector3 spawnPosition, MonsterData monsterData, bool isRandom = true)
    {
        GameObject itemBox = Instantiate(dropItemPrefab, spawnPosition, Quaternion.identity);
        if (!isRandom)
        {
            itemBox.transform.GetComponent<DropItemBoxController>().isRandomDrop = false;
            itemBox.transform.GetComponent<DropItemBoxController>().dropItemIds = monsterData.dropItems;
            Debug.Log(monsterData.dropItems.Count);
        }
        return itemBox;
    }

    /// <summary>
    /// 01.17 희정 추가한 함수
    /// </summary>
    //타입과 등급으로 해당 아이템 가져오기
    public List<Item> GetItemsByTypeAndGrade(ItemType itemType, ItemGrade grade)
    {
        return ItemDatabase.Where(i => i.type == itemType && i.grade >= grade).ToList();
    }

    //특정 아이템 리스트
    public List<Item> GetSpeificItem(List<string> itemids)
    {
        List<Item> items = new List<Item>();
        foreach (var itemid in itemids)
        {
            items.Add(GetItemById(itemid));
        }

        return items;
    }



    protected override void HandleGameStateChange(global::GameSystemState newState, object additionalData)
    {

    }
}
