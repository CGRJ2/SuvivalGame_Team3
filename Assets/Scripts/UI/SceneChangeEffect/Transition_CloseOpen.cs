using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_CloseOpen : TransitionSceneEffect
{

    [Header("화면 닫을 상하좌우 이미지 설정")]
    [SerializeField] RectTransform imageLeft;
    [SerializeField] RectTransform imageRight;
    [SerializeField] RectTransform imageUp;
    [SerializeField] RectTransform imageDown;
    [SerializeField] float closeOpenTime;
    [SerializeField] float waitTime;
    [SerializeField] TwinCloseOpenMode mode;



    Vector2 SidePosCenter = new Vector2(0, 0);
    Vector2 SidePosBottom;
    Vector2 SidePosUp;
    Vector2 SidePosLeft;   // 좌측 열림 위치
    Vector2 SidePosRight;

    private void Start()
    {
        RectTransform canvasRect = transform.root.GetComponent<RectTransform>();

        SidePosBottom = new Vector2(0, -canvasRect.rect.height / 2);
        SidePosLeft = new Vector2(-canvasRect.rect.width / 2, 0);
        SidePosUp = -SidePosBottom;
        SidePosRight = -SidePosLeft;
    }


    public override IEnumerator TransitionPattern(string sceneName)
    {
        // 1. 초기 세팅
        //*************************************//
        RectTransform rect_A;
        RectTransform rect_B;

        Vector2 rectA_Temp = Vector2.zero;
        Vector2 rectB_Temp = Vector2.zero;

        if (mode == TwinCloseOpenMode.Vertical)
        {
            rect_A = imageUp;
            rect_A.anchoredPosition = SidePosUp;

            rect_B = imageDown;
            rect_B.anchoredPosition = SidePosBottom;

        }
        else if (mode == TwinCloseOpenMode.Horizontal)
        {
            rect_A = imageLeft;
            rect_A.anchoredPosition = SidePosLeft;
            
            rect_B = imageRight;
            rect_B.anchoredPosition = SidePosRight;
        }
        else
        {
            rect_A = null;
            rect_B = null;
            Debug.LogError("모드 설정 에러!");
        }
        rectA_Temp = rect_A.anchoredPosition;
        rectB_Temp = rect_B.anchoredPosition;
        //*************************************//

        // 혹시 이전 tween이 남아있다면 정리
        rect_A.DOKill();
        rect_B.DOKill();

        yield return null; // 한 프레임 넘기고 애니메이션 실행 (UI 적용 안정화)

        // 2. 닫기 애니메이션
        Sequence closeSeq = DOTween.Sequence();
        closeSeq.Append(rect_A.DOAnchorPos(SidePosCenter, closeOpenTime).SetEase(Ease.InOutQuad));
        closeSeq.Join(rect_B.DOAnchorPos(SidePosCenter, closeOpenTime).SetEase(Ease.InOutQuad));
        yield return closeSeq.WaitForCompletion();  // 반드시 기다려야 함

        // 3. 로딩 텍스트 (원하면 이 위치에서 처리)
        loadingText.text = sceneName;
        loadingText.gameObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        loadingText.gameObject.SetActive(false);

        // 사운드 코루틴 시작

        // 4. 비동기 씬 로딩
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitUntil(() => oper.isDone);  // 로딩 끝날 때까지 대기

        // 5. 열기 애니메이션
        imageLeft.DOKill(); imageRight.DOKill();
        yield return null; // 한 프레임 대기
        Sequence openSeq = DOTween.Sequence();
        openSeq.Append(rect_A.DOAnchorPos(rectA_Temp, closeOpenTime).SetEase(Ease.InOutQuad));
        openSeq.Join(rect_B.DOAnchorPos(rectB_Temp, closeOpenTime).SetEase(Ease.InOutQuad));
        yield return openSeq.WaitForCompletion();
    }
}

enum TwinCloseOpenMode { Vertical, Horizontal }