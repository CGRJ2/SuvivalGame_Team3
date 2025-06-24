using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawShpereGizmos : MonoBehaviour
{
    public float size;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
