using UnityEngine;

public class ItemGenerater
{

    public Item GenerateItem(string itemName)
    {
        // 아이템 속성 매핑
        switch (itemName)
        {
            case "용의알":
                return CreateItem("001", "용의알", "용의 알.",
                    ItemType.퀘스트, ItemGrade.일반);
            
            case "가죽갑옷":
                return CreateItem("001", "가죽갑옷", "튼튼한 가죽으로 만들어진 갑옷입니다.",
                    ItemType.방어구, ItemGrade.일반, EquipmentSlot.몸);

            case "가죽장갑":
                return CreateItem("002", "가죽장갑", "가죽으로 만들어진 손 보호 장갑입니다.",
                    ItemType.방어구, ItemGrade.일반, EquipmentSlot.손);

            case "경험치":
                return CreateItem("003", "경험치", "사용 시 경험치를 획득합니다.",
                    ItemType.소모품, ItemGrade.고급, consumableType: ConsumableType.버프);

            case "고급목걸이":
                return CreateItem("004", "고급목걸이", "희귀한 광물로 제작된 목걸이입니다.",
                    ItemType.장신구, ItemGrade.희귀, EquipmentSlot.머리);

            case "고기":
                return CreateItem("005", "고기", "신선한 고기입니다. 체력을 회복시킵니다.",
                    ItemType.소모품, ItemGrade.일반, consumableType: ConsumableType.체력포션);

            case "고대유물":
                return CreateItem("006", "고대유물", "고대의 신비로운 유물입니다. 특별한 용도로 사용됩니다.",
                    ItemType.퀘스트, ItemGrade.희귀);

            case "고블린도끼":
                return CreateItem("007", "고블린도끼", "고퀄리티 무기입니다.",
                    ItemType.퀘스트, ItemGrade.일반);

            case "곡괭이":
                return CreateItem("008", "곡괭이", "광석을 채굴할 수 있는 도구입니다.",
                    ItemType.제작재료, ItemGrade.일반);

            case "금화":
                return CreateItem("009", "금화", "귀중한 통화입니다.",
                    ItemType.제작재료, ItemGrade.고급);

            case "나무":
                return CreateItem("010", "나무", "여러 용도로 사용 가능한 재료입니다.",
                    ItemType.제작재료, ItemGrade.일반);

            case "나무완드":
                return CreateItem("011", "나무완드", "초보자를 위한 나무 마법 지팡이입니다.",
                    ItemType.무기, ItemGrade.일반, EquipmentSlot.손, WeaponType.한손무기);

            case "나뭇가지":
                return CreateItem("012", "나뭇가지", "나뭇가지입니다. 연료로 사용할 수 있습니다.",
                    ItemType.무기, ItemGrade.일반, EquipmentSlot.손, WeaponType.한손무기);

            case "낡은검":
                return CreateItem("013", "낡은검", "오래된 검입니다. 기본적인 무기입니다.",
                    ItemType.무기, ItemGrade.일반, EquipmentSlot.손, WeaponType.한손무기);

            case "낡은반지":
                return CreateItem("014", "낡은반지", "오래된 반지입니다.",
                    ItemType.장신구, ItemGrade.일반, EquipmentSlot.손);

            case "도끼":
                return CreateItem("015", "도끼", "나무를 벨 수 있는 도구입니다.",
                    ItemType.무기, ItemGrade.일반, EquipmentSlot.손, WeaponType.양손무기);

            case "돌":
                return CreateItem("016", "돌", "작은 돌멩이입니다. 여러 용도로 사용됩니다.",
                    ItemType.제작재료, ItemGrade.일반);

            case "마나포션":
                return CreateItem("017", "마나포션", "사용 시 마나를 회복합니다.",
                    ItemType.소모품, ItemGrade.일반, consumableType: ConsumableType.마나포션);

            case "마법보석":
                return CreateItem("018", "마법보석", "마법의 힘을 지닌 희귀한 보석입니다.",
                    ItemType.제작재료, ItemGrade.희귀);

            case "목걸이":
                return CreateItem("019", "목걸이", "평범한 목걸이입니다. 장식용으로 사용됩니다.",
                    ItemType.장신구, ItemGrade.일반, EquipmentSlot.머리);

            case "무":
                return CreateItem("020", "무", "먹을 수 있는 채소입니다.",
                    ItemType.소모품, ItemGrade.일반, consumableType: ConsumableType.체력포션);

            case "미트":
                return CreateItem("021", "미트", "풍미 가득한 고기입니다. 체력을 회복시킵니다.",
                    ItemType.소모품, ItemGrade.일반, consumableType: ConsumableType.체력포션);

            case "방패":
                return CreateItem("022", "방패", "적의 공격을 막아주는 기본 방패입니다.",
                    ItemType.방어구, ItemGrade.일반, EquipmentSlot.방패);

            case "버섯":
                return CreateItem("023", "버섯", "숲에서 자란 평범한 버섯입니다.",
                    ItemType.소모품, ItemGrade.일반, consumableType: ConsumableType.체력포션);

            case "보물지도":
                return CreateItem("024", "보물지도", "숨겨진 보물의 위치를 표시한 지도입니다.",
                    ItemType.퀘스트, ItemGrade.고급);

            case "비트":
                return CreateItem("025", "비트", "달콤한 맛이 나는 뿌리 채소입니다.",
                    ItemType.소모품, ItemGrade.일반, consumableType: ConsumableType.체력포션);

            case "빛의폭발":
                return CreateItem("026", "빛의폭발", "강력한 빛의 힘을 가진 스크롤입니다.",
                    ItemType.소모품, ItemGrade.희귀, consumableType: ConsumableType.버프);

            case "사과":
                return CreateItem("027", "사과", "싱싱한 사과입니다. 체력을 회복시킵니다.",
                    ItemType.소모품, ItemGrade.일반, consumableType: ConsumableType.체력포션);

            case "양손검":
                return CreateItem("028", "양손검", "양손으로 사용하는 강력한 검입니다.",
                    ItemType.무기, ItemGrade.고급, EquipmentSlot.손, WeaponType.양손무기);

            case "영혼반지":
                return CreateItem("029", "영혼반지", "착용자에게 특별한 능력을 부여하는 반지입니다.",
                    ItemType.장신구, ItemGrade.희귀, EquipmentSlot.손);

            case "영혼의수정":
                return CreateItem("030", "영혼의수정", "강력한 영혼 에너지가 담긴 수정입니다.",
                    ItemType.제작재료, ItemGrade.희귀);

            case "완드":
                return CreateItem("031", "완드", "초보자 마법사가 사용하는 나무 지팡이입니다.",
                    ItemType.무기, ItemGrade.일반, EquipmentSlot.손, WeaponType.한손무기);

            case "은화":
                return CreateItem("032", "은화", "귀중한 통화로 거래에 사용됩니다.",
                    ItemType.제작재료, ItemGrade.고급);

            case "잃어버린편지":
                return CreateItem("033", "잃어버린편지", "어딘가로 보내야 할 중요한 편지입니다.",
                    ItemType.퀘스트, ItemGrade.일반);

            case "장갑":
                return CreateItem("034", "장갑", "손을 보호해주는 평범한 장갑입니다.",
                    ItemType.방어구, ItemGrade.일반, EquipmentSlot.손);

            case "지도":
                return CreateItem("035", "지도", "주변 지역을 표시한 지도입니다.",
                    ItemType.퀘스트, ItemGrade.일반);

            case "철검":
                return CreateItem("036", "철검", "단단한 철로 제작된 검입니다.",
                    ItemType.무기, ItemGrade.고급, EquipmentSlot.손, WeaponType.한손무기);

            case "체력포션":
                return CreateItem("037", "체력포션", "사용 시 체력을 회복합니다.",
                    ItemType.소모품, ItemGrade.일반, consumableType: ConsumableType.체력포션);

            case "초급공격스크롤":
                return CreateItem("038", "초급공격스크롤", "초급 공격 마법을 사용할 수 있는 스크롤입니다.",
                    ItemType.소모품, ItemGrade.희귀, consumableType: ConsumableType.버프);

            case "초록포션":
                return CreateItem("039", "초록포션", "초록빛을 띠는 특이한 포션입니다.",
                    ItemType.소모품, ItemGrade.고급, consumableType: ConsumableType.버프);

            case "캐터필러":
                return CreateItem("040", "캐터필러", "어딘가의 퀘스트에 사용되는 아이템입니다.",
                    ItemType.퀘스트, ItemGrade.일반);

            case "티네임":
                return CreateItem("041", "티네임", "어떤 의식을 위해 사용되는 특별한 이름입니다.",
                    ItemType.퀘스트, ItemGrade.고급);

            case "파란포션":
                return CreateItem("042", "파란포션", "푸른빛을 띠는 신비로운 포션입니다.",
                    ItemType.소모품, ItemGrade.희귀, consumableType: ConsumableType.마나포션);

            case "황금열쇠":
                return CreateItem("043", "황금열쇠", "특별한 보물을 열 수 있는 열쇠입니다.",
                    ItemType.퀘스트, ItemGrade.전설);


            default:
                Debug.LogError($"아이템 이름 {itemName}을(를) 찾을 수 없습니다.");
                return null;
        }
    }

