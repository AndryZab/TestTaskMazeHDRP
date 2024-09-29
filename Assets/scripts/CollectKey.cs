using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectKey : MonoBehaviour
{
    private KeysShowGUI keysShow;
    private Audiomanager audiomanagerScript;
    private void Start()
    {
        audiomanagerScript = FindObjectOfType<Audiomanager>();
        keysShow = FindObjectOfType<KeysShowGUI>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audiomanagerScript.PlaySFX(audiomanagerScript.CollectKey);
            keysShow.keyCollectedInterface = true;
            gameObject.SetActive(false);
        }
    }
}
