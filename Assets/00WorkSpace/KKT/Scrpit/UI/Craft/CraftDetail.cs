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
    public Image itemImage;
    public TextMeshProUGUI makeTime;
    public TextMeshProUGUI itemDesc;

    [Header("MaterialDetail")]
    public Transform materialListParent;
    public GameObject materialSlotPrefab;

    [Header("SelecPart")]
    public GameObject SelectPartPanel;

    [Header("Button")]
    public Button craftButton;
    public Button selectPartButton;

    private bool canCraft = false;
    // 재료 데이터
    public class MatData
    {
        public string name;
        public int have;
        public int need;

        public MatData(string name, int have, int need)
        {
            this.name = name;
            this.have = have;
            this.need = need;
        }
    }
    // 버튼 변환
    public void SetItemType(string type)
    {
        if (type == "회복아이템")
        {
            craftButton.gameObject.SetActive(false);
            selectPartButton.gameObject.SetActive(true);
        }
        else
        {
            craftButton.gameObject.SetActive(true);
            selectPartButton.gameObject.SetActive(false);
        }

        Debug.Log("craftButton.interactable = " + craftButton.interactable);
    }
    public void UpdateDetail(string _itemName, string _itemType, Sprite _itemSprite, string _makeTime, string _desc, List<MatData> matDatas)
    {
        itemName.text = _itemName;
        itemType.text = _itemType;
        itemImage.sprite = _itemSprite;
        makeTime.text = _makeTime;
        itemDesc.text = _desc;
        SetItemType(_itemType);
        ShowMaterial(matDatas);
    }
    public void UpdateCraftButton(List<MatData> matDatas)
    {
        canCraft = true;
        foreach (var mat in matDatas)
        {
            if (mat.have < mat.need)
            {
                canCraft = false;
            }
        }

        Debug.Log("제작 가능? " + canCraft);

        craftButton.interactable = canCraft;

        var buttonImage=craftButton.GetComponent<Image>();

        // 색상 변경
        if (buttonImage != null) 
        {
            buttonImage.color = canCraft ? Color.red : Color.gray;
        }

        Debug.Log("craftButton.interactable = " + craftButton.interactable);
    }
    
    public void ShowMaterial(List<MatData> matDatas)
    {
        
        // 기존 슬롯 삭제
        foreach (Transform child in materialListParent)
        {
            Destroy(child.gameObject);
        }

        // 생성
        foreach (var data in matDatas)
        {
            var slot = Instantiate(materialSlotPrefab, materialListParent);
            var name = slot.transform.Find("MaterialSlot/MatName").GetComponent<TextMeshProUGUI>();
            var count = slot.transform.Find("MaterialSlot/MatCount").GetComponent<TextMeshProUGUI>();
            name.text = data.name;
            count.text = $"{data.have}/{data.need}";
            count.color = (data.have < data.need) ? Color.red : Color.black;
        }
        Debug.Log("craftButton.interactable = " + craftButton.interactable);

        UpdateCraftButton(matDatas);
    }
    public void ShowSelectPart()
    {
        if (!canCraft)
        {
            Debug.Log("재료가 부족해서 부품선택 불가!");
            return;
        }
        SelectPartPanel.SetActive(true);
        UIEscape.Instance.OpenPanel(SelectPartPanel);
        Debug.Log("스택");
    }
    public void CloseSelectPart()
    {
        SelectPartPanel.SetActive(false);
        UIEscape.Instance.ClosePanel(SelectPartPanel);
    }
    // 초기화
    public void ClearDetail()
    {
        itemName.text = "";
        itemType.text = "";
        itemImage.sprite = null;
        makeTime.text = "";
        itemDesc.text = "";

        foreach (Transform child in materialListParent)
            Destroy(child.gameObject);
    }
    public void Update()
    {
        if (SelectPartPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSelectPart();
        }
    }

}