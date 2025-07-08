using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpPadItem", menuName = "New Item/��ô ������/�����е�")]
public class Item_JumpPad : Item_Throwing
{
    // �÷��̾� �� �ڸ��� ����
    [SerializeField] GameObject installedJumpPad;
    [SerializeField] float installOffset;
    public void InstallPrefab()
    {
        PlayerController pc = PlayerManager.Instance.instancePlayer;

        Transform playerTransform = pc.transform;
        Vector3 forward = pc.View.avatar.forward;


        Instantiate(installedJumpPad, playerTransform.position + forward * installOffset, playerTransform.rotation);
    }

    public override void OnAttackEffect()
    {
        InstallPrefab();
    }
}