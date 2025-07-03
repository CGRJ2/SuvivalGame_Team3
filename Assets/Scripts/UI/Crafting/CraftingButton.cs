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

        while (pressedTime < craftingTime)
        {
            if (!btnSelf.interactable) yield break;

            pressedTime += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(pressedTime / craftingTime);
            yield return null;
        }


        // 정상적으로 끝났을 때
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
