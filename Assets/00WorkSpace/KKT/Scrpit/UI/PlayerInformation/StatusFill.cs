using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusFill : MonoBehaviour
{
    [Header("PlayerInformation")]
    public Image hpBarFill;
    public Image staminaBarFill;
    public Image mentalBarFill;
    public int maxHP = 100;
    public int curHP = 100;
    public int maxStamina = 100;
    public int curStamina = 100;
    public int maxMental = 100;
    public int curMental = 100;

    [Header("InventoryStatus")]
    public Image headHPBarFill;
    public Image rightHandHPBarFill;
    public Image leftHandHPBarFill;
    public Image rightLegHPBarFill;
    public Image leftLegHPBarFill;
    public int maxHeadHP = 100;
    public int maxRightHandHP = 100;
    public int maxLeftHandHP = 100;
    public int maxRightLegHP = 100;
    public int maxLeftLegHP = 100;
    public int curHeadHP = 100;
    public int curRightHandHP = 100;
    public int curLeftHandHP = 100;
    public int curRightLegHP = 100;
    public int curLeftLegHP = 100;

    private void Update()
    {
        hpBarFill.fillAmount = (float)curHP / maxHP;
        staminaBarFill.fillAmount = (float)curStamina / maxStamina;
        mentalBarFill.fillAmount = (float)curMental / maxMental;

        headHPBarFill.fillAmount = (float)curHeadHP / maxHeadHP;
        rightHandHPBarFill.fillAmount = (float)maxRightHandHP / maxRightHandHP;
        leftHandHPBarFill.fillAmount = (float)curLeftHandHP / maxLeftHandHP;
        rightLegHPBarFill.fillAmount = (float)curRightLegHP / maxRightLegHP;
        leftLegHPBarFill.fillAmount = (float)curLeftLegHP / maxLeftLegHP;
    }
}
