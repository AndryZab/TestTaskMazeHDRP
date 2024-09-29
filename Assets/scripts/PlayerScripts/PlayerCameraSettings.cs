using System.Collections;
using UnityEngine;


public class PlayerCameraSettings: MonoBehaviour
{
    public Transform cameraTransform;

    [Header("Camera settings mouse rotate")]
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    public float minimumY = -60f;
    public float maximumY = 60f;
    private float rotationX = 180f; 
    private float rotationY = 0f;


    [Header("Camera settings bobbing")]
    public float bobFrequency = 5f;
    public float bobAmplitudeY = 0.15f;
    public float bobAmplitudeX = 0.1f;
    public float runBobMultiplier = 1.7f;
    private float bobTimer;
    private float currentBobAmplitudeY;
    private float currentBobAmplitudeX;
    private Vector3 cameraInitialPosition;

    [Header("Camera settings landing shake")]
    public float landingShakeDuration = 0.2f;


    private PlayerGUI playerGUI;
    void Start()
    {
        playerGUI = FindObjectOfType<PlayerGUI>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        cameraInitialPosition = cameraTransform.localPosition; // Store initial camera position

    }

    void Update()
    {
        bool allPanelsInactive = true;

        foreach (GameObject panel in playerGUI.panelsGUI)
        {
            if (panel != null && panel.activeSelf == true)
            {
                allPanelsInactive = false;
                break;
            }
        }

        if (allPanelsInactive)
        {

            cameraRotateMouse();
        }
    }
    public void cameraRotateMouse()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;

        
        rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY); 

        
        transform.localRotation = Quaternion.Euler(0, rotationX, 0);

        
        Camera.main.transform.localRotation = Quaternion.Euler(rotationY, 0, 0);
    }
    /// <summary>
    /// Applies a bobbing effect to the camera while moving.
    /// </summary>
    public void CameraBobbing()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            bobTimer += Time.deltaTime * bobFrequency; // Update bobbing timer

            float targetBobAmplitudeY = bobAmplitudeY;
            float targetBobAmplitudeX = bobAmplitudeX;

            if (Input.GetKey(KeyCode.LeftShift)) // Increase bobbing when running
            {
                targetBobAmplitudeY *= runBobMultiplier;
                targetBobAmplitudeX *= runBobMultiplier;
            }

            // Smoothly adjust the current bobbing amplitude
            currentBobAmplitudeY = Mathf.Lerp(currentBobAmplitudeY, targetBobAmplitudeY, Time.deltaTime * 5f);
            currentBobAmplitudeX = Mathf.Lerp(currentBobAmplitudeX, targetBobAmplitudeX, Time.deltaTime * 5f);

            // Apply the bobbing effect to the camera
            cameraTransform.localPosition = cameraInitialPosition + new Vector3(Mathf.Sin(bobTimer * 0.5f) * currentBobAmplitudeX, Mathf.Sin(bobTimer) * currentBobAmplitudeY, 0f);
        }
        else
        {
            // Reset bobbing when player stops moving
            bobTimer = 0f;
            currentBobAmplitudeY = 0f;
            currentBobAmplitudeX = 0f;
            cameraTransform.localPosition = cameraInitialPosition;
        }
    }

    /// <summary>
    /// Handles the camera's landing effect by creating a slight drop and return motion.
    /// </summary>
    public IEnumerator LandingEffect()
    {
        Vector3 originalPosition = cameraTransform.localPosition;
        Vector3 targetPosition = originalPosition + new Vector3(0, -0.33f, 0); // Slight drop
        float timer = 0f;

        // Lower the camera
        while (timer < landingShakeDuration / 1.3f)
        {
            cameraTransform.localPosition = Vector3.Lerp(originalPosition, targetPosition, timer / (landingShakeDuration / 1.3f));
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        // Return camera to original position
        while (timer < landingShakeDuration / 1.3f)
        {
            cameraTransform.localPosition = Vector3.Lerp(targetPosition, originalPosition, timer / (landingShakeDuration / 1.3f));
            timer += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPosition; // Reset camera to original position
    }
}