    private Item CreateItem(string id, string name, string description, ItemType type,
        ItemGrade grade, EquipmentSlot equipmentSlot = EquipmentSlot.머리, WeaponType weaponType = WeaponType.한손무기,
        ConsumableType consumableType = ConsumableType.없음, int quantity = 1, int costValue = 100)
    {
        Item newItem = new Item(name, name, description, type, grade, quantity);

        if (type == ItemType.무기 || type == ItemType.방어구 || type == ItemType.장신구)
        {
            newItem.equipmentSlot = equipmentSlot;
            newItem.isStackable = false;
            newItem.maxStack = 1;
            if (type == ItemType.무기)
            {
                newItem.weaponType = weaponType;
            }
        }
        else if (type == ItemType.소모품)
        {
            //int randInt = Random.Range(0, 3);
            string size = string.Empty;
            //switch (randInt)
            //{
            //    case 0:
            //        size = "소형";
            //        break;
            //    case 1:
            //        size = "중형";
            //        break;
            //    case 2:
            //        size = "대형";
            //        break;
            //}
            newItem.name = $"소형 {newItem.id}";
            newItem.isStackable = true;
            newItem.consumableType = consumableType;
            newItem.maxStack = 99;
            newItem.effectAmount = costValue / 10; // 예: 효과량은 가격에 기반
        }
        if (type == ItemType.퀘스트) newItem.isQuestItem = true;

        newItem.sprite = ItemManager.Instance.GetItemSprite(newItem.id); // 이름에 맞는 스프라이트 연결
        newItem.id = newItem.name.Trim();
        newItem.costValue = costValue;
        return newItem;
    }
}
