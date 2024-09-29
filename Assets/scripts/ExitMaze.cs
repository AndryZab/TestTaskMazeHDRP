using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitMaze : MonoBehaviour
{
    public GameObject panelExitMaze;
    public GameObject DoorExitMaze;
    private KeysShowGUI KeysShowGUIScript;
    private Audiomanager AudiomanagerScript;
    private void Start()
    {
       AudiomanagerScript = FindObjectOfType<Audiomanager>();
       KeysShowGUIScript = FindObjectOfType<KeysShowGUI>();
    }
    private void Update()
    {
        if (KeysShowGUIScript.countCollected == 3)
        {
            DoorExitMaze.transform.rotation = Quaternion.Euler(-90, -90, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            AudiomanagerScript.PlaySFX(AudiomanagerScript.Victory);
            panelExitMaze.SetActive(true);
        }
    }
}
