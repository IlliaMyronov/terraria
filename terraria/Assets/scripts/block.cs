
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class block : MonoBehaviour
{

    // Use this for initialization
    private SpriteRenderer rend;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rend = this.GetComponent<SpriteRenderer>();
    }
    public void setSprite(Sprite newSprite)
    {

        rend.sprite = newSprite;

    }

}