using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Panel_PartState : MonoBehaviour
{
    public float initMaxHp;

    [Header("���� HUD �̹��� ü��")]
    public Image imageInHUD;

    [Header("������ �̹���")]
    public Image image;

    [Header("������ ����ü��(TMP)")]
    public TMP_Text TMP_CurrentHp;
    public Image currentHP_Slider;

    [Header("������ �ִ�ü��(TMP)")]
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
        // �� ���߱�
        if (hp > 0)
        {
            // ���� �� ��ȭ
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

        // �����̴� ��ġ ����
        currentHP_Slider.fillAmount = Mathf.Clamp01(hp / initMaxHp);

        // ���� ����
        TMP_CurrentHp.text = hp.ToString();
    }

    public void UpdateCurrentMaxHP_View(float currentMaxHp)
    {
        // �����̴� ��ġ ����
        maxHP_Slider.fillAmount = Mathf.Clamp01((float)currentMaxHp / (float)initMaxHp);

        // ���� ����
        TMP_MaxHp.text = currentMaxHp.ToString();
    }
}
