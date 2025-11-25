using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    private void Awake() => Init();

    private void Init()
    {
        Manager.camera.cinemachineBrain = GetComponent<CinemachineBrain>();
    }
}
