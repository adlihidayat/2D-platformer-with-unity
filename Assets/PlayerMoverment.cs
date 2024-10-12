using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;

    public float speed;
    private float Move;

    public float Jump;
    public bool isJumping;

    private Rigidbody2D rb;

    private bool isWalking = false;  // To track if the walk sound is playing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move = Input.GetAxis("Horizontal");

        // Move the player
        rb.velocity = new Vector2(speed * Move, rb.velocity.y);

        // Handle walking sound
        if (Move != 0 && !isWalking && !isJumping)  // If player is moving and not jumping, play the walking sound
        {
            audioManager.PlaySFX(audioManager.walk, true); // Play the walk sound once
            isWalking = true;
        }
        else if (Move == 0 && isWalking)  // Stop walking sound when player stops moving
        {
            audioManager.StopSFX();  // Stop the walking sound when player stops
            isWalking = false;
        }

        // Handle jump
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            audioManager.PlaySFX(audioManager.jump);  // Play jump sound
            rb.AddForce(new Vector2(rb.velocity.x, Jump));
            Debug.Log("jump");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
        }
    }
}
