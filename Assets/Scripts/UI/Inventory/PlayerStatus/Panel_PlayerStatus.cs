using UnityEngine;


public class Panel_PlayerStatus : MonoBehaviour
{
    [Header("파츠 정보 필드")]
    public Panel_PartState head;
    public Panel_PartState leftArm;
    public Panel_PartState leftLeg;
    public Panel_PartState rightArm;
    public Panel_PartState rightLeg;

    [Header("생존 수치 필드")]
    public Panel_SuvivalState state_HpSum;
    public Panel_SuvivalState state_Battery;
    public Panel_SuvivalState state_WillPower;

    [Header("색상 설정")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color damagedColor;
    [SerializeField] private Color deactiveColor;


    public void Init()
    {
        head.SetColors(defaultColor, damagedColor, deactiveColor);
        leftArm.SetColors(defaultColor, damagedColor, deactiveColor);
        leftLeg.SetColors(defaultColor, damagedColor, deactiveColor);
        rightArm.SetColors(defaultColor, damagedColor, deactiveColor);
        rightLeg.SetColors(defaultColor, damagedColor, deactiveColor);
    }
}
