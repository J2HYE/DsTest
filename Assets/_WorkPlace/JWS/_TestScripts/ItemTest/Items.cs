using UnityEngine;


namespace JWS
{
    public class Items : MonoBehaviour
    {
        private float triggerArea = 3f; // 플레이어와의 트리거 거리
        private float moveSpeed = 20f; // 아이템이 이동하는 속도
        private Transform playerTransform;

        private void Start()
        {
            if (playerTransform == null)
                playerTransform = GameManager.playerTransform;
        }

        private void Update()
        {
            CheckDistance();
        }

        private void CheckDistance()
        {
            if (playerTransform == null)
                playerTransform = GameManager.playerTransform;

            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance < triggerArea)
            {
                ApproachedPlayer();
            }
        }

        private void ApproachedPlayer()
        {
            if (playerTransform == null)
                playerTransform = GameManager.playerTransform;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }

        public void PickUp()
        {
            int randomIndex = Random.Range(0, ItemManager.ItemDatabase.Count);
            Item itemData = ItemManager.ItemDatabase[randomIndex].Clone();
            if (itemData != null)
            {
                //Debug.Log($"[ItemManager] 아이템 '{itemData.name}' {itemData.quantity}개 생성완료");
                InventoryManager.Instance.AddItemLogic(itemData);
                UIManager.Instance.PickUpItemTextDisplay?.AddItem(itemData.name);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning($"[Items] 아이템을 찾을 수 없습니다.");
            }
        }

        private void OnTriggerEnter(Collider hit)
        {
            if (hit.CompareTag("Player"))
            {
                PickUp();
            }
            else
            {
                Debug.LogWarning("[OnControllerColliderHit] 충돌한 객체에 Items 컴포넌트가 없습니다.");
            }
        }
    }
}