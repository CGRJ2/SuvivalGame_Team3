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
            Debug.Log($"[Test] {name} �÷��̾� Ÿ�� ���� �Ϸ�");
        }

        //if (monster.data != null)
        //{
        //    monster.SetData(monster.data);
        //}
        //else
        //{
        //    Debug.LogWarning($"[Test] {name}�� data�� �������� �ʾҽ��ϴ�.");
        //}
    }
}
