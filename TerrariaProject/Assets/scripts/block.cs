using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour {

	// Use this for initialization
	private SpriteRenderer rend;
	private Sprite sprite;
	public float speed = 0;
	private Rigidbody2D rb;

	private void Start() 
	{
		rb = this.GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(-speed, 0);
	}
	public void setSprite(Sprite newSprite) 
	{

		sprite = newSprite;
	
	}

}
