using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    public Button craftButton;
    public Image buttonImage;
    public Color activeColor = Color.red;       // Ȱ��ȭ�� ������
    public Color inactiveColor = Color.gray;    // ��Ȱ��ȭ�� ��ο��

    // ��� üũ(�ӽ�)
    public bool hasMaterials = false;

    // �ӽ� üũ ��
    private void Update()
    {
        if (hasMaterials)
        {
            craftButton.interactable = true;
            buttonImage.color = activeColor;
        }
        else
        {
            craftButton.interactable = false;
            buttonImage.color = activeColor;
        }
    }

    // �����δ� public void CheckCraftable(){} �Լ� ���� ��뿹��
}
