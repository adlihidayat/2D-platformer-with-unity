using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;

    public float speed;
    private float Move;
    private bool isRunning;
    private bool isFacingRight = true;
    public float Jump;
    public bool isJumping;
    public bool isGrounded;
    public float groundCheckRadius;
    public Transform groundCheck;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isWalking = false;  // To track if the walk sound is playing

    public LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }



    void Update()
    {
        Move = Input.GetAxisRaw("Horizontal");

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

        CheckMovementDirection();
        UpdateAnimations();
        CheckSurroundings();
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }


    private void CheckMovementDirection()
    {
        if (isFacingRight && Move < 0)
        {
            Flip();
        }
        else if (!isFacingRight && Move > 0)
        {
            Flip();
        }
        if (rb.velocity.x != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isRunning",  isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
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

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
