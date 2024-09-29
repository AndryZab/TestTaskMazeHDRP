using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Navigation")]
    public Transform player;
    public Vector3[] patrolPoints;
    public float speedAI;
    public float speedAIDefault;
    public float distanceToAttackPlayer;
    private NavMeshAgent agent;
    private Animator animator;
    private int currentPointIndex = 0;

    [Header("IEnumerato Delay Atack")]
    public float delayAtack = 0f;
    private bool CoroutineIsRun = false;
    private bool ReadyToSwitchWalk = true;
    public bool RoaringMonster = false;

    private PlayerGUI playerGUI;
    private EnemyAudio enemyAudio;
    private PlayerDeath playerDeath;
    private EnemyChase enemyChase;


    void Start()
    {
        enemyChase = GetComponent<EnemyChase>();
        enemyAudio = GetComponent<EnemyAudio>();
        playerDeath = FindObjectOfType<PlayerDeath>();
        playerGUI = FindObjectOfType<PlayerGUI>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyAudio.StartPatrolAudio();

    }

    public void InitializeMethods()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Adjust the volume of the monster's steps based on distance to the player
        enemyAudio.AdjustStepVolume(distanceToPlayer);

        
        UpdateAnimation(); // Update the monster's animation
    }
  



    /// <summary>
    /// Rotate the enemy towards the target.
    /// </summary>
    public void RotateTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed / 10);
        }
    }

    /// <summary>
    /// Update animation based on state and distance to the player.
    /// </summary>
    void UpdateAnimation()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Trigger attack if within range while chasing
        if (distanceToPlayer < distanceToAttackPlayer && enemyChase.isChasing)
        {
            animator.SetTrigger("Attack");
            enemyChase.isChasing = false;
            if (playerDeath.Death == true)
            {
                enemyChase.PlayerIsDeath = true;
            }
            enemyAudio.AtackPlayerSound();
            if (!CoroutineIsRun)
            {
                StartCoroutine(delayForAtack());
            }
        }
        // Set run animation if chasing
        else if (enemyChase.isChasing)
        {
            RoaringMonster = false;
            animator.SetTrigger("Run");
        }
        // Set angry animation if roaring
        else if (RoaringMonster)
        {
            animator.SetTrigger("Angry");
        }
        // Set walk animation if ready
        else if (ReadyToSwitchWalk)
        {
            animator.SetTrigger("Walk");
        }
        
    }


    private IEnumerator delayForAtack()
    {
        // Disable walking and set speed to zero
        ReadyToSwitchWalk = false;
        CoroutineIsRun = true;
        agent.speed = 0f;
        yield return new WaitForSeconds(delayAtack);
        // Restore walking ability and speed
        ReadyToSwitchWalk = true;
        agent.speed = speedAIDefault;
        enemyAudio.StartPatrolAudio();

    }



    public void Patrol()
    {

        if (agent.remainingDistance < 1.5f)
        {
            MoveToNextPoint();
            
        }
        RotateTowards(agent.steeringTarget);
    }

    /// <summary>
    /// Move to the next patrol point.
    /// </summary>
    public void MoveToNextPoint()
    {
        // Play monster steps sound if not already playing

        // Return if there are no patrol points
        if (patrolPoints.Length == 0) return;

        // If at the last checkpoint, start a delay
        if (currentPointIndex == patrolPoints.Length - 1)
        {
            StartCoroutine(RoarDelayAtLastPoint());
            return;
        }
        enemyAudio.soundRoar = true;
        // Set destination to the current patrol point
        agent.SetDestination(patrolPoints[currentPointIndex]);
        // Move to the next patrol point index
        currentPointIndex = (currentPointIndex + 1);
       

    }

    private IEnumerator RoarDelayAtLastPoint()
    {
        // Set destination to the current patrol point
        agent.SetDestination(patrolPoints[currentPointIndex]);
        agent.speed = 0f;
        RoaringMonster = true;
        yield return new WaitForSeconds(2.3f);
        agent.speed = speedAIDefault; 
        RoaringMonster = false;
        // Move to the next patrol point after the delay
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;

    }
}
