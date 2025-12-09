using System.Collections.Generic;
using UnityEngine;


public class Panel_PlayerStatus : MonoBehaviour
{
    public Dictionary<BodyPartType, Panel_PartState> dic_PartStatePanels = new();

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
        var panels = GetComponentsInChildren<Panel_PartState>();
        foreach(var panel in panels)
        {
            dic_PartStatePanels.Add(panel.partType, panel);
            panel.SetColors(defaultColor, damagedColor, deactiveColor);
        }
    }
}
