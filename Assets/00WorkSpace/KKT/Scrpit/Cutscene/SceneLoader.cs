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
    //    videoPlayer.SetDirectAudioVolume(0, 0.1f); // ���� ����(0.0~1.0)
    //    videoPlayer.Play();
    //    videoPlayer.loopPointReached += OnVideoEnd; // ���� ������ ȣ��
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

    // ����ȭ�� �̵�
    public void GoToMainScene()
    {
        SceneManager.LoadScene("KKT_MainSceneTest");
    }

    // �ΰ��� �̵�
    public void GoToGameScene()
    {
        SceneManager.LoadScene("KKT_GameSceneTest");
    }

    // Ư�� �ε����� ������ �̵�
    //public void LoadScene(int sceneIndex)
    //{
    //    SceneManager.LoadScene(sceneIndex);
    //}
}
