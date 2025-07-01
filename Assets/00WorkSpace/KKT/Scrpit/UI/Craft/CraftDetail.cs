using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftDetail : MonoBehaviour
{
    [Header("ItemDetail")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemType;
    public Image ItemImage;
    public TextMeshProUGUI makeTime;
    public TextMeshProUGUI itemDesc;

    [Header("MaterialDetail")]
    public Transform materialListParent;
    public GameObject materialSlotPrefab;

    [Header("Button")]
    public Button craftButton;
    public Button selectPartButton;


    public class MatData
    {
        public string name;
        public int have;
        public int need;

        // 基敲
        public MatData(string name, int have, int need)
        {
            this.name = name;
            this.have = have;
            this.need = need;
        }
    }
    public void ShowMaterial(List<MatData> matDatas)
    {
        // 扁粮 浇吩 昏力
        foreach(Transform child in materialListParent)
        {
            Destroy(child.gameObject);
        }

        // 积己
        foreach(var data in matDatas)
        {
            var slot = Instantiate(materialSlotPrefab, materialListParent);
            var name= slot.transform.Find("MatName").GetComponent<TextMeshProUGUI>();
            var count = slot.transform.Find("MatCount").GetComponent<TextMeshProUGUI>();
            name.text = data.name;
            count.text = $"{data.have}/{data.need}";
            count.color = (data.have < data.need) ? Color.red : Color.black;
        }
    }
}

