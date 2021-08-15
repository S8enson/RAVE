using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAudioPhyllo : MonoBehaviour
{
        // Passes specific visualiser settings instance to settings when scene is 'entered' & passes audio component to scene
    public Phyllo phyllo, p2;
    public PhylloTunnel pT, pT2;
    public GameObject settingsUIP;
    // Start is called before the first frame update
    void Start()
    {
        phyllo.audio = Audio.Instance.gameObject.GetComponent<Audio>();
        p2.audio = Audio.Instance.gameObject.GetComponent<Audio>();
        pT.audio = Audio.Instance.gameObject.GetComponent<Audio>();
        pT2.audio = Audio.Instance.gameObject.GetComponent<Audio>();
        Settings.Instance.vSettingsUI = settingsUIP;

                Settings.Instance.DismissV();

        Settings.Instance.DismissP();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
