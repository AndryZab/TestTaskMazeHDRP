using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private bool AtackPlayer = false;
    public bool soundRoar = false;
    public AudioClip MonsterRoarClip;
    public AudioClip MonsterAtack;
    public AudioSource MonsterSteps;
    public AudioSource MonsterRun;
    private Audiomanager AudiomanagerScript;
    private PlayerDeath playerDeath;

    private void Awake()
    {
        AudiomanagerScript = FindObjectOfType<Audiomanager>();
        playerDeath = FindObjectOfType<PlayerDeath>();
        AudiomanagerScript.musicsourceFollowMonster.Stop();
    }
    private void Update()
    {

        if (playerDeath.Death)
        {
            AudiomanagerScript.musicsourceFollowMonster.Stop();
        }

    }

    public void AtackPlayerSound()
    {
        if (AtackPlayer)
        {
            AudiomanagerScript.PlaySFX(MonsterAtack);
            AtackPlayer = false;
        }
    }
 

    public void AudioChasingStart()
    {
        AudiomanagerScript.musicsourcedefault.Stop();
        MonsterSteps.Stop();

        Debug.Log("PlayChasing");

        if (soundRoar)
        {
            // Play roar sound
            AudiomanagerScript.PlaySFX(MonsterRoarClip);
            soundRoar = false;
            AtackPlayer = true;
        }
        if (!AudiomanagerScript.musicsourceFollowMonster.isPlaying)
        {
            // Play follow monster music
            AudiomanagerScript.musicsourceFollowMonster.Play();
            Debug.Log("playFollow");
        }

        if (!MonsterRun.isPlaying)
        {
            // Play monster running sound
            MonsterRun.Play();
        }

    }
    public void StartPatrolAudio()
    {
        MonsterRun.Stop();
        AudiomanagerScript.musicsourceFollowMonster.Stop();
        if (!AudiomanagerScript.musicsourcedefault.isPlaying)
        {
            // Play default music when chasing stops
            AudiomanagerScript.musicsourcedefault.Play();
        }
        if (!MonsterSteps.isPlaying)
        {
            // Play monster steps sound
            MonsterSteps.Play();
        }
    }
    public void AdjustStepVolume(float distanceToPlayer)
    {
        // Maximum distance at which sound is barely audible
        float maxDistance = 30f;

        // Minimum distance at which sound is maximum
        float minDistance = 17.0f;

        // Linearly interpolate volume based on distance
        MonsterSteps.volume = Mathf.Clamp(1 - (distanceToPlayer - minDistance) / (maxDistance - minDistance), 0f, 1f);
        MonsterRun.volume = Mathf.Clamp(1 - (distanceToPlayer - minDistance) / (maxDistance - minDistance), 0f, 1f);
    }
}
