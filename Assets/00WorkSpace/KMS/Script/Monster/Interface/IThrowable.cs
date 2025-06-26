using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
    void ApplyThrow(Vector3 direction, float force);
}
