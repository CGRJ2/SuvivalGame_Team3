using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class ClickScale : MonoBehaviour, IPointerClickHandler
{
    public GameObject mapPanel;

    public float scaleAmount = 1.2f; // 확대 배율
    public float duration = 0.15f; // 시간

    private Vector3 originalScale;
    private bool isScaleUp = false;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StopAllCoroutines();
        if (!isScaleUp)
        {
            StartCoroutine(ScaleTo(originalScale * scaleAmount, duration));
            mapPanel.transform.SetAsLastSibling();
        }
        else
        {
            StartCoroutine (ScaleTo(originalScale, duration));
        }
        isScaleUp = !isScaleUp;
    }

    System.Collections.IEnumerator ScaleTo(Vector3 target, float time)
    {
        Vector3 start = transform.localScale;
        float t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime / time;
            transform.localScale=Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.localScale = target;
    }

}
