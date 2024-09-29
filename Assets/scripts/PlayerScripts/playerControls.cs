using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;




public class PlayerControls : MonoBehaviour
{

    private Collider playerCollider;
    private Rigidbody rb;

    private PlayerAudio playerAudio;
    private PlayerMovementController playerMovementController;
    private PlayerCameraSettings cameraControl;
    private PlayerDeath playerDeath;
    private PlayerGUI playerGUI;




    /// <summary>
    /// Initialize components and settings at the start.
    /// </summary>
    void Start()
    {
        playerCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();


        playerAudio = GetComponent<PlayerAudio>();
        playerMovementController = GetComponent<PlayerMovementController>();
        cameraControl = GetComponent<PlayerCameraSettings>();
        playerDeath = GetComponent<PlayerDeath>();
        playerGUI = GetComponent<PlayerGUI>();
        playerAudio.StopAllSounds();
    }

   
  

    /// <summary>
    /// Updates player state, checks for death, and manages heartbeat and jumping.
    /// </summary>
    void Update()
    {
        if (playerDeath.Death && playerCollider.enabled == false)
        {
            playerAudio.StopAllSounds();

        }
        playerAudio.PlayerHeartBeat(playerDeath.Death, playerCollider);
        CheckPanelsStateAndJump();

    }

    /// <summary>
    /// Applies player movement and triggers camera bobbing if grounded.
    /// </summary>
    void FixedUpdate()
    {
        playerMovementController.MovePlayer();

        if (playerMovementController.isGrounded && !playerDeath.Death && playerCollider.enabled == true)
        {
            cameraControl.CameraBobbing();
        }
    }



    /// <summary>
    /// Checks GUI panel states and manages jumping and landing effects.
    /// </summary>
    private void CheckPanelsStateAndJump()
    {
        playerGUI.Panels();
        if (!playerGUI.anyPanelActive && Time.timeScale == 1)
        {
            playerMovementController.Jump(playerAudio); // Handle jump
        }

        playerMovementController.LandingCheck();

        if (playerMovementController.isGrounded && !playerMovementController.isLanding)
        {
            playerMovementController.isLanding = true;
            StartCoroutine(cameraControl.LandingEffect()); // Trigger landing effect
        }

        

        if (playerGUI.anyPanelActive && playerMovementController.isGrounded)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
            playerCollider.enabled = false;
        }
    }

   

  

}
