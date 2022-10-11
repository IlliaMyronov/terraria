using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class block : MonoBehaviour {

	// Use this for initialization
	private SpriteRenderer rend;
	private Sprite sprite;
	public float velocity = 0.0f;
	private Rigidbody2D rb;
	private float acceleration;

	private void Start() 
	{
		rb = this.GetComponent<Rigidbody2D>();
	}
	public void setSprite(Sprite newSprite) 
	{

		sprite = newSprite;
	
	}

	public void notify(string message) 
	{

        if (message == "moveRight") 
		{
			velocity = 1.0f;
			this.move();
		}

        else if(message == "moveLeft")
        {
			velocity = -1.0f;
			this.move();
        }
		 
		else if (message == "stop")
        {
			velocity = 0.0f;
			this.move();
		}

	}

	private void move()
    {
		rb.velocity = new Vector2(velocity, 0);
	}
	
}
