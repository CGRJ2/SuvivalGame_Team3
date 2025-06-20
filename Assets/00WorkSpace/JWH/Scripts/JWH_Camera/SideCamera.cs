using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCamera : ICamera
{
    private Vector3 offset;

    public SideCamera(Vector3 offset)
    {
        this.offset = offset;
    }

    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    public void UpdateCamera(CameraControl controller)
    {
        Transform target = controller.GetTarget();
        if (target == null) return;

        Vector3 targetPos = target.position + offset;
        Camera.main.transform.position = targetPos;
        Camera.main.transform.rotation = Quaternion.Euler(0f, 90f, 0f);// ¥‹πÊ«‚
    }
}
