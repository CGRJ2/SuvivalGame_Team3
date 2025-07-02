using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusFill : MonoBehaviour
{
    [Header("PlayerInformation")]
    public Image hpBarFill;
    public Image staminaBarFill;
    public Image willBarFill;
    

    [Header("InventoryStatus")]
    public Image headHPBarFill;
    public Image rightArmHPBarFill;
    public Image leftArmHPBarFill;
    public Image rightLegHPBarFill;
    public Image leftLegHPBarFill;

    private PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = PlayerManager.Instance.instancePlayer.Status;
        if(playerStatus == null)
        {
            Debug.Log("playerStatus 연결이 필요");
        }

        playerStatus.CurrentWillPower.Subscribe(UpdateWillPowerBar);
        playerStatus.CurrentBattery.Subscribe(UpdateStaminaBar);
    }

    void Update()
    {
        if(playerStatus==null) return;

        staminaBarFill.fillAmount = (float)playerStatus.CurrentBattery.Value / 100f;
        willBarFill.fillAmount = (float)playerStatus.CurrentWillPower.Value / 100f;

        //var head = playerStatus.GetPart(BodyPartTypes.Head).Hp.Subscribe;
        // var head = playerStatus.GetPart(BodyPartTypes.Head);
        var rightArm = playerStatus.GetPart(BodyPartTypes.RightArm);
        var leftArm = playerStatus.GetPart(BodyPartTypes.LeftArm);
        var rightLeg = playerStatus.GetPart(BodyPartTypes.RightLeg);
        var leftLeg = playerStatus.GetPart(BodyPartTypes.LeftLeg);

        // SubscribeBodyPart(playerStatus.GetPart(BodyPartTypes.Head), headHPBarFill);
    }

    void UpdateWillPowerBar(int value)
    {
        if (willBarFill != null)
        {
            willBarFill.fillAmount = value / 100f;
        }
    }

    void UpdateStaminaBar(int value)
    {
        if (staminaBarFill != null)
        {
            staminaBarFill.fillAmount = value / 100f;
        }
    }

    //void SubscribeBodyPart(BodyPart part, Image bar)
    //{
    //    if (part == null || bar == null) return;

    //    part.Hp.Subscribe((hp) =>
    //    {
    //        bar.fillAmount = (part.MaxHp == 0) ? 0f : hp / (float)part.MaxHp;
    //    });
    //}
}
