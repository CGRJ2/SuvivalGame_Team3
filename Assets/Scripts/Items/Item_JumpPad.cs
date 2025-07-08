using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpPadItem", menuName = "New Item/��ô ������/�����е�")]
public class Item_JumpPad : Item_Throwing
{
    // �ʿ��� ��� �߰� �Ӽ� ����
    [SerializeField] private float jumpForce = 15f; // ���� �� (�� ���� ����)

    // ���� OnAttackEffect�� �������̵�
    public override void OnAttackEffect()
    {
        // Ư���� ȿ���� �ʿ��ϸ� ���⿡ ����
        Debug.Log("���� �����е尡 ��ġ�Ǿ����ϴ�!");
    }
}