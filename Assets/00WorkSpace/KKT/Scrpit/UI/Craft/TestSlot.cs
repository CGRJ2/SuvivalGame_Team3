using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlot : MonoBehaviour
{
    public string itemName;
    public string itemType;
    public Sprite itemSprite;
    public string makeTime;
    public string itemDesc;
    public List<CraftDetail.MatData> matDatas;

    // 상세창 참조 (씬에서 Drag&Drop)
    public CraftDetail craftDetail;

    // 슬롯이 클릭될 때 호출
    //public void OnSlotClick()
    //{
    //    craftDetail.UpdateDetail(itemName, itemType, itemSprite, makeTime, itemDesc, matDatas);
    //}

    // 예시
    public void OnClickSlot()
    {
        // 예시 데이터
        string exampleName = "바느질 세트";
        string exampleType = "회복아이템";
        Sprite exampleSprite = Resources.Load<Sprite>("Images/ItemPhoto"); // 경로/이름에 맞게!
        string exampleTime = "7s";
        string exampleDesc = "블랙아웃 상태의 팔과 다리를 수술할 수 있는 아이템이다.";

        var matDatas = new System.Collections.Generic.List<CraftDetail.MatData>
        {
            new CraftDetail.MatData("솜조각", 10, 10),
            new CraftDetail.MatData("먼지", 500, 5),
            new CraftDetail.MatData("플라스틱 파편", 500, 3)
        };

        craftDetail.UpdateDetail(
            exampleName, exampleType, exampleSprite, exampleTime, exampleDesc, matDatas
        );
    }
}
