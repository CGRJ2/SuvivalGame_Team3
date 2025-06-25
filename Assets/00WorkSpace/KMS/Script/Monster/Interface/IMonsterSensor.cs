using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IMonsterSensor
{
    bool IsTargetVisible(Transform self, Transform target, float detectionRange, float fov, float eyeHeight);
}