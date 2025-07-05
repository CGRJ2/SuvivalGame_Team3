using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerView;

public class JWH_sceneTrigger : MonoBehaviour//Ʈ���ſ� �ִ� �ڵ��Դϴ�
{
    [SerializeField] private int cutsceneType = 0; // ����� �ƾ� ��ȣ
    //[SerializeField] private CutsceneType cutsceneSource = CutsceneType.Room;//Ÿ�Ը� �����ؼ� ��밡��
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