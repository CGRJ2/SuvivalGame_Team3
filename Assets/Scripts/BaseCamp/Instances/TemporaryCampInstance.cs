using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryCampInstance : CampInstance
{
    public GameObject fbx_DestroyCamp;

    void Start()
    {
        BaseCampManager.Instance.tempCampData = new TempCampData(respawnPoint);
        BaseCampManager.Instance.currentTempCampInstance = this;
        Debug.Log("간이 캠프 생성");
    }


    public void DestroyTempCamp()
    {
        // 데이터 삭제
        BaseCampManager.Instance.tempCampData = null;
        
        // 인스턴스 참조 삭제
        if (BaseCampManager.Instance.currentTempCampInstance == this)
            BaseCampManager.Instance.currentTempCampInstance = null;

        GameObject explosion = Instantiate(fbx_DestroyCamp);
        explosion.transform.SetParent(null);

        Destroy(gameObject);
    }

}

[System.Serializable]
public class TempCampData
{
    public Transform respawnPoint;

    public TempCampData(Transform respawnPoint)
    {
        this.respawnPoint = respawnPoint;
    }
}