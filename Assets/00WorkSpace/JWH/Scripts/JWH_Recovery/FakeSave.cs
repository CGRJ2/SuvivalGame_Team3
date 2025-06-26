using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSave : MonoBehaviour
{
    public void NotSave()
    {
        Debug.Log("세이브 완료! (실제로 저장되지 않음)");
    }

    public void NotLoad()
    {
        Debug.Log("로드 완료! (실제로 불러오지 않음)");
    }
}
