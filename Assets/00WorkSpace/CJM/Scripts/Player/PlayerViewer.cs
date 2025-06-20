using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewer : MonoBehaviour
{
    //[SerializeField] private Transform aim;
    [SerializeField] private Transform avatar;
    [SerializeField] private Transform cameraFocusTransform;

    Rigidbody rb;
    Vector2 currentRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Vector3 Move(Vector3 dir, float moveSpeed)
    {
        Vector3 moveDirection = dir;

        Vector3 velocity = rb.velocity;
        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;

        rb.velocity = velocity;
        return moveDirection;
    }

    public void SetAvatarRotation(Vector3 dir, float rotateSpeed)
    {
        if (dir == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        avatar.rotation = Quaternion.Lerp(avatar.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public Vector3 SetAimRotation(Vector3 mouseDir, float minPitch, float maxPitch)
    {
        currentRotation.x += mouseDir.x;
        currentRotation.y = Mathf.Clamp(currentRotation.y + mouseDir.y, minPitch, maxPitch);

        cameraFocusTransform.rotation = Quaternion.Euler(0, currentRotation.x, 0);
        //transform.rotation = Quaternion.Euler(0, currentRotation.x, 0);

        //Vector3 currentEuler = aim.localEulerAngles;
        //aim.localEulerAngles = new Vector3(currentRotation.y, currentEuler.y, currentEuler.z);

        Vector3 rotateDirVector = cameraFocusTransform.forward;
        rotateDirVector.y = 0;
        return rotateDirVector.normalized;
    }

    
}
