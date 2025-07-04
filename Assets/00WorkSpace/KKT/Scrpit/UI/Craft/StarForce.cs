 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarForce : MonoBehaviour
{
    public GameObject starforcePanel;
    public RectTransform starImage;
    public RectTransform barBG;
    public RectTransform successZoneImage;
    public Button stopButton;
    public TextMeshProUGUI resultText;

    public float speed;
    private bool movingRight = true;
    private bool isPlaying = true;

    private float leftLimit, rightLimit;

    void OnEnable()
    {
        isPlaying = true;
        movingRight = true;
        resultText.gameObject.SetActive(false);
        starImage.anchoredPosition = new Vector2(0, starImage.anchoredPosition.y); // 원하는 위치로 초기화
    }
    private void Start()
    {
        resultText.gameObject.SetActive(false);

        // 별 속도 
        speed = 900f;
        // speed = Random.Range(800f,1200f) // 랜덤 속도

        // 최대 움직임 범위
        float halfBarWidth = barBG.rect.width * 0.5f;
        leftLimit = barBG.anchoredPosition.x - halfBarWidth + starImage.rect.width * 0.5f;
        rightLimit = barBG.anchoredPosition.x + halfBarWidth - starImage.rect.width * 0.5f;

        stopButton.onClick.AddListener(OnStopPressed);
    }
    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        // 별 이동
        float move = speed * Time.deltaTime * (movingRight ? 1 : -1);
        var pos = starImage.anchoredPosition;
        pos.x += move;

        if (pos.x > rightLimit)
        {
            pos.x = rightLimit;
            movingRight = false;
        }
        if (pos.x < leftLimit)
        {
            pos.x = leftLimit;
            movingRight = true;
        }

        starImage.anchoredPosition = pos;
    }
    void OnStopPressed()
    {
        if (!isPlaying)
        {
            return;
        }

        
        // 별이 성공 존인지 확인
        float starX = starImage.position.x;
        float zoneMin = successZoneImage.position.x - successZoneImage.rect.width * 0.5f;
        float zoneMax = successZoneImage.position.x + successZoneImage.rect.width * 0.5f;

        if (starX > zoneMin && starX < zoneMax)
        {
            resultText.text = "<color=yellow>성공!</color>";
        }
        else
        {
            resultText.text = "<color=red>실패!</color>";
        }

        resultText.gameObject.SetActive(true);

        isPlaying = false;

        StartCoroutine(Hide(1.5f));
    }
    // 결과창 끄기
    IEnumerator Hide(float delay)
    {
        yield return new WaitForSeconds(delay);
        starforcePanel.SetActive(false);
        resultText.gameObject.SetActive(false);
    }
}
