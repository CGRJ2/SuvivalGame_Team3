using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerView;



////�Ƹ��� �÷��̾� ��Ʈ�ѷ�?
//public void StartCatCutscene(CatAI cat)
//{
//    int rand = Random.Range(0, 2);

//    PlayCutscene(rand, CutsceneType.Cat);
//    cat.PlayCutsceneAnim(rand);

//    // �߰� ���� �ۼ�
//}

//public void PlayCutscene(int cutsceneType, CutsceneType source)
//{
//    Debug.Log($"PlayCutscene Type: {source}, Index: {cutsceneType}");
//    switch (source)
//    {
//        case CutsceneType.Cat:
//            switch (cutsceneType)
//            {
//                case 0: animator.SetTrigger("PlayerCutsceneA"); break;
//                case 1: animator.SetTrigger("PlayerCutsceneB"); break;
//            }
//            break;

//        case CutsceneType.Room:
//            switch (cutsceneType)
//            {
//                case 0: Debug.Log("RoomCutsceneA Ʈ���� ����"); break;
//                case 1: Debug.Log("RoomCutsceneB Ʈ���� ����"); break;

//            }
//            break;


//            //�߰� ����
//    }
//}

//public enum CutsceneType
//{
//    Cat,
//    Room,
//    
//    // �߰�����
//}