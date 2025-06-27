using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAppearGimmick : MonoBehaviour
{
    [SerializeField] private DailyState[] visibleStates;

    private Renderer objRenderer;
    private Collider objCollider;
    private bool isVisible;

    private void Awake()
    {
        objRenderer = GetComponent<Renderer>();
        objCollider = GetComponent<Collider>();
        isVisible = false;
    }

    private void Update()
    {
        UpdateVisible();
    }

    private void UpdateVisible()
    {
        var currentState = GimmickManager.Instance.CurrentState;
        bool DoVisible = System.Array.Exists(visibleStates, s => s == currentState);

        if (DoVisible != isVisible)
        {
            isVisible = DoVisible;

            if (isVisible)
                Debug.Log($"[{name}] 시간대 {currentState}오브젝트 dodo등장");
            else
                Debug.Log($"[{name}] 시간대 {currentState}오브젝트 dodo다운");
        }

        if (objRenderer != null)
            objRenderer.enabled = DoVisible;

        if (objCollider != null)
            objCollider.enabled = DoVisible;
    }
}
