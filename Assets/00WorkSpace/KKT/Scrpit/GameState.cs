using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static Vector3 lastPlayerPos;
    public static int lastPlayerHP;
    public static bool isSave;

    public static EntryMode entryMode = EntryMode.None;
}

public enum EntryMode
{
    None,       // �⺻�� (���� ���� ����)
    NewGame,    // �����ϱ�
    Continue,   // �̾��ϱ�
}
