using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartDurabilityUI : MonoBehaviour
{
    public Image rightHandImage;
    public Image leftHandImage;
    public Image rightLegImage;
    public Image leftLegImage;

    public int rightHandDurability = 100;
    public int leftHandDurability = 100;
    public int rightLegDurability = 100;
    public int leftLegDurability = 100;

    private Color brokenColor = Color.black;

    private Color rightHandNormalColor;
    private Color leftHandNormalColor;
    private Color rightLegNormalColor;
    private Color leftLegNormalColor;

    void Start()
    {
        rightHandNormalColor = rightHandImage.color;
        leftHandNormalColor = leftHandImage.color;
        rightLegNormalColor = rightLegImage.color;
        leftLegNormalColor = leftLegImage.color;
    }

    private void Update()
    {
        rightHandImage.color = (rightHandDurability <= 0) ? brokenColor : rightHandNormalColor;
        leftHandImage.color = (leftHandDurability <= 0) ? brokenColor : leftHandNormalColor;
        rightLegImage.color = (rightLegDurability <= 0) ? brokenColor : rightLegNormalColor;
        leftLegImage.color = (leftLegDurability <= 0) ? brokenColor : leftLegNormalColor;
    }
}
