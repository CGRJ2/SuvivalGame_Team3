using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoldToCraft : MonoBehaviour
{
    public GameObject craftInteract;
    public Image progressBar;
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI descText;

    public float holdTime = 2f;
    private float holdTimer = 0f;
    private bool isCrafted = false;

    void OnEnable()
    {
        holdTimer = 0f;
        isCrafted = false;
        progressBar.fillAmount = 0f;
        descText.text = "길게 누르기";
    }
    private void Update()
    {
        if (isCrafted) return;

        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("E키 입력");
            holdTimer += Time.deltaTime;
            progressBar.fillAmount = Mathf.Clamp01(holdTimer / holdTime);

            if (holdTimer > holdTime)
            {
                OnCrafted();
            }
        }
        else
        {
            holdTimer = 0f;
            progressBar.fillAmount = 0f;
            Debug.Log("제작 시간 미달");
        }
    }

    void OnCrafted()
    {
        isCrafted=true;
        descText.text = "<color=yellow>제작 완료!</color>";
        StartCoroutine(Hide(1.5f));
    }

    IEnumerator Hide(float delay)
    {
        yield return new WaitForSeconds(delay);
        craftInteract.SetActive(false);
    }
}
