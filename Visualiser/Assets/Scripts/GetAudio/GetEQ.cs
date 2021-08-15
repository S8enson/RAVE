using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEQ : MonoBehaviour
{
    // Passes audio component to scene
    public AudioManager am;
    public EqualizerManager eq;
    
    void Start()
    { am = AudioManager.Instance;
        
        eq.audioManager = am;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
