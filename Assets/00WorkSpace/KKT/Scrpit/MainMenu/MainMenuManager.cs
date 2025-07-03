using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button continueButton;
    public Button newStartButton;
    public Button exitButton;

    public GameObject player;
    public GameObject npc;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (SceneManager.GetActiveScene().name == "KKT_MainSceneTest" && !EventSystem.current.IsPointerOverGameObject())
            {
                SceneManager.LoadScene("KKT_GameSceneTest");
            }
        }
    }
    public void OnClickContinue()
    {
        Debug.Log("이어하기 실행");
        SceneManager.LoadScene("KKT_PayloadSceneTest");
    }
    public void OnClickNewStart()
    {
        Debug.Log("새로하기 실행");
        SceneManager.LoadScene("EmptyTestScene");
    }
    public void OnClickExit()
    {
        Debug.Log("게임 종료!");
        Application.Quit();
    
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
    public void OnClickTest()
    {
        SceneManager.LoadScene("KKT_MainSceneTest");
    }

    public void OnClickTest2()
    {
        SceneManager.LoadScene("KKT_MainSceneTest");
    }
}
