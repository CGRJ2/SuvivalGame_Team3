using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneTest01 : MonoBehaviour
{
    public string nextSceneName;

    private bool cutsceneDone = false;
    private bool loadingDone=false;
    private AsyncOperation op;

    private void Start()
    {
        switch (GameState.entryMode)
        {
            case EntryMode.NewGame:
                Debug.Log("새로하기로 진입");
                // 초기화
                break;
            case EntryMode.Continue:
                Debug.Log("이어하기로 진입");
                // 세이브 데이터 불러오기
                break;
        }

        StartCoroutine(LoadNextSceneAsync());
        StartCoroutine(PlayCutscene());
        StartCoroutine(WaitAndGo());
    }

    IEnumerator LoadNextSceneAsync()
    {
        op=SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            yield return null;
        }
        loadingDone = true;
    }

    IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(1f);

        cutsceneDone = true;
    }

    IEnumerator WaitAndGo()
    {
        while(!cutsceneDone || !loadingDone)
        {
            yield return null;
        }
        op.allowSceneActivation=true;
    }
}
