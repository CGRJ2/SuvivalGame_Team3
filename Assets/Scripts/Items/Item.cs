using UnityEngine;

public class Item : ScriptableObject
{
    public ItemType itemType;       //아이템 타입
    public string itemName;         //아이템 이름
    [TextArea(3, 10)]
    public string description;         //아이템 설명
    public Sprite imageSprite;        //아이템 이미지
    public GameObject instancePrefab;   //아이템 프리펩

    public int maxCount; // 한 칸당 최대 보유 가능 수량

    // 공통 초기화 가능
    protected virtual void OnEnable()
    {
        // 기본 아이템인스턴스 프리펩 설정. 특수 아이템의 경우 아이템인스턴스 프리펩을 다른걸로 달아주면 됨
        if (instancePrefab == null)
            instancePrefab = Resources.Load<GameObject>("ItemInstancePrefabs/ItemInstance_Noraml");

        if (imageSprite == null)
            imageSprite = Resources.Load<Sprite>("Sprites/ItemIcons/DefaultImage");
    }

    // 사용 => 인벤토리 우클릭 상호작용 시 호출
    public void UseInInventory(SlotData slotData, int multieUseCount = 1)
    {
        // 장비 아이템
        if (this is IEquipable equipable)
        {
            //equipable.EquipToQuickSlot();
            // 이미 인벤토리에서 처리 함
        }
        // 소비 아이템 (소비 아이템인데 장착 가능한 애들은 따로 장착까지만 가능)
        else if (this is Item_Consumable consumable)
        {
            // 소비템 종류 별 효과 실행
            ConsumableSorting(consumable);

            // 슬롯 데이터에서 개수 줄이기
            consumable.Consume(slotData, multieUseCount);
            PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();
        }
        // 레시피 아이템이라면
        else if (this is Item_Recipe recipe)
        {
            recipe.UnlockThisRecipe();
            // 만약 레시피 사용해서 언락해서 소비시켜줄거면 SlotData.C
            slotData.CleanSlotData();
            PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();
        }
    }

    // 기능실행 => 플레이어가 장착한 상태에서 공격할 시 호출
    public void ActivateEffectOnAttack(SlotData slotData) // 여기서 slotData는 현재 활성화 되어있는 퀵슬롯의 원본 슬롯 데이터를 참조한다
    {
        // 장비 아이템
        if (this is IEquipable equipable)
        {
            if (this is Item_Throwing throwing)
            {
                if (throwing is Item_JumpPad jumpPad)
                    jumpPad.OnAttackEffect();
                else
                    throwing.OnAttackEffect();
                throwing.Consume(slotData);

                PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();
            }
        }
        // 소비 아이템 (소비 아이템인데 장착 가능한 애들은 따로 장착까지만 가능)
        else if (this is Item_Consumable consumable)
        {
            // 소비템 종류 별 효과 실행
            ConsumableSorting(consumable);

            // 슬롯 데이터에서 개수 줄이기
            consumable.Consume(slotData);

            // 퀵슬롯 & 인벤토리 UI 업데이트
            PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();

        }
    }

    // 소비템 종류 별 분류해서 각 종류에 맞는 효과 실행
    private void ConsumableSorting(Item_Consumable consumable)
    {
        if (consumable is Item_HealingHP hp)
        {
            hp.Hp_HealingCriticalPart();
        }
        else if (consumable is Item_BodyPart part)
        {
            part.Hp_Init();
        }
        else if (consumable is Item_ChargeBattery charge)
        {
            charge.Battery_Healing();
        }
        else if (consumable is Item_NewBattery battery)
        {
            battery.Battery_Init();
        }
        else
        {
            consumable.ConsumeEffectInvoke();
        }
    }


    // 아이템 인스턴스 생성
    public void SpawnItem(Transform transform, int count = 1)
    {
        GameObject instanceObj = Instantiate(instancePrefab, transform.position, transform.rotation);
        Rigidbody rb = instanceObj.GetComponent<Rigidbody>();

        // 힘을 커스텀할 수 있게 다른곳에 설정 가능한 변수를 받아오는 식으로 하자
        if (rb != null) rb.AddForce(Vector3.up * 3, ForceMode.Impulse);

        ItemInstance instance = instanceObj.GetComponent<ItemInstance>();
        instance.InitInstance(this, count);
    }
}

public enum ItemType
{
    Equipment, // 장비
    Consumalbe, // 소모품
    Ingredient, // 재료
    Function, // 기능
    Quest, //퀘스트
    ETC, // 기타
    AllType
}

[System.Serializable]
public class ItemRequirement
{
    public Item item;
    public int count;
}


public interface IEquipable
{
    //public void EquipToQuickSlot();

    public void OnAttackEffect();
}

public interface IConsumable
{
    public void Consume(SlotData slotData, int multieUseCount = 1);
}

