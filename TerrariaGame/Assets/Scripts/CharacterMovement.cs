using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //horizontal takes values of movement direction
    private float horizontal;
    private bool isFacingRight;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpPower;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        isFacingRight = true;
    }
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        // jump if space is pressed and if possible
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            Debug.Log(IsGrounded());
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }

        // if space bar is released in the middle of the jump shorten the jump
        if (Input.GetKeyUp(KeyCode.Space) == true && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        }

        // set velocity using acceleration
        if (horizontal != 0)
        {
            if (rb.velocity.x != 0)
            {
                // if I am still allowed to accelerate or I am slowing down
                if ((Math.Abs(rb.velocity.x) < maxSpeed) || (rb.velocity.x * horizontal < 0))
                {
                    rb.velocity = new Vector2(rb.velocity.x + (acceleration * Time.deltaTime * horizontal), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(maxSpeed * horizontal, rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x + acceleration * Time.deltaTime * horizontal, rb.velocity.y);
                Debug.Log("acceleration is " + acceleration + " Time is " + Time.deltaTime + " horizontal is " + horizontal);
            }
        }

        Flip();
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0) || (!isFacingRight && horizontal > 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

}