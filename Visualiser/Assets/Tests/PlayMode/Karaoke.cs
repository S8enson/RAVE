using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Karaoke
{
    private GameObject testObject;
    private Audio audio;
    bool sceneLoaded;
    bool referencesSetup;



    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("KaraokeTestScene", LoadSceneMode.Single);
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
            if (t.name == "Audio Source")
            {
                audio = t.GetComponent<Audio>();

            }
        }

        referencesSetup = true;
    }

    [UnityTest]
    public IEnumerator TestNotNullAfterLoad()
    {   // Tests Audio component is not null
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();
        Assert.IsNotNull(audio);

        yield return null;
    }

    [UnityTest]
    public IEnumerator KaraokeModeFalse()
    {
        // Tests Karaoke Mode is False on Startup
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();
        Assert.IsFalse(audio.karaokeMode);

    }

    [UnityTest]
    public IEnumerator KaraokeMicIsNull()
    {
        // Tests Karaoke Mic is Null on Startup
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();
        Assert.IsNull(audio.karaokeMic.clip);
    }

    [UnityTest]
    public IEnumerator KaraokeModeUpdate()
    {   // Tests Karaoke Mic is Not Null after selection and Update
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();



        audio.kMicrophone = Microphone.devices[0];
        audio.kMicChange = true;
        audio.karaokeMode = true;
        yield return null;
        Assert.NotNull(audio.karaokeMic);
        Assert.IsTrue(audio.karaokeMode);
        Assert.IsFalse(audio.kMicChange);

    }




}



