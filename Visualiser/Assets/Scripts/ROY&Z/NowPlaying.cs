using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NowPlaying : MonoBehaviour
{
    public Text nowPlayingText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(AudioManager.instance.GetCurrentTrackIdx() >= 0)
        {
            //nowPlayingText.text = "" + (AudioManager.instance.GetCurrentTrackIdx() + 1) + ". " + AudioManager.instance.GetCurrentSong().name;
            nowPlayingText.text = AudioManager.instance.GetCurrentSong().name;
        }
        else
        {
            nowPlayingText.text = "---------------------";
        }
    }
}
