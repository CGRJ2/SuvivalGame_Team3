using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class LoadingSceneTest :MonoBehaviour
{
    [SerializeField] TMP_Text loadingText;
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeTime;

    [SerializeField] RawImage videoImage;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] string inGameSceneName;

    protected Coroutine loadingRoutine;

    [SerializeField] float skipEscHoldTime;
    float holdTime = 0;
    bool isChanging = false;

    private void Awake() => Init();


    private void Start()
    {
        videoPlayer.loopPointReached += LoadInGameScenceAfterVideoEnd;
        //StartLoading(inGameSceneName); //테스트용
    }

    private void OnDestroy()
    {
        if (loadingRoutine != null) StopCoroutine(loadingRoutine);
    }
    private void Update()
    {
        if (isChanging || !videoPlayer.isPlaying) return;
        if (Input.GetKey(KeyCode.Escape))
        {
            holdTime += Time.deltaTime;
            if (holdTime > skipEscHoldTime)
            {
                SkipVideo();
            }
        }
    }
    void Init()
    {
    }

    public void GameStart()
    {
        Debug.Log("선택");
        StartCoroutine(FadeOutAndVideoPlayer());
    }

    public void SkipVideo()
    {
        if (videoPlayer.isPlaying)
        {
            StartLoading(inGameSceneName);
        }
    }

    public void LoadInGameScenceAfterVideoEnd(VideoPlayer vp)
    {
        Debug.Log("비디오 종료됨");
        StartLoading(inGameSceneName);
    }


    public void StartLoading(string sceneName)
    {
        isChanging = true;

        if (loadingRoutine == null)
        {
            loadingRoutine = StartCoroutine(FadeAndSceneChange(sceneName));
        }
        else
        {
            StopCoroutine(loadingRoutine);
            loadingRoutine = StartCoroutine(FadeAndSceneChange(sceneName));
        }
    }


    public IEnumerator FadeAndSceneChange(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        videoImage.gameObject.SetActive(true);
        float timer = 0;
        while (timer < fadeTime)
        {
            Color color = videoImage.color;
            color.a = Mathf.Lerp(1, 0, timer / fadeTime);
            videoImage.color = color;

            timer += Time.deltaTime;
            yield return null;
        }

        videoImage.gameObject.SetActive(false);

        // 현재 씬 이름을 저장 (씬 객체는 언로드 후 무효화될 수 있음)
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 새 씬 로드 (Additive)
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = false;

        // 페이드 인
        timer = 0;
        while (timer < fadeTime)
        {
            Color color = fadeImage.color;
            color.a = Mathf.Lerp(1, 0, timer / fadeTime);
            fadeImage.color = color;

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitUntil(() => loadOp.progress >= 0.9f);
        loadOp.allowSceneActivation = true;
    }

    public IEnumerator FadeOutAndVideoPlayer()
    {
        fadeImage.gameObject.SetActive(true);
        float timer = 0;
        while (timer < fadeTime)
        {
            Color color = fadeImage.color;
            color.a = Mathf.Lerp(0, 1, timer / fadeTime);
            fadeImage.color = color;

            timer += Time.deltaTime;
            yield return null;
        }

        videoImage.gameObject.SetActive(true);
        yield return null;
        videoPlayer.Play();

        timer = 0;
        while (timer < fadeTime)
        {
            Color color = videoImage.color;
            color.a = Mathf.Lerp(0, 1, timer / fadeTime);
            videoImage.color = color;

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
