using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryCampInstance : CampInstance
{
    public GameObject fx_DestroyCamp;

    // �����Ϳ��� ���� �� ��ü �ı�
    public void DestroyWithData()
    {
        Debug.Log("���� ķ�� �ı� --- �ڷ�ƾ���� ������ �ı� ����");

        // ������ �ʱ�ȭ
        BaseCampManager.Instance.tempCampData = new TempCampData();
        
        // �ν��Ͻ� ���� ����
        if (BaseCampManager.Instance.currentTempCampInstance == this)
            BaseCampManager.Instance.currentTempCampInstance = null;


        // ��ü �ı� ����
        DestroyOnlyInstacne();
    }

    // ��ü �ı�(�ΰ��� �ε� & ���ο� �ӽ���Ʈ ��ġ �� ȣ��)
    public void DestroyOnlyInstacne()
    {
        GameObject explosion = Instantiate(fx_DestroyCamp);
        explosion.transform.SetParent(null);
        Destroy(gameObject);
    }
}

[System.Serializable]
public class TempCampData
{
    public bool isActive;
    public Vector3 tempCampPosition;
    public Quaternion tempCampRotation;

    public TempCampData() 
    {
        isActive = false;
        this.tempCampPosition = Vector3.zero;
        this.tempCampRotation = Quaternion.identity;
    }

    public TempCampData(Transform tempCampTransform)
    {
        isActive = true;
        this.tempCampPosition = tempCampTransform.position;
        this.tempCampRotation = tempCampTransform.rotation;
    }
}