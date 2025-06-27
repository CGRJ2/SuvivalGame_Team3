using UnityEngine;

public class InteractableTest_Get : InteractableBase
{
    [SerializeField] Item item;
    public override void Interact()
    {
        Debug.Log($"{gameObject.name} �� �ֿ���.");
        item.SpawnItem(transform);
    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"�ݱ�(E) : {gameObject.name}");
    }

   
}
