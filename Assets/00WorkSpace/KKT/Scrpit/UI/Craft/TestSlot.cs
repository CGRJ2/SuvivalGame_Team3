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

    // ��â ���� (������ Drag&Drop)
    public CraftDetail craftDetail;

    // ������ Ŭ���� �� ȣ��
    //public void OnSlotClick()
    //{
    //    craftDetail.UpdateDetail(itemName, itemType, itemSprite, makeTime, itemDesc, matDatas);
    //}

    // ����
    public void OnClickSlot()
    {
        // ���� ������
        string exampleName = "�ٴ��� ��Ʈ";
        string exampleType = "ȸ��������";
        Sprite exampleSprite = Resources.Load<Sprite>("Images/ItemPhoto"); // ���/�̸��� �°�!
        string exampleTime = "7s";
        string exampleDesc = "���ƿ� ������ �Ȱ� �ٸ��� ������ �� �ִ� �������̴�.";

        var matDatas = new System.Collections.Generic.List<CraftDetail.MatData>
        {
            new CraftDetail.MatData("������", 10, 10),
            new CraftDetail.MatData("����", 500, 5),
            new CraftDetail.MatData("�ö�ƽ ����", 500, 3)
        };

        craftDetail.UpdateDetail(
            exampleName, exampleType, exampleSprite, exampleTime, exampleDesc, matDatas
        );
    }
}
