using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
   [Header("Movement Settings")]
    private int xInput;
    private int yInput;
    public float moveSpeed = 7f;
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f; // Makes falling feel "heavy"
    public float fastFallMultiplier = 5f; // Extra boost when pressing 'S'

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        xInput = (int)Input.GetAxisRaw("Horizontal");
        yInput = (int)Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

        if (rb.velocity.y < 0)
        {
           
            float currentMultiplier = (yInput < 0) ? fastFallMultiplier : fallMultiplier;
            rb.velocity += Vector2.up * Physics2D.gravity.y * (currentMultiplier - 1) * Time.fixedDeltaTime;
        }
       
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // --- Ground Detection ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
