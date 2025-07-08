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
    None,       // 기본값 (아직 진입 안함)
    NewGame,    // 새로하기
    Continue,   // 이어하기
}
