using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class EqualiserPresetsTest
{
    private GameObject testObject;
    private EqualizerManager equalizerManager;
    bool sceneLoaded;
    bool referencesSetup;

    //These are all taken and modified from the Karaoke test done by Sam
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("EqualiserTestScene", LoadSceneMode.Single);
    }

    //These are all taken and modified from the Karaoke test done by Sam
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoaded = true;
    }

    //These are all taken and modified from the Karaoke test done by Sam
    void SetupReferences()
    {

        if (referencesSetup)
        {
            return;
        }


        Transform[] objects = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in objects)
        {
            if (t.name == "EqualiserManager")
            {
                equalizerManager = t.GetComponent<EqualizerManager>();
            }

            if (t.name == "AudioManager")
            {
                equalizerManager.audioManager = t.GetComponent<AudioManager>();
            }
        }

        if (equalizerManager != null)
        {
            
            equalizerManager.SetupEqualiser();
        }

        referencesSetup = true;
    }

    //These are all taken and modified from the Karaoke test done by Sam
    [UnityTest]
    public IEnumerator TestSliderNotNullAfterLoad()
    {

        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();
        Assert.IsNotNull(equalizerManager);
        //Add all other references as well for quick nullref testing
        yield return null;
    }

    [UnityTest]
    //if the toggle is on it changes the interactability of the the dropdown
    public IEnumerator checkToggle()
    {
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();
        equalizerManager.customEqualiserToggle.isOn = true;

        if (equalizerManager.customEqualiserToggle.isOn)
        {
            equalizerManager.presetEqualisersDropdown.interactable = false;
        }
        else
        {
            equalizerManager.presetEqualisersDropdown.interactable = true;
        }

        Assert.IsFalse(equalizerManager.presetEqualisersDropdown.interactable);

        yield return null;
    }

    [UnityTest]
    //if the toggle is on it changes the interactability of the the dropdown
    public IEnumerator saveCustomEqualiser()
    {
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();
        equalizerManager.customEqualiserToggle.isOn = true;
        equalizerManager.slider1.value = 2.0f;

        if (equalizerManager.customEqualiserToggle.isOn)
        {
            for (int i = 0; i < 10; i++)
            {
                equalizerManager.customEqualiserValues[i] = equalizerManager.Sliders[i].value;
            }
        }

        Assert.IsTrue(equalizerManager.customEqualiserValues[0] == 2.0f);

        yield return null;
    }

    [UnityTest]
    public IEnumerator SetEqualiserToSavedValues()
    {
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();

        equalizerManager.customEqualiserValues = new float[10] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 3.0f };

        for (int i = 0; i < equalizerManager.Sliders.Length; i++)
        {
            equalizerManager.Sliders[i].value = equalizerManager.customEqualiserValues[i];
            equalizerManager.audioMixer.SetFloat(equalizerManager.parametersNames[i], equalizerManager.Sliders[i].value);
        }

        Assert.IsTrue(equalizerManager.slider10.value == 3.0f);

        yield return null;
    }

    [UnityTest]
    public IEnumerator testSetPresets()
    {
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();

        equalizerManager.presetEqualisersDropdown.value = 1;

        equalizerManager.setPresets();

        Assert.IsTrue(equalizerManager.slider1.value == 1.11f);

        yield return null;
    }
}
