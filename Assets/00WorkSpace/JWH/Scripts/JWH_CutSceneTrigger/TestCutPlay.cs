using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCutPlay : MonoBehaviour//테스트코드 지워도 문제 없음
{
    public void PlayCutscene(int cutsceneType)
    {
        Debug.Log($"컷씬 {cutsceneType} 재생 테스트됨");
    }
}
