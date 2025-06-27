using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    protected PlayerController pc;

    public virtual void Interact()
    {
        Debug.Log($"{gameObject.name} : ��ȣ�ۿ� ����");
    }

    public virtual void SetInteractableEnable()
    {
        Debug.Log($"{gameObject.name} : ��ȣ�ۿ� ���� ����");
    }

    private void OnTriggerEnter(Collider other)
    {
        pc = other.GetComponent<PlayerController>();

        if (pc != null)
        {
            pc.Cc.InteractableObj = this;
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
