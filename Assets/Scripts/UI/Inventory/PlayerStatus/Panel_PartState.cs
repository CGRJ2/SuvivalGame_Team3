using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Panel_PartState : MonoBehaviour
{
    public float initMaxHp;

    [Header("부위 HUD 이미지 체인")]
    public Image imageInHUD;

    [Header("부위별 이미지")]
    public Image image;

    [Header("부위별 현재체력(TMP)")]
    public TMP_Text TMP_CurrentHp;
    public Image currentHP_Slider;

    [Header("부위별 최대체력(TMP)")]
    public TMP_Text TMP_MaxHp;
    public Image maxHP_Slider;

    private Color defaultColor = Color.white;
    private Color damagedColor;
    private Color deactiveColor;

    public void SetColors(Color defaultColor, Color damagedColor, Color deactiveColor)
    {
        this.defaultColor = defaultColor;
        this.damagedColor = damagedColor;
        this.deactiveColor = deactiveColor;
    }



    public void UpdateHP_View(float hp)
    {
        // 색 맞추기
        if (hp > 0)
        {
            // 부위 색 변화
            float colorKey = Mathf.Clamp(hp, 0, initMaxHp);
            float t = colorKey / initMaxHp;
            Color color = Color.Lerp(damagedColor, defaultColor, t);
            image.color = color;
            imageInHUD.color = color;
        }
        else
        {
            image.color = deactiveColor;
            imageInHUD.color = deactiveColor;
        }

        // 슬라이더 수치 적용
        currentHP_Slider.fillAmount = Mathf.Clamp01(hp / initMaxHp);

        // 숫자 적용
        TMP_CurrentHp.text = hp.ToString();
    }

    public void UpdateCurrentMaxHP_View(float currentMaxHp)
    {
        // 슬라이더 수치 적용
        maxHP_Slider.fillAmount = Mathf.Clamp01((float)currentMaxHp / (float)initMaxHp);

        // 숫자 적용
        TMP_MaxHp.text = currentMaxHp.ToString();
    }
}
