using System.Collections;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource StepsPlayerSource;
    public AudioSource RunPlayerSource;
    public AudioSource heartbeatAudioSource;
    public AudioClip Jump;
    public Transform[] enemy;
    private bool isPlayingSteps = false;
    private bool isPlayingRun = false;
    private Audiomanager Audiomanager;

    /// <summary>
    /// Play the sound of footsteps.
    /// </summary>
    private void Start()
    {
        Audiomanager = FindObjectOfType<Audiomanager>();
    }

    public void PlayStepsSound()
    {
        if (!isPlayingSteps)
        {
            StepsPlayerSource.Play();
            RunPlayerSource.Stop();
            isPlayingSteps = true;
            isPlayingRun = false;
        }
    }

    /// <summary>
    /// Play the sound of running.
    /// </summary>
    public void PlayRunSound()
    {
        if (!isPlayingRun)
        {
            RunPlayerSource.Play();
            StepsPlayerSource.Stop();
            isPlayingRun = true;
            isPlayingSteps = false;
        }
    }

    /// <summary>
    /// Stop the sounds of footsteps and running.
    /// </summary>
    public void StopMovementSounds()
    {
        StepsPlayerSource.Stop();
        RunPlayerSource.Stop();
        isPlayingSteps = false;
        isPlayingRun = false;
    }

    /// <summary>
    /// Play the jump sound effect.
    /// </summary>
    public void PlayJumpSound()
    {
        Audiomanager.PlaySFX(Jump);
    }

    /// <summary>
    /// Adjusts heartbeat audio pitch based on distance to the nearest enemy and manages playback.
    /// </summary>
    public void PlayerHeartBeat(bool death, Collider playerCollider)
    {
        float closestDistance = float.MaxValue;

        foreach (Transform enemy in enemy)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.position);
                closestDistance = Mathf.Min(closestDistance, distance);
            }
        }

        // Adjust pitch based on distance
        float targetPitch = Mathf.Clamp(2.3f - (closestDistance / 29f), 0.7f, 2.3f); // Closer enemy increases tempo

        // Smooth pitch transition
        heartbeatAudioSource.pitch = Mathf.Lerp(heartbeatAudioSource.pitch, targetPitch, Time.deltaTime * 2f);

        if (closestDistance < 50f && !heartbeatAudioSource.isPlaying && !death && playerCollider.enabled == true)
        {
            heartbeatAudioSource.Play();
        }
        else if (closestDistance > 50f)
        {
            heartbeatAudioSource.Stop();
        }
    }

    /// <summary>
    /// Stops all sounds when the player dies.
    /// </summary>
    public void StopAllSounds()
    {
        StepsPlayerSource.Stop();
        RunPlayerSource.Stop();
        heartbeatAudioSource.Stop();
    }
}
