using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    protected PlayerController pc;
    bool isActive;

    public virtual void Interact()
    {
        CloseInteractableUI();
    }

    public virtual void ShowInteractableUI()
    {
        Debug.Log("�г� ����");
        UIManager.Instance.popUpUIGroup.interactableUI.gameObject.SetActive(true);
    }

    public virtual void CloseInteractableUI()
    {
        Debug.Log("�г� �ݱ�");
        UIManager.Instance.popUpUIGroup.interactableUI.gameObject.SetActive(false);
    }

    public virtual void OnDisableActions()
    {
        isActive = false;
    }

    public void OnDisable()
    {
        OnDisableActions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            // �÷��̾ ���°� Ȯ�� �ϸ� Ȱ��ȭ
            isActive = true;

            // �÷��̾� ��ȣ�ۿ� Obj�� �߰�
            pc = other.GetComponent<PlayerController>();
            pc.Cc.InteractableObj = this;
            ShowInteractableUI();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            // �÷��̾ ���� ������
            if (isActive)
            {
                pc = other.GetComponent<PlayerController>();

                // �÷��̾� ��ȣ�ۿ� Obj�� �̹� �߰��Ǿ� �ִ� ���¶�� return
                if (pc.Cc.InteractableObj == this as IInteractable) return;

                // �÷��̾� ��ȣ�ۿ� Obj�� ���ڸ��� ����� �߰�
                else if (pc.Cc.InteractableObj == null)
                {
                    pc.Cc.InteractableObj = this;
                    ShowInteractableUI();
                }
            }
            // �߰��� ������Ʈ�� �ı� or �÷��̾ ������ٸ�
            else
            {
                CloseInteractableUI();
            }
            

            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            // �÷��̾ ���� ������ �����ٸ� ��Ȱ��ȭ
            isActive = false;

            // ���� �÷��̾� ��ȣ�ۿ� Obj�� �ڽ��� �Ҵ�Ǿ��ִٸ� �Ҵ� ���� �� ��ȣ�ۿ�UI ����
            if (pc != null)
            {
                if (pc.Cc.InteractableObj == this as IInteractable)
                {
                    pc.Cc.InteractableObj = null;
                    pc = null;
                    CloseInteractableUI();
                }
            }
        }
    }

    
}
