using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMS.Monster.Interface
{
    public interface IKnockbackable
    {
        void ApplyKnockback(Vector3 direction, float force);
    }
}
