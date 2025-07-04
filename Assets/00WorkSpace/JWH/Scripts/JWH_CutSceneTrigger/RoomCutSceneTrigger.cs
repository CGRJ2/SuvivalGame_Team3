using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCutsceneTrigger : MonoBehaviour
{
    [SerializeField] private int cutsceneType = 0; // ����� �ƾ� ��ȣ
    private bool hasPlayed = false;

    [SerializeField] private TestCutPlay TestCutPlay;//�׽�Ʈ��

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

             TestCutPlay.PlayCutscene(cutsceneType);//�׽�Ʈ��
             hasPlayed = true;
            
        }
    }
}