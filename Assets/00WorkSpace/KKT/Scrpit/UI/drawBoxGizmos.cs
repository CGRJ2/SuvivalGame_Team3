using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class drawBoxGizmos : MonoBehaviour
{
    void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return;

        Gizmos.color = Color.red;

        // ���� ��ǥ�� ��ȯ (Transform ����)
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(box.center, box.size);

        Gizmos.matrix = oldMatrix;
    }
}
