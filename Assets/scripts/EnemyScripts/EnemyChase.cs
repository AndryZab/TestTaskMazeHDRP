using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    [Header("Navigation and Chasing")]
    public Transform player;
    public float detectionRange = 30f;
    public float chaseDuration = 3f;
    public float speedAImultiply;
    private NavMeshAgent agent;
    private Animator animator;
    public bool isChasing = false;
    private float chaseTimer;
    public bool PlayerIsDeath = false;

    private PlayerGUI playerGUI;
    private EnemyAudio enemyAudio;
    private EnemyMovement enemyMovement;

    private void Start()
    {
        playerGUI = FindObjectOfType<PlayerGUI>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAudio = GetComponent<EnemyAudio>();
    }

    public void InitializeMethods()
    {
        
        // If chasing the player and the player is alive
        if (isChasing && (PlayerIsDeath == false || playerGUI.anyPanelActive == false))
        {
            ChasePlayer(); // Chase the player
        }
        else
        {
            enemyMovement.Patrol();
            // Detect the player if they are alive and the relevant UI is not active
            
           
        }

        DetectPlayer();
       

    }
    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the detection range
        if (distanceToPlayer < detectionRange && CanSeePlayerWithFieldOfView())
        {
            if (PlayerIsDeath == false || playerGUI.anyPanelActive == false)
            {
                isChasing = true;

            }
           
            agent.speed = enemyMovement.speedAIDefault * speedAImultiply;

            // Reset chase timer every time the player is detected
            chaseTimer = chaseDuration;
        }
       
    }


    /// <summary>
    /// Chase the player and handle chase duration.
    /// </summary>
    void ChasePlayer()
    {
        enemyMovement.RoaringMonster = false;
        // Set the agent's destination to the player's position
        agent.SetDestination(player.position);
        // Rotate towards the player
        enemyMovement.RotateTowards(player.position);
        // Stop the default music



        // If the player is no longer visible, decrease the chase timer
        if (!CanSeePlayerWithFieldOfView())
        {
            chaseTimer -= Time.deltaTime;
        }
        else
        {
            // Reset the timer if the player is seen again
            chaseTimer = chaseDuration;
            enemyAudio.AudioChasingStart();

        }

        // Stop chasing when the timer expires
        if (chaseTimer <= 0)
        {
            enemyAudio.StartPatrolAudio();

            isChasing = false;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                // Reset to default speed
                agent.speed = enemyMovement.speedAIDefault;
            }
            // Return to patrolling
            enemyMovement.MoveToNextPoint();
        }
    }


    /// <summary>
    /// Check if the enemy can still see the player.
    /// </summary>
    bool CanSeePlayerWithFieldOfView()
    {
        float fieldOfViewAngle = 50f; // Enemy's field of view angle
        int numberOfRays = 5; // Number of rays for checking
        Vector3 rayOrigin = transform.position + Vector3.up * 1.9f; // Offset to enemy's center

        // Calculate direction to the player
        Vector3 directionToPlayer = (player.position + Vector3.up * 1.9f - rayOrigin).normalized;

        // Check each ray within the field of view
        for (int i = 0; i < numberOfRays; i++)
        {
            // Calculate angle for each ray
            float angleOffset = (i - (numberOfRays / 2)) * (fieldOfViewAngle / (numberOfRays - 1));
            Vector3 rayDirection = Quaternion.Euler(0, angleOffset, 0) * directionToPlayer;

            // Visualize rays in the editor for debugging

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    return true; // Enemy sees the player
                }
            }
        }

        return false; // Enemy does not see the player
    }
}
