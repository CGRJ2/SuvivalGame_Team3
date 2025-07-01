using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UnlockStageRequireView : MonoBehaviour
{

    public Image image_StageImage;
    public Image lockedImage;
    public TMP_Text tmp_StageNameAndUnlockedCondition;

    public void UpdateView(StageData stageData, Color ActiveColor, Color DeactiveColor)
    {
        image_StageImage.sprite = stageData.stageImage;

        if (stageData.isUnlocked)
        {
            tmp_StageNameAndUnlockedCondition.color = ActiveColor;
            tmp_StageNameAndUnlockedCondition.text = $"[{stageData.stageName}] 해금";
            lockedImage.gameObject.SetActive(false);
        }
        else
        {
            tmp_StageNameAndUnlockedCondition.color = DeactiveColor;
            tmp_StageNameAndUnlockedCondition.text = $"[{stageData.stageName}] 해금 필요";
            lockedImage.gameObject.SetActive(true);
        }
    }
}
