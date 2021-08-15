using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAudioFF : MonoBehaviour
{
        // Passes specific visualiser settings instance to settings when scene is 'entered' & passes audio component to scene
    public AudioFlowField aFF;
    public GameObject settingsUIFF;

    // Start is called before the first frame update
    void Start()
    {
        aFF.audio = Audio.Instance.gameObject.GetComponent<Audio>();
        Settings.Instance.vSettingsUI = settingsUIFF;
        Settings.Instance.DismissV();

        Settings.Instance.DismissP();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
