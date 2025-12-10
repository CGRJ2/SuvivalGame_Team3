using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    private void Awake() => Init();

    private void Init()
    {
        Manager.camera.TpsViewCamera = GetComponent<CinemachineVirtualCamera>();
    }
}
