using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLib : MonoBehaviour
{
    // Passes audio component to scene
    public AudioManager am;
    public PlayListGUIManager gui;
    public LibraryManager libManager;
    void Start()
    { am = AudioManager.Instance;
        gui.audioManager = am;
        libManager.audioManager = am;
        am.libManager = libManager;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
