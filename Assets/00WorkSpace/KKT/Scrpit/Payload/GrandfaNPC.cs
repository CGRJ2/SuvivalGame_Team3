using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrandfaNPC : MonoBehaviour
{
    public Payload payload;
    public GameObject player;
    public GameObject npc;
    public GameObject talkPanel;
    public TextMeshProUGUI talkText;
    public Button saveButton;

    public float playerDetectRadius;
    public bool isSaved;
    private bool isTalking;

    private void Start()
    {
        isSaved = GameState.isSave;
    }
    private void Update()
    {
        // 플레이어가 인식 범위 안에 있는지
        float dist = Vector3.Distance(player.transform.position, npc.transform.position);
        bool isPlayerNear = dist <= playerDetectRadius;

        if (!isPlayerNear)
        {
            isTalking = false;
            talkPanel.SetActive(false);
            return; 
        }

        if (isTalking) return;

        if (isPlayerNear&&Input.GetKeyDown(KeyCode.E))
        {
            if (!isSaved)
            {
                StartCoroutine(ShowAndHide(talkPanel, talkText, "기록을 하지 않았군아 \n 여행을 떠나기 전에 마지막 기록을 하자꾸나", 2f));
            }
            else
            {
                StartCoroutine(ShowAndHide(talkPanel, talkText, "기록을 마쳤다. \n 이제 마지막 여행을 떠나도록 하자.", 2f));
            }
        }
    }
    public void LastSave()
    {
        payload.UnlockPayload();
        Debug.Log("저장 완료! 비행기로 탈출 가능!");
    }
    private IEnumerator ShowAndHide(GameObject panel, TextMeshProUGUI text, string msg, float duration)
    {
        isTalking = true;
        text.text = msg;
        panel.SetActive(true);
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
        isTalking = false;
    }

    public void OnClickSave()
    {
        isSaved = true;

        GameState.lastPlayerPos = player.transform.position;
        Player playerScript = player.GetComponent<Player>();
        GameState.lastPlayerHP = playerScript.curHP;

        GrandfaNPC grandfaNPCScript = npc.GetComponent<GrandfaNPC>();
        GameState.isSave = grandfaNPCScript.isSaved;

        LastSave();
    }
}
