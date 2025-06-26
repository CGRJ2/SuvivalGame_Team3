using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public VideoPlayer videoPlayer;

    //private void Start()
    //{
    //    videoPlayer.SetDirectAudioVolume(0, 0.1f); // 볼륨 조절(0.0~1.0)
    //    videoPlayer.Play();
    //    videoPlayer.loopPointReached += OnVideoEnd; // 영상 끝나면 호출
    //}

    public void Update()
    {
        if (SceneManager.GetActiveScene().name=="KKT_MainSceneTest")
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                GoToGameScene();
            }
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

    // 특정 인덱스의 씬으로 이동
    //public void LoadScene(int sceneIndex)
    //{
    //    SceneManager.LoadScene(sceneIndex);
    //}
}
