using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerView;

public class JWH_sceneTrigger : MonoBehaviour//트리거에 넣는 코드입니다
{
    [SerializeField] private int cutsceneType = 0; // 재생할 컷씬 번호
    //[SerializeField] private CutsceneType cutsceneSource = CutsceneType.Room;//타입만 변경해서 사용가능
    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null && player.View != null)
            {
                //player.PlayCutscene(cutsceneType, cutsceneSource);//
                hasPlayed = true;
            }
        }
    }
}