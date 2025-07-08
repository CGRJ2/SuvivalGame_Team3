using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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
                GameState.entryMode = EntryMode.Continue;
                SceneManager.LoadScene("LoadingScene");
            }
        }
    }
    public void OnClickNewStart()
    {
        GameState.entryMode = EntryMode.NewGame;
        SceneManager.LoadScene("LoadingScene");
    }
    public void OnClickContinue()
    {
        GameState.entryMode = EntryMode.Continue;
        SceneManager.LoadScene("LoadingScene");
    }
    public void OnClickExit()
    {
        Debug.Log("게임 종료!");
        Application.Quit();
    
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}
