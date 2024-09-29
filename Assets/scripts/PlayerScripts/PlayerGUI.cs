using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    public GameObject[] panelsGUI;
    public bool anyPanelActive = false;
    public void Panels()
    {
        for (int i = 1; i < panelsGUI.Length; i++)
        {
            if (panelsGUI[i].activeSelf)
            {
                anyPanelActive = true; // Found an active panel
                break;
            }
        }
    }
}
