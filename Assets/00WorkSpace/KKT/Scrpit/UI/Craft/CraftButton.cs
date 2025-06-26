using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    public Button craftButton;
    public Image buttonImage;
    public Color activeColor = Color.red;       // 활성화시 빨간색
    public Color inactiveColor = Color.gray;    // 비활성화시 어두운색

    // 재료 체크(임시)
    public bool hasMaterials = false;

    // 임시 체크 용
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

    // 실제로는 public void CheckCraftable(){} 함수 만들어서 사용예정
}
