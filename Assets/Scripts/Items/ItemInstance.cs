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
        // 임시 ==> 오브젝트풀 패턴으로 수정 필요
        Destroy(gameObject);
    }

    public override void Interact()
    {
        base.Interact();

        // 플레이어 인벤토리로 들어감
        pc.Status.inventory.AddItem(item, count);

        // 인터랙터블 상태 해제
        pc.Cc.InteractableObj = null;

        // 임시 파괴 (오브젝트 풀 패턴으로 대체 필요)
        Destroy(gameObject);
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        Debug.Log($"{gameObject.name} : 아이템 습득(E) 팝업 UI 활성화");
    }
}
