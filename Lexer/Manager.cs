using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject UI;
    void Start()
    {
        
    }

    // key press e
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            UI.SetActive(!UI.activeSelf);
        }
        
    }
}
