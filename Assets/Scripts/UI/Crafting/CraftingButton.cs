using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public float craftingTime;
    float pressedTime;
    [HideInInspector] public Button btnSelf;
    [SerializeField] Slider progressBar;

    Coroutine pressedRoutine;

    private void Awake()
    {
        btnSelf = GetComponent<Button>();
    }

    IEnumerator CraftingPressedRoutine()
    {
        pressedTime = 0;
        float finalCraftingTime = 0;

        // ķ�� ����3 ���� ����
        if (BaseCampManager.Instance.baseCampData.CurrentCampLevel.Value > 2)
            finalCraftingTime = craftingTime * 0.5f;
        // ķ�� ����2 ����
        else if (BaseCampManager.Instance.baseCampData.CurrentCampLevel.Value > 1)
            finalCraftingTime = craftingTime * 0.75f;

        while (pressedTime < finalCraftingTime)
            {
                if (!btnSelf.interactable) yield break;

                pressedTime += Time.deltaTime;
                progressBar.value = Mathf.Clamp01(pressedTime / finalCraftingTime);
                yield return null;
            }


        // ���������� ������ ��
        UIManager.Instance.craftingGroup.StartCrafting();
        pressedTime = 0;
        progressBar.value = 0;
        pressedRoutine = null;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (btnSelf.interactable && pressedRoutine == null)
            pressedRoutine = StartCoroutine(CraftingPressedRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pressedRoutine != null)
        {
            StopCoroutine(pressedRoutine);
            pressedRoutine = null;
        }
        pressedTime = 0;
        progressBar.value = 0;
    }
}
