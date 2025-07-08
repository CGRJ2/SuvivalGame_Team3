using UnityEngine;

public class Item : ScriptableObject
{
    public ItemType itemType;       //������ Ÿ��
    public string itemName;         //������ �̸�
    [TextArea(3, 10)]
    public string description;         //������ ����
    public Sprite imageSprite;        //������ �̹���
    public GameObject instancePrefab;   //������ ������

    public int maxCount; // �� ĭ�� �ִ� ���� ���� ����

    // ���� �ʱ�ȭ ����
    protected virtual void OnEnable()
    {
        // �⺻ �������ν��Ͻ� ������ ����. Ư�� �������� ��� �������ν��Ͻ� �������� �ٸ��ɷ� �޾��ָ� ��
        if (instancePrefab == null)
            instancePrefab = Resources.Load<GameObject>("ItemInstancePrefabs/ItemInstance_Noraml");

        if (imageSprite == null)
            imageSprite = Resources.Load<Sprite>("Sprites/ItemIcons/DefaultImage");
    }

    // ��� => �κ��丮 ��Ŭ�� ��ȣ�ۿ� �� ȣ��
    public void UseInInventory(SlotData slotData, int multieUseCount = 1)
    {
        // ��� ������
        if (this is IEquipable equipable)
        {
            //equipable.EquipToQuickSlot();
            // �̹� �κ��丮���� ó�� ��
        }
        // �Һ� ������ (�Һ� �������ε� ���� ������ �ֵ��� ���� ���������� ����)
        else if (this is Item_Consumable consumable)
        {
            // �Һ��� ���� �� ȿ�� ����
            ConsumableSorting(consumable);

            // ���� �����Ϳ��� ���� ���̱�
            consumable.Consume(slotData, multieUseCount);
            PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();
        }
        // ������ �������̶��
        else if (this is Item_Recipe recipe)
        {
            recipe.UnlockThisRecipe();
            // ���� ������ ����ؼ� ����ؼ� �Һ�����ٰŸ� SlotData.C
            slotData.CleanSlotData();
            PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();
        }
    }

    // ��ɽ��� => �÷��̾ ������ ���¿��� ������ �� ȣ��
    public void ActivateEffectOnAttack(SlotData slotData) // ���⼭ slotData�� ���� Ȱ��ȭ �Ǿ��ִ� �������� ���� ���� �����͸� �����Ѵ�
    {
        // ��� ������
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
        // �Һ� ������ (�Һ� �������ε� ���� ������ �ֵ��� ���� ���������� ����)
        else if (this is Item_Consumable consumable)
        {
            // �Һ��� ���� �� ȿ�� ����
            ConsumableSorting(consumable);

            // ���� �����Ϳ��� ���� ���̱�
            consumable.Consume(slotData);

            // ������ & �κ��丮 UI ������Ʈ
            PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();

        }
    }

    // �Һ��� ���� �� �з��ؼ� �� ������ �´� ȿ�� ����
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


    // ������ �ν��Ͻ� ����
    public void SpawnItem(Transform transform, int count = 1)
    {
        GameObject instanceObj = Instantiate(instancePrefab, transform.position, transform.rotation);
        Rigidbody rb = instanceObj.GetComponent<Rigidbody>();

        // ���� Ŀ������ �� �ְ� �ٸ����� ���� ������ ������ �޾ƿ��� ������ ����
        if (rb != null) rb.AddForce(Vector3.up * 3, ForceMode.Impulse);

        ItemInstance instance = instanceObj.GetComponent<ItemInstance>();
        instance.InitInstance(this, count);
    }
}

public enum ItemType
{
    Equipment, // ���
    Consumalbe, // �Ҹ�ǰ
    Ingredient, // ���
    Function, // ���
    Quest, //����Ʈ
    ETC, // ��Ÿ
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

