using UnityEngine;

public class InteractableTest_Get : MonoBehaviour, IInteractable
{
    [SerializeField] Item item;
    public void Interact()
    {
        Debug.Log($"{gameObject.name} �� �ֿ���.");
        item.SpawnItem(transform);
    }

    public void SetInteractableEnable()
    {
        Debug.Log($"�ݱ�(E) : {gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        ColliderController CC = other.GetComponent<ColliderController>();

        if (CC != null)
        {
            CC.InteractableObj = this;
            SetInteractableEnable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ColliderController CC = other.GetComponent<ColliderController>();

        if (CC != null)
        {
            if (CC.InteractableObj == this as IInteractable)
            {
                CC.InteractableObj = null;
            }
            else return;
        }
    }
}
