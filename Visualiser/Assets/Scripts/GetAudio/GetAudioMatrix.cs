using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAudioMatrix : MonoBehaviour
{
        // Passes specific visualiser settings instance to settings when scene is 'entered' & passes audio component to scene
    public MatrixAdj shader;
    public GameObject settingsUI;

    // Start is called before the first frame update
    void Start()
    {
        shader.audio = Audio.Instance.gameObject.GetComponent<Audio>();
        Settings.Instance.vSettingsUI = settingsUI;

        Settings.Instance.DismissV();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
