using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryCampInstance : CampInstance
{
    public GameObject fx_DestroyCamp;

    // 데이터에서 삭제 후 객체 파괴
    public void DestroyWithData()
    {
        Debug.Log("간이 캠프 파괴 --- 코루틴으로 서서히 파괴 예정");

        // 데이터 초기화
        BaseCampManager.Instance.tempCampData = new TempCampData();
        
        // 인스턴스 참조 삭제
        if (BaseCampManager.Instance.currentTempCampInstance == this)
            BaseCampManager.Instance.currentTempCampInstance = null;


        // 객체 파괴 실행
        DestroyOnlyInstacne();
    }

    // 객체 파괴(인게임 로드 & 새로운 임시텐트 설치 시 호출)
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