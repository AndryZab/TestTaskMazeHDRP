using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private EnemyChase enemyChase;
    public Transform player;
    /// <summary>
    /// Update AI behavior each frame.
    /// </summary>

    private void Start()
    {
       enemyMovement = GetComponent<EnemyMovement>();
       enemyChase = GetComponent<EnemyChase>();
        
    }
    void Update()
    {
       enemyMovement.InitializeMethods();
       enemyChase.InitializeMethods();
    }

    

   
    /// <summary>
    /// Visualize detection range and rays in the editor.
    /// </summary>
    void OnDrawGizmos()
    {
        // Visualize the detection range sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 25f);

        if (player != null)
        {
            // Visualize the ray origin point
            Gizmos.color = Color.blue;
            Vector3 rayOrigin = transform.position + Vector3.up * 1.9f; // Offset point up
            Gizmos.DrawSphere(rayOrigin, 0.2f); // Small sphere at the ray origin

            // Calculate direction to the player
            Vector3 directionToPlayer = (player.position + Vector3.up * 1.9f - rayOrigin).normalized;

            // Field of view parameters
            float fieldOfViewAngle = 50f; // Enemy's field of view angle
            int numberOfRays = 5; // Number of rays for checking

            // Check each ray within the field of view
            for (int i = 0; i < numberOfRays; i++)
            {
                // Calculate angle for each ray
                float angleOffset = (i - (numberOfRays / 2)) * (fieldOfViewAngle / (numberOfRays - 1));
                Vector3 rayDirection = Quaternion.Euler(0, angleOffset, 0) * directionToPlayer;

                // Visualize each ray
                Gizmos.color = Color.red;
                Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * 25f); // Line for each ray
            }
        }
    }


}
