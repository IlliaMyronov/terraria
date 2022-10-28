using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{

    private Rigidbody2D rb;
    private void Start()
    {

        rb = this.GetComponent<Rigidbody2D>();

    }
    public void move(Vector2 velocity)
    {

        rb.velocity = velocity;

    }
}
