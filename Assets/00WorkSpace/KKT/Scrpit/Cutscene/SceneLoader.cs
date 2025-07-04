using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public VideoPlayer videoPlayer;

    public void Update()
    {
        if (SceneManager.GetActiveScene().name=="KKT_MainSceneTest")
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
                GoToGameScene();
        }
    }

    // 메인화면 이동
    public void GoToMainScene()
    {
        SceneManager.LoadScene("KKT_MainSceneTest");
    }

    // 인게임 이동
    public void GoToGameScene()
    {
        SceneManager.LoadScene("KKT_GameSceneTest");
    }

    // 세이브 이동
    public void GoToPayloadScene()
    {
        SceneManager.LoadScene("KKT_PayloadSceneTest");
    }

    // 특정 인덱스의 씬으로 이동
    //public void LoadScene(int sceneIndex)
    //{
    //    SceneManager.LoadScene(sceneIndex);
    //}
}
