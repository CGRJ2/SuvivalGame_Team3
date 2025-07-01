using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    protected PlayerController pc;

    public virtual void Interact()
    {

    }

    public virtual void SetInteractableEnable()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            pc = other.GetComponent<PlayerController>();

            pc.Cc.InteractableObj = this;
            SetInteractableEnable();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            pc = other.GetComponent<PlayerController>();

            if (pc.Cc.InteractableObj == this as IInteractable) return;
            else if (pc.Cc.InteractableObj == null)
            {
                pc.Cc.InteractableObj = this;
                SetInteractableEnable();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            Debug.LogError(gameObject.name);
            pc.Cc.InteractableObj = null;
            pc = null;
        }
    }
}
