using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartUIUpdater : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public Image leftHandImage, rightHandImage, leftLegImage, rightLegImage;

    private Color brokenColor = Color.black;
    private Color activeColor = Color.white;

    private void Update()
    {
        if(playerStatus==null) return;

        UpdateBodyPartUI(BodyPartTypes.RightArm, rightHandImage);
        UpdateBodyPartUI(BodyPartTypes.LeftArm, leftHandImage);
        UpdateBodyPartUI(BodyPartTypes.RightLeg, rightLegImage);
        UpdateBodyPartUI(BodyPartTypes.LeftLeg, leftLegImage);
    }

    void UpdateBodyPartUI(BodyPartTypes type, Image img)
    {
        var part = playerStatus.GetPart(type);
        if (part == null || img == null) return;

        img.color=part.Activate.Value? brokenColor : activeColor;
    }

}
