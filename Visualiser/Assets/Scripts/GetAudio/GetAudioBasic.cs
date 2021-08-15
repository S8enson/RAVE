using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAudioBasic : MonoBehaviour
{
    // Passes specific visualiser settings instance to settings when scene is 'entered' & passes audio component to scene
    public BarVis b0;
    public GameObject settingsUI;
    // Start is called before the first frame update
    void Start()
    {

        b0.audioVisualiser = Audio.Instance.gameObject.GetComponent<Audio>();

        Settings.Instance.vSettingsUI = settingsUI;

                Settings.Instance.DismissV();
        Settings.Instance.DismissP();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
