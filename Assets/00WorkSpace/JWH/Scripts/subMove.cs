using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
       
        float horizontal = Input.GetAxisRaw("Horizontal"); // A, D
        float vertical = Input.GetAxisRaw("Vertical");     // W, S

        
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

       
        if (direction.magnitude >= 0.1f)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
