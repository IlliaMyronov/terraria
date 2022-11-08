
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class block : MonoBehaviour
{

    // Use this for initialization
    private SpriteRenderer rend;
    private Sprite mySprite;
    public float velocity = 0.0f;
    private Rigidbody2D rb;
    private float acceleration;
    [SerializeField]
    static private float jumpPower;
    [SerializeField]
    static private float gravity;
    private float jump;
    private bool isJumping;
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rend = this.GetComponent<SpriteRenderer>();
    }
    public void setSprite(Sprite newSprite)
    {

        rend.sprite = newSprite;

    }

    public void move(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    private void Update()
    {



    }


}