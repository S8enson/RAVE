using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ProgressBarManager : MonoBehaviour
{
    public AudioManager audioManager;
    public Text timeText;
    public Slider progressBar;
    private AudioSource AudioSrc;
    private AudioClip currentSong;
    private string printableTimeString;
    public Settings settings;
    public bool wasPlaying = true;
    public SliderHandleScript handleScript;

    // Start is called before the first frame update
    void Start()
    {
        //setting the text to it's default amount, should never be shown, but if it does it means there is a problem. 
        timeText.text = "00:00 / 00:00";
        //getting our audio source from the audio manager
        AudioSrc = audioManager.GetAudioSource();

        currentSong = audioManager.GetCurrentSong();
        progressBar.minValue = 0;
        progressBar.maxValue = currentSong.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioSrc != null)
        {
            if (AudioSrc.isPlaying)
            {
                //grabs te current song from the audioManager, used to ensure that the times shown are correct when songs we next or previous track. 
                currentSong = audioManager.GetCurrentSong();

                //Debug.Log(currentSong.name);
                //Debug.Log(currentSong.length);

                //updates the maxValue in case the current song has changed. 
                progressBar.maxValue = currentSong.length;

                //gets the progress bars current value from the audio source. 
                
                if (!handleScript.sliderIsBeingHeldDown)
                {
                    progressBar.value = AudioSrc.time;
                    timeText.text = convertSecondsToPrintValue(AudioSrc.time, currentSong.length);
                }
            }
            else if ((progressBar.value + 1.0f) >= progressBar.maxValue)
            {
                if (!handleScript.sliderIsBeingHeldDown)
                {
                    audioManager.NextTrack();
                }
            }
            else
            {
                timeText.text = convertSecondsToPrintValue(progressBar.value, currentSong.length);
            }

            
        }
    }

    // A method to convert seconds from our current songs length and current time into a printable string.
    private string convertSecondsToPrintValue(float currentTime, float maxTime)
    {
        //Have to round to make sure that sounds under one second will still get progress times on the progress bar aka they will show 00:00 / 00:01 instead of just 00:00 / 00:00
        int currentTimeInt = (int)(Math.Round(currentTime));
        int maxTimeInt = (int)(Math.Round(maxTime));

        int minutes = TimeSpan.FromSeconds(currentTimeInt).Minutes;
        int seconds = TimeSpan.FromSeconds(currentTimeInt).Seconds;
        string printableTimeString = minutes.ToString("00") + ":" + seconds.ToString("00") + " / ";

        minutes = TimeSpan.FromSeconds(maxTimeInt).Minutes;
        seconds = TimeSpan.FromSeconds(maxTimeInt).Seconds;
        printableTimeString += minutes.ToString("00") + ":" + seconds.ToString("00");

        return printableTimeString;
    }


    public void playFromNewPosition()
    {
        if (AudioSrc.isPlaying)
            wasPlaying = true;
        else
            wasPlaying = false;

        AudioSrc.Pause();
        
        int currentTimeInt = (int)(Math.Round(progressBar.value));

        AudioSrc.time = currentTimeInt;

        AudioSrc.Play();
        
    }

    public void PauseWhileHeld()
    {
        if (AudioSrc.isPlaying)
            wasPlaying = true;
        else
            wasPlaying = false;
        
        AudioSrc.Pause();
    }

    public void PlayWhenReleased()
    {
       if(!wasPlaying)
        {
            settings.playSwap();
        }
        //if (settings != null)
        //{
        //    settings.playerBTN.SetActive(false);
        //    settings.pauseBTN.SetActive(true);
        //}
        if (Math.Round(progressBar.value) != Math.Round(AudioSrc.time))
        {
            if (progressBar.value >= currentSong.length)
                audioManager.NextTrack();
            else
                AudioSrc.time = progressBar.value;
        }
        
        AudioSrc.Play();

        //audioManager.TogglePlayPauseButton();

        //if (!AudioSrc.isPlaying)
        //{
        //    Debug.Log("we are here");
        //    if (settings != null)
        //    { 
        //        settings.playerBTN.SetActive(false);
        //        settings.pauseBTN.SetActive(true);
        //    }

        //    if (Math.Round(progressBar.value) != Math.Round(AudioSrc.time))
        //    {
        //        if (progressBar.value >= currentSong.length)
        //            audioManager.NextTrack();
        //        else
        //            AudioSrc.time = progressBar.value;
        //    }

        //    AudioSrc.Pause();
        //    AudioSrc.Play();

        //    if (settings != null)
        //    {
        //        settings.playerBTN.SetActive(false);
        //        settings.pauseBTN.SetActive(true);
        //    }

        //}
        //else if(AudioSrc.isPlaying)
        //{
        //    if (settings != null)
        //    {
        //            settings.playerBTN.SetActive(true);
        //            settings.pauseBTN.SetActive(false);
        //    }
        //    AudioSrc.Pause();
        //}
    }

}
