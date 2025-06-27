using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAITestInitializer : MonoBehaviour
{
    [SerializeField] private BaseMonster monster;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            monster.SetTarget(player.transform);
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
