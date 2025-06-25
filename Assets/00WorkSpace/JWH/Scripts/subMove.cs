using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public BaseCamp camp;

    void Update()
    {
       
        float horizontal = Input.GetAxisRaw("Horizontal"); // A, D
        float vertical = Input.GetAxisRaw("Vertical");     // W, S

        
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

       
        if (direction.magnitude >= 0.1f)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            camp.UpgradeCamp();
        }
    }
}
