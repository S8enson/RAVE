using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PlayModeTests
{
    private GameObject testObject;
    private Audio audio;
    private AudioSource audioSrc;
    bool sceneLoaded;
    bool referencesSetup;
    private AudioManager audioManager;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("SongNameTestScene", LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoaded = true;
    }

    void SetupReferences()
    {
        if (referencesSetup)
        {
            return;
        }

        Transform[] objects = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in objects)
        {  
            if (t.name == "AudioManager")
            {
                audioManager = t.GetComponent<AudioManager>();
                // audioSrc.audioManager = audioManager;
            }
            if (t.name == "Audio Source")
            {
                audioSrc = t.GetComponent<AudioSource>();
                // audioManager.audioSrc = audioSrc;
            }
          
        }
        referencesSetup = true;
    }


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NowPlaying()
    {
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();

        string test = "kidcudi";
        Assert.AreEqual(test, audioSrc.clip.name);

        yield return null;
    }

    [UnityTest]
    public IEnumerator NowPlayingIsNotEmpty()
    {
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();
        Assert.NotNull(audioSrc.name);
    }

    [UnityTest]
    public IEnumerator CheckToggleFade()
    {
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();

        audioManager.fading = true;
        audioManager.ToggleFade();
        
        Assert.IsFalse(audioManager.fading);

        yield return null;
    }
}
