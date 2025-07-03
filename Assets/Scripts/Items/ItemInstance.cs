using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInstance : InteractableBase
{
    public Item item;
    public int count;
    [SerializeField] float rigidDeactiveTime;
    [SerializeField] float destroyTime;

    public void InitInstance(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    private void Start()
    {
        StartCoroutine(AfterSpawnRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator AfterSpawnRoutine()
    {
        yield return new WaitForSeconds(rigidDeactiveTime);
        GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(destroyTime);
        // �ӽ� ==> ������ƮǮ �������� ���� �ʿ�
        Destroy(gameObject);
    }

    public override void Interact()
    {
        base.Interact();

        // �÷��̾� �κ��丮�� ��
        pc.Status.inventory.AddItem(item, count);

        // ���ͷ��ͺ� ���� ����
        pc.Cc.InteractableObj = null;

        // �ӽ� �ı� (������Ʈ Ǯ �������� ��ü �ʿ�)
        Destroy(gameObject);
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        Debug.Log($"{gameObject.name} : ������ ����(E) �˾� UI Ȱ��ȭ");
    }
}
