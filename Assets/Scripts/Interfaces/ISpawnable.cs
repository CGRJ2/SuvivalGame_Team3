using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����, FO
public interface ISpawnable
{
    public Action DeactiveAction { get; set; }
    public Transform OriginTransform { get; set; }

}
