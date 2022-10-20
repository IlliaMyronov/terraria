
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class block : MonoBehaviour
{

    // Use this for initialization
    private SpriteRenderer rend;
    private Sprite sprite;
    public float velocity = 0.0f;
    private Rigidbody2D rb;
    private float acceleration;
    [SerializeField]
    static private float jumpPower;
    [SerializeField]
    static private float gravity;
    private float jump;
    private bool isJumping;
    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    public void setSprite(Sprite newSprite)
    {

        sprite = newSprite;
        isJumping = true;

    }

    public void move(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    private void Update()
    {



    }


}