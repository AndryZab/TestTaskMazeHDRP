using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject panelPause;
    private PlayerGUI playerGUI;
    private PlayerDeath playerDeath;

    private void Start()
    {
        playerGUI = FindObjectOfType<PlayerGUI>();
        playerDeath = FindObjectOfType<PlayerDeath>();
    }
    private void Update()
    {
        if (playerGUI != null && playerDeath.Death == false && playerGUI.panelsGUI[2].activeSelf == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (panelPause.activeSelf)
                {
                    panelPause.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Time.timeScale = 1f;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    panelPause.SetActive(true);
                    Time.timeScale = 0f;

                }
            }
        }

    }
}
