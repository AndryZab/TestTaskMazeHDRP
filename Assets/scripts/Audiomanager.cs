using UnityEngine;
using UnityEngine.UI;

public class Audiomanager : MonoBehaviour
{
    public AudioSource musicsourcedefault;
    public AudioSource musicsourceFollowMonster;
    public AudioSource PlayerDeathSource;
    public AudioSource SFXsource;

    private int currentMusicIndex = -1;
    private bool isPlaying = false;


    [Header("PlayerSounds")]
    public AudioClip Death;
    public AudioClip CollectKey;
    public AudioClip AllKeysCollected;
    public AudioClip Victory;
    public AudioClip Jump;

    public void PlaySFX(AudioClip clip)
    {
        if (SFXsource != null && clip != null)
        {
            SFXsource.PlayOneShot(clip);
        }
    }
   



}
