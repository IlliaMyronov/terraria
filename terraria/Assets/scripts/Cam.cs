using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.velocity = new Vector2(5, 0);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            rb.velocity = new Vector2(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.velocity = new Vector2(-5, 0);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity = new Vector2(0, 5);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
}
