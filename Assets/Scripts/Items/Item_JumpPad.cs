using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpPadItem", menuName = "New Item/투척 아이템/점프패드")]
public class Item_JumpPad : Item_Throwing
{
    // 플레이어 제 자리에 놓기
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