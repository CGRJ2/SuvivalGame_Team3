using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAITestInitializer : MonoBehaviour
{
    [SerializeField] private BaseMonster monster;

    private void Start()
    {
        PlayerController pc = PlayerManager.Instance.instancePlayer;
        if (pc != null)
        {
            monster.SetTarget(pc.transform);
            Debug.Log($"[Test] {name} 플레이어 타겟 지정 완료");
        }

        //if (monster.data != null)
        //{
        //    monster.SetData(monster.data);
        //}
        //else
        //{
        //    Debug.LogWarning($"[Test] {name}의 data가 설정되지 않았습니다.");
        //}
    }
}
