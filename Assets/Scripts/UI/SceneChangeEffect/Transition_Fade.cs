using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition_Fade : TransitionSceneEffect
{
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeTime;

    public override IEnumerator TransitionPattern(string sceneName)
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            Color color = fadeImage.color;
            color.a = Mathf.Lerp(0, 1, timer / fadeTime);
            fadeImage.color = color;

            timer += Time.deltaTime;
            yield return null;
        }

        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        // 여기서 커스텀 이름으로 변경
        loadingText.text = $"{sceneName}";
        loadingText.gameObject.SetActive(true);
        yield return null;
        Thread.Sleep(1000);
        loadingText.gameObject.SetActive(false);

        timer = 0;
        while (timer < fadeTime)
        {
            Color color = fadeImage.color;
            color.a = Mathf.Lerp(1, 0, timer / fadeTime);
            fadeImage.color = color;

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
