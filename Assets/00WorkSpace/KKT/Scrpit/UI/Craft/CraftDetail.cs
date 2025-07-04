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

    [Header("Panel")]
    public GameObject craftInteractPanel;
    public GameObject starforcePanel;


    private bool canCraft = false;
    private bool recipeNeed = false;
    // ��� ������
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
    // ��ư ��ȯ
    public void SetItemName(string name)
    {
        if (name == "� ������ ��ǰ")
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
    // ������ ������ Ŭ���� ���� ǥ��
    public void UpdateDetail(string _itemName, string _itemType, Sprite _itemSprite, string _makeTime, string _desc, bool _recipeNeed, List<MatData> matDatas)
    {
        itemName.text = _itemName;
        itemType.text = _itemType;
        itemImage.sprite = _itemSprite;
        makeTime.text = _makeTime;
        itemDesc.text = _desc;
        recipeNeed=_recipeNeed;
        SetItemName(_itemName);
        ShowMaterial(matDatas);
        Debug.Log("recipe = " + _recipeNeed);
    }
    // ���� ��ư Ȱ��ȭ/��Ȱ��ȭ
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

        Debug.Log("���� ����? " + canCraft);

        craftButton.interactable = canCraft;

        var buttonImage=craftButton.GetComponent<Image>();

        // ���� ����
        if (buttonImage != null) 
        {
            buttonImage.color = canCraft ? Color.red : Color.gray;
        }

        Debug.Log("craftButton.interactable = " + craftButton.interactable);
    }
    // ���� ��� ������Ʈ
    public void ShowMaterial(List<MatData> matDatas)
    {
        // ���� ���� ����
        foreach (Transform child in materialListParent)
        {
            Destroy(child.gameObject);
        }
        // ����
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
            Debug.Log("��ᰡ �����ؼ� ��ǰ���� �Ұ�!");
            return;
        }
        SelectPartPanel.SetActive(true);
        UIEscape.Instance.OpenPanel(SelectPartPanel);
        Debug.Log("����");
    }
    public void CloseSelectPart()
    {
        SelectPartPanel.SetActive(false);
        UIEscape.Instance.ClosePanel(SelectPartPanel);
    }
    // �ʱ�ȭ
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
    public void OnClickCraftButton()
    {
        craftInteractPanel.SetActive(false);
        starforcePanel.SetActive(false);

        if (recipeNeed)
        {
            craftInteractPanel.SetActive(true);
        }
        else 
        {
            starforcePanel.SetActive(true);
        }
    }
    public void Update()
    {
        if (SelectPartPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSelectPart();
        }
    }
}