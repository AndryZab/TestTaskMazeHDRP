using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float runMultiplier = 1.3f;
    private Rigidbody rb;
    private Collider playerCollider;
    private Animator animator;

    public bool isGrounded;
    public bool isLanding = true;

    public float jumpForce = 15f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.3f;

    private PlayerDeath playerDeath;
    private PlayerAudio playerAudio;

    /// <summary>
    /// Moves the player based on input.
    /// </summary>
    private void Start()
    {
        playerCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerDeath = GetComponent<PlayerDeath>();
        playerAudio = GetComponent<PlayerAudio>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);
    }

    public void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (playerCollider.enabled == false)
        {
            horizontal = 0f;
            vertical = 0f;
        }
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection.Normalize();

        if (horizontal != 0 || vertical != 0)
        {
            if (isGrounded && !playerDeath.Death)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    moveDirection *= runMultiplier;
                    animator.SetTrigger("RunPlayer"); // Running animation
                    playerAudio.PlayRunSound();
                }
                else
                {
                    animator.SetTrigger("WalkPlayer"); // Walking animation
                    playerAudio.PlayStepsSound();
                }
            }
        }

        ApplyAnimationStates(horizontal, vertical); // Call method to update animation states

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime); // Apply movement
    }

    /// <summary>
    /// Updates animation states depending on the player's current movement.
    /// </summary>
    private void ApplyAnimationStates(float horizontal, float vertical)
    {
        if (!isGrounded && !playerDeath.Death)
        {
            animator.SetTrigger("JumpPlayer"); // Jump animation
            playerAudio.StopMovementSounds();
        }
        else if (playerDeath.Death)
        {
            animator.SetTrigger("DeathPlayer"); // Death animation
        }
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("DeathPlayer") && horizontal == 0 && vertical == 0)
        {
            playerAudio.StopMovementSounds();
            animator.SetTrigger("IdlePlayer"); // Idle animation
        }
    }

    public void LandingCheck()
    {
        if (!isGrounded)
        {
            isLanding = false;
        }
    }

    /// <summary>
    /// Handle jumping mechanics.
    /// </summary>
    public void Jump(PlayerAudio playerAudio)
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            playerAudio.PlayJumpSound();
        }
    }
}
