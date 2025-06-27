using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSave : MonoBehaviour
{
    public void TryNotSave()
    {
        if (GimmickManager.CanSaveNow())
        {
            Debug.Log("세이브! (실제로 저장되지 않음)");
        }
        else
        {
            Debug.LogWarning("낮잠 자면 다 죽어~");
        }
    }

    public void NotLoad()
    {
        Debug.Log("로드! (실제로 불러오지 않음)");
    }
}
