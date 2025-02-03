using UnityEngine;
using System.Collections.Generic;

public class DropItemBoxController : MonoBehaviour
{
    public List<string> dropItemIds = new List<string>();
    [SerializeField] private float detectionDistance = 2f;
    public bool isRandomDrop = true;

    private void Start()
    {
        Destroy(this.gameObject, DsConstValue.DROP_ITEM_DESTROY_INTERVAL);
    }

    private void Update()
    {
        if (InputManager.InputActions.actions["Interact"].triggered)
        {
            OpenBox(dropItemIds);
        }
    }

    public void OpenBox(List<string> items)
    {
        if (!IsNearPlayer())
            return;

        if (isRandomDrop)
        {
            Debug.Log("희정님 아이템메니저 이용시");
            // //아이템 데이터 리스트에서 드랍 확률 체크 후 인벤토리에 추가
            // items.ForEach(id =>
            // {
            //     Item item = ItemManager.Instance.FindItemById(id);
            //     //아이템 랜덤 생성
            //     if (item != null && Random.value <= item.itemDropChance)
            //     {
            //         item.Initialize(); //드랍할 아이템 초기화
            //         ItemManager.Instance.AddItemToInventory(item);  //인벤토리에 아이템 추가
            //     }
            // });

        }
        else
        {
            foreach (var itemId in items)
            {
                ItemManager.Instance.AddItemLogic(itemId);
            }
        }

        Destroy(gameObject); //박스 삭제
    }

    private bool IsNearPlayer()
    {
        float disatance = Vector3.Distance(transform.position, GameManager.playerTransform.position);
        return disatance <= detectionDistance;
    }
}
