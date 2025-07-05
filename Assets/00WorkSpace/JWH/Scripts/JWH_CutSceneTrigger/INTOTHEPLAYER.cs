using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerView;



////아마도 플레이어 컨트롤러?
//public void StartCatCutscene(CatAI cat)
//{
//    int rand = Random.Range(0, 2);

//    PlayCutscene(rand, CutsceneType.Cat);
//    cat.PlayCutsceneAnim(rand);

//    // 추가 연출 작성
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
//                case 0: Debug.Log("RoomCutsceneA 트리거 실행"); break;
//                case 1: Debug.Log("RoomCutsceneB 트리거 실행"); break;

//            }
//            break;


//            //추가 가능
//    }
//}

//public enum CutsceneType
//{
//    Cat,
//    Room,
//    
//    // 추가가능
//}