using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rbody;

    Animator animator;

    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;

    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpSpeed = 18f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 15f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip mushJumpSound;

    GameSession gameSession;

    float gravityScaleAtStart = 5;

    
    bool isAlive = true;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        gravityScaleAtStart = rbody.gravityScale;
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        if (!isAlive) return;
        
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        //Check & Prevent double jump while in the air
        bool canJump = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if(value.isPressed && canJump)
        {
            rbody.velocity += new Vector2(0f, jumpSpeed);
        }
        
    }
    void OnFire(InputValue value)
    {
        if(!isAlive) return;

        if (value.isPressed)
        {
            AudioSource.PlayClipAtPoint(fireSound, Camera.main.transform.position);
            Instantiate(bullet,gun.position,transform.rotation);
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rbody.velocity.y);

        rbody.velocity = playerVelocity ;

        // Check player Horizontal speed and play/don't play running animation
        bool playerHasHorizontalSpeed = Mathf.Abs(rbody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rbody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        // Check player collides with the ladder and set gravity accordingly
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rbody.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(rbody.velocity.x, moveInput.y * climbSpeed);
        rbody.velocity = climbVelocity;
        rbody.gravityScale = 0f;

        // Check player vertical speed and run ClimbAnimation

        bool playerHasVerticalSpeed = Mathf.Abs(rbody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
    void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards")))
        {
            AudioSource.PlayClipAtPoint(dieSound, Camera.main.transform.position);
            isAlive = false;
            animator.SetTrigger("Dying");
            rbody.velocity = deathKick;
            // Call Gamesession script 
            gameSession.ProcessPlayerDeath();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bouncy")
        {
            AudioSource.PlayClipAtPoint(mushJumpSound, Camera.main.transform.position);
        }
    }
}
