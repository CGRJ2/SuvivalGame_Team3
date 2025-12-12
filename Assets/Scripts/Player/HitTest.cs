using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HitTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var hitBox = GetComponentInChildren<Hitbox>();
        hitBox.Init(transform);
        hitBox.Configure(10f, 1f, 1f, 1f);
        hitBox.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            var hitBox = GetComponentInChildren<Hitbox>();
            hitBox.SetActive(true);
        }
    }
}
