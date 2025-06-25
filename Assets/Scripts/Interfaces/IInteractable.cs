using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    // �÷��̾�� ȣ��
    public void Interact();


    // Interactable ������Ʈ ������ �ݶ��̴��� �÷��̾ �ν� �� ����.
    public void SetInteractableEnable();

    // �÷��̾��� ��ȣ�ۿ� �����ȿ� ������ UI �˾�â ���� (ex �ݱ�:E, ��ġ�ϱ�:E)
    // �÷��̾�CC�� List<IInteractable> interactablesInRange�� �ش� ������Ʈ �־��ֱ�

}
