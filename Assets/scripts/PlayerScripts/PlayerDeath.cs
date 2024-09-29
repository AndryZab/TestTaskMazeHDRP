using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private PlayerControls playerControlsScript;
    public GameObject PanelDeath;
    public Animator animatorDeathCamera;
    private Audiomanager audiomanagerScript;
    public bool Death = false;
    private void Start()
    {
        audiomanagerScript = FindObjectOfType<Audiomanager>();
        audiomanagerScript.PlayerDeathSource.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap") || other.CompareTag("Enemy") && Death == false)
        {

            audiomanagerScript.PlayerDeathSource.Play();
            animatorDeathCamera.enabled = true;
            PanelDeath.SetActive(true);
            Death = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
