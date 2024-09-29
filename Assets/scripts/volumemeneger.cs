using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volumemeneger : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] public Slider musicslider;
    [SerializeField] private Slider soundSlider;
   


    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusisVolume();
            SetSoundVolume();
        }
    }

    public void SetMusisVolume()
    {
        float volume = musicslider.value;
        float maxVolume = Mathf.Lerp(-50f, 4f, volume);
        myMixer.SetFloat("music", maxVolume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSoundVolume()
    {
        float volume = soundSlider.value;
        float maxVolume = Mathf.Lerp(-50f, 4f, volume);
        myMixer.SetFloat("Sound", maxVolume);
       
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }


    private void LoadVolume()
    {
        musicslider.value = PlayerPrefs.GetFloat("musicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMusisVolume();
        SetSoundVolume();
    }
}
