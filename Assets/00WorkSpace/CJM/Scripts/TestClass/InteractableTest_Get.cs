using UnityEngine;

public class InteractableTest_Get : InteractableBase
{
    [SerializeField] Item item;
    public override void Interact()
    {
        Debug.Log($"{gameObject.name} 를 주웠다.");
        item.SpawnItem(transform);
    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"줍기(E) : {gameObject.name}");
    }

   
}
