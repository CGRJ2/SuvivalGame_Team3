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
        Debug.Log("패널 열기");
        UIManager.Instance.popUpUIGroup.interactableUI.gameObject.SetActive(true);
    }

    public virtual void CloseInteractableUI()
    {
        Debug.Log("패널 닫기");
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
            // 플레이어가 들어온걸 확인 하면 활성화
            isActive = true;

            // 플레이어 상호작용 Obj에 추가
            pc = other.GetComponent<PlayerController>();
            pc.Cc.InteractableObj = this;
            ShowInteractableUI();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            // 플레이어가 들어와 있으면
            if (isActive)
            {
                pc = other.GetComponent<PlayerController>();

                // 플레이어 상호작용 Obj에 이미 추가되어 있는 상태라면 return
                if (pc.Cc.InteractableObj == this as IInteractable) return;

                // 플레이어 상호작용 Obj에 빈자리가 생기면 추가
                else if (pc.Cc.InteractableObj == null)
                {
                    pc.Cc.InteractableObj = this;
                    ShowInteractableUI();
                }
            }
            // 중간에 오브젝트가 파괴 or 플레이어가 사라진다면
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
            // 플레이어가 영역 밖으로 나갔다면 비활성화
            isActive = false;

            // 만약 플레이어 상호작용 Obj에 자신이 할당되어있다면 할당 해제 후 상호작용UI 끄기
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
