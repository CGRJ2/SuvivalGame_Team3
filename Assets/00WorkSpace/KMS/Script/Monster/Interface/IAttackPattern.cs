using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackPattern
{
    int Damage { get; }
    float Cooldown { get; }
}
