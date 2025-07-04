using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCutsceneTrigger : MonoBehaviour
{
    [SerializeField] private int cutsceneType = 0; // 재생할 컷씬 번호
    private bool hasPlayed = false;

    [SerializeField] private TestCutPlay TestCutPlay;//테스트용

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            //var player = other.GetComponent<PlayerController>();
            //if (player != null && player.view != null)
            //{
            //    player.view.PlayCutscene(cutsceneType, CutsceneType.Room);
            //    hasPlayed = true;
            //}

             TestCutPlay.PlayCutscene(cutsceneType);//테스트용
             hasPlayed = true;
            
        }
    }
}