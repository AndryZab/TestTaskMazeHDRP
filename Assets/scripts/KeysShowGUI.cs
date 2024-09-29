using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the display of collected keys and UI fading effects.
/// </summary>
public class KeysShowGUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject[] keys; 
    [SerializeField] private TextMeshProUGUI textCount; 
    [SerializeField] private Image panelForShowCountKeys; 
    [SerializeField] private Image keyGUI;
    [SerializeField] private TextMeshProUGUI textAllFoundKeys; 

    private int countActive; 
    public int countCollected;
    private bool keyCollected;

    private Audiomanager audiomanagerScript;
    private void Start()
    {
        audiomanagerScript =FindObjectOfType<Audiomanager>();
        countActive = 0;
        foreach (GameObject key in keys)
        {
            if (key.activeInHierarchy)
            {
                countActive++;
            }
        }
    }

    public bool keyCollectedInterface
    {
        get => keyCollected; 
        set => keyCollected = value; 
    }

    private void Update()
    {
        UpdateKeyCounts(); // Refresh key count display
        HandleKeyCollection(); // Manage key collection and trigger fading effect
    }

    /// <summary>
    /// Updates the number of active keys displayed on the UI.
    /// </summary>
    private void UpdateKeyCounts()
    {
        
        textCount.text = $"{countCollected}/{countActive}"; // Update UI text
    }

    /// <summary>
    /// Checks if a key has been collected and starts fading out if all keys are collected.
    /// </summary>
    private void HandleKeyCollection()
    {
        foreach (GameObject key in keys)
        {
            if (!key.activeSelf && keyCollected)
            {
                countCollected++;
                keyCollected = false; // Reset the flag
                textCount.text = $"{countCollected}/3"; // Update UI text
                if (countCollected >= 3)
                {
                    audiomanagerScript.PlaySFX(audiomanagerScript.AllKeysCollected);
                    textAllFoundKeys.gameObject.SetActive(true);
                    StartCoroutine(FadeOutObjects()); // Start fading effect
                }
            }
        }
    }

    /// <summary>
    /// Fades out specified UI elements over time.
    /// </summary>
    private IEnumerator FadeOutObjects()
    {
        const float fadeDuration = 3f; // Duration of fade
        float startAlpha = 1f; // Initial alpha
        float endAlpha = 0f; // Final alpha

        // Fading images and text components
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration);
            SetAlpha(panelForShowCountKeys, alpha);
            SetAlpha(keyGUI, alpha);
            SetAlpha(textAllFoundKeys, alpha);
            SetAlpha(textCount, alpha);
            yield return null;
        }

        HideObjects(); // Hide the objects after fading
    }

    /// <summary>
    /// Sets the alpha value for a Graphic component.
    /// </summary>
    private void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic != null)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }

    /// <summary>
    /// Deactivates all objects after fading.
    /// </summary>
    private void HideObjects()
    {
        panelForShowCountKeys.gameObject.SetActive(false);
        keyGUI.gameObject.SetActive(false);
        textAllFoundKeys.gameObject.SetActive(false);
        textCount.gameObject.SetActive(false);
    }
}
