using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//v5 jul to merge v6
public class BottomPanelGUIManager : MonoBehaviour
{
    //This is the GUI manager for the buttons (play, pause etc) at the bottom
    public static BottomPanelGUIManager instance;
 

    public Button playButton,
                  stopButton,
                  nextTrackButton,
                  prevTrackButton;
    
    public Slider volumeSlider;
    private string prevTrackSymbol = "|\u25C0";
    private string nextTrackSymbol = "\u25B6|";
    private string pauseSymbol = "||";
    private string playSymbol = "\u25B6";
    public Image SpeakerSwapImage;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            if (AudioManager.instance.playList.Count > 1) 
                nextTrackButton.enabled = true;
            else
                nextTrackButton.enabled = false;
            prevTrackButton.GetComponentInChildren<Text>().text = prevTrackSymbol;
            nextTrackButton.GetComponentInChildren<Text>().text = nextTrackSymbol;
        }
        else
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);

    }


    void Start()
    {
        if (AudioManager.instance.playList.Count == 0)
        {
            playButton.interactable = false;
        }

        //volumeSlider.onValueChanged.AddListener((float _) => AudioManager.instance.SetVolume(_));
        
    }
    
    public void ToggleMute()
    {
        Image temp = GetComponent<Image>();
        GetComponent<Image>().sprite = SpeakerSwapImage.sprite;
        SpeakerSwapImage = temp;
        AudioManager.instance.ToggleMute();

    }
    
    //Disable/enable the Fast Forward Button if necessary
    public void AdjustNextTrackButtonState(bool enableLoop)
    {
        
        if (AudioManager.instance.ReachedEndOfPlayList())
        {
            if (enableLoop)
                ChangeNextButtonState(true);
            else
                ChangeNextButtonState(false);
        }

        ChangePrevButtonState(true);

    }

    public void AdjustPrevTrackButtonState(bool enableLooping)
    {
        
        
            if (AudioManager.instance.GetCurrentTrackIdx() == 0)
            {
                if (!enableLooping)
                    ChangePrevButtonState(false);
            }

            ChangeNextButtonState(true);


    }


    public void ChangeNextButtonState(bool val)
    {
        nextTrackButton.enabled = val;
    }

    public void ChangePrevButtonState(bool val)
    {
        prevTrackButton.enabled = val;
    }

  
    public void ChangePlayButtonState(bool toPlay)
    {
        //true --> pauseSymbol
        if (toPlay)
        {
            playButton.GetComponentInChildren<Text>().text = pauseSymbol;
        }
        //false --> playSymbol
        else
        {
            playButton.GetComponentInChildren<Text>().text = playSymbol;
        }

    }



}
