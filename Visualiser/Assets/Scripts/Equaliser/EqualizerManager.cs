using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//[System.Serializable]
public class EqualizerManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider slider4;
    public Slider slider5;
    public Slider slider6;
    public Slider slider7;
    public Slider slider8;
    public Slider slider9;
    public Slider slider10;
    string sceneName = "Equaliser";
    public bool hasEqualiserBeenSetup = false;

    public Slider[] Sliders;
    public string[] parametersNames;
    private static EqualizerManager instance = null;

    public float[] customEqualiserValues;
    public bool areEqualiserValuesSaved;

    //Custom Equaliser Toggle elements
    public Toggle customEqualiserToggle;

    //controls the dropdown via the Equaliser
    public bool isToggled;

    public TMPro.TMP_Dropdown presetEqualisersDropdown;

    public bool usingCustomEqualiser;
    public int equaliserPreset;
    public AudioManager audioManager;
    private bool copyComplete;

    void Start()
    {  copyComplete = false;

        audioManager = AudioManager.Instance;
        customEqualiserValues = new float[10];// { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
        audioManager.customEqualiserValues.CopyTo(customEqualiserValues, 0);
        usingCustomEqualiser = true;
        copyComplete = true;
        // equaliserPreset = 0;
        // areEqualiserValuesSaved = false;
        
        
}

    void Update()
    {
        if (SceneManager.GetActiveScene().name == sceneName && hasEqualiserBeenSetup == false)
        {
            hasEqualiserBeenSetup = true;
            SetupEqualiser();
        }
        else if (SceneManager.GetActiveScene().name == sceneName && hasEqualiserBeenSetup == true)
        {
            updateEqualiser();
            saveCustomEqualiser();
            saveCurrentPresetEqualiser();
        }
        else if (SceneManager.GetActiveScene().name != sceneName)
        {
            hasEqualiserBeenSetup = false;
        }

        checkToggle();
        checkPresets();
        if(copyComplete){
        audioManager.customEqualiserValues = customEqualiserValues;
        }
    }

    public void SetupEqualiser()
    {
        slider1 = GameObject.Find("20-40hz Slider").GetComponent<Slider>();
        slider2 = GameObject.Find("40-80hz Slider").GetComponent<Slider>();
        slider3 = GameObject.Find("80-160hz Slider").GetComponent<Slider>();
        slider4 = GameObject.Find("160-300hz Slider").GetComponent<Slider>();
        slider5 = GameObject.Find("300-600hz Slider").GetComponent<Slider>();
        slider6 = GameObject.Find("600-1200hz Slider").GetComponent<Slider>();
        slider7 = GameObject.Find("1200-2400hz Slider").GetComponent<Slider>();
        slider8 = GameObject.Find("2400-5000hz Slider").GetComponent<Slider>();
        slider9 = GameObject.Find("5000-10000hz Slider").GetComponent<Slider>();
        slider10 = GameObject.Find("10000-20000hz Slider").GetComponent<Slider>();

        customEqualiserToggle = GameObject.Find("CustomEqualisersToggle").GetComponent<Toggle>();
        presetEqualisersDropdown = GameObject.Find("PresetEqualisersDropdown").GetComponent<TMPro.TMP_Dropdown>();

        customEqualiserToggle.isOn = usingCustomEqualiser;
        presetEqualisersDropdown.interactable = false;
        presetEqualisersDropdown.value = equaliserPreset;

        Sliders = new Slider[10] { slider1, slider2, slider3, slider4, slider5, slider6, slider7, slider8, slider9, slider10 };
        parametersNames = new string[10] { "30hz", "60hz", "120hz", "230hz", "450hz", "900hz", "1800hz", "3700hz", "7500hz", "15000hz" };

        for (int i = 0; i < Sliders.Length; i++)
        {
            float value;
            bool result = audioMixer.GetFloat(parametersNames[i], out value);
            if (result)
            {
                Sliders[i].value = customEqualiserValues[i];
                customEqualiserValues[i] =  Sliders[i].value;
            }
            else
            {
                return;
            }
        }
    }

    private void updateEqualiser()
    {
        for (int i = 0; i < Sliders.Length; i++)
        {
            audioMixer.SetFloat(parametersNames[i], Sliders[i].value);
            customEqualiserValues[i] =  Sliders[i].value;
        }
    }

    private void checkToggle()
    {
        if (customEqualiserToggle.isOn)
        {
            presetEqualisersDropdown.interactable = false;
            usingCustomEqualiser = true;

            if(areEqualiserValuesSaved == true)
            {
                setEqualiserToSavedValues();
                areEqualiserValuesSaved = false;
            }
        }
        else
        {
            presetEqualisersDropdown.interactable = true;
            usingCustomEqualiser = false;
            areEqualiserValuesSaved = true;
        }
    }

    private void setEqualiserToSavedValues()
    {
        for (int i = 0; i < Sliders.Length; i++)
        {
            Sliders[i].value = customEqualiserValues[i];
            audioMixer.SetFloat(parametersNames[i], Sliders[i].value);
        }
    }

    private void saveCustomEqualiser()
    {
        if (customEqualiserToggle.isOn)
        {
            for (int i = 0; i < 10; i++)
            {
                customEqualiserValues[i] = Sliders[i].value;
            }
        }
    }

    private void saveCurrentPresetEqualiser()
    {
        if (!customEqualiserToggle.isOn)
        {
            equaliserPreset = presetEqualisersDropdown.value;
        }
    }

    private void checkPresets()
    {
        if (presetEqualisersDropdown.IsInteractable())
        {
            setPresets();
        }
    }

    public void setPresets()
    {
        //if value = default
        if (presetEqualisersDropdown.value == 0)
        {
            for (int i = 0; i < Sliders.Length; i++)
            {
                Sliders[i].value = 1.0f;
            }
        }
        //if value = Mini Bass
        else if (presetEqualisersDropdown.value == 1)
        {
            slider1.value = 1.11f;
            slider2.value = 1.22f;
            slider3.value = 1.22f;
            slider4.value = 1.11f;

            for (int i = 4; i < Sliders.Length; i++)
            {
                Sliders[i].value = 1.0f;
            }
        }
        //if value = Medium Bass
        else if (presetEqualisersDropdown.value == 2)
        {
            slider1.value = 1.22f;
            slider2.value = 1.33f;
            slider3.value = 1.33f;
            slider4.value = 1.22f;

            for (int i = 4; i < Sliders.Length; i++)
            {
                Sliders[i].value = 1.0f;
            }
        }
        //if value = Deep Bass
        else if (presetEqualisersDropdown.value == 3)
        {
            slider1.value = 1.33f;
            slider2.value = 1.44f;
            slider3.value = 1.44f;
            slider4.value = 1.33f;

            for (int i = 4; i < Sliders.Length; i++)
            {
                Sliders[i].value = 1.0f;
            }
        }
        //if value = Mini Mids
        else if (presetEqualisersDropdown.value == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                Sliders[i].value = 1.0f;
            }

            slider5.value = 1.11f;
            slider6.value = 1.22f;
            slider7.value = 1.22f;
            slider8.value = 1.11f;

            for (int i = 8; i < Sliders.Length; i++)
            {
                Sliders[i].value = 1.0f;
            }
        }
        //if value = Medium Mids
        else if (presetEqualisersDropdown.value == 5)
        {
            for (int i = 0; i < 4; i++)
            {
                Sliders[i].value = 1.0f;
            }

            slider5.value = 1.22f;
            slider6.value = 1.33f;
            slider7.value = 1.33f;
            slider8.value = 1.22f;

            for (int i = 8; i < Sliders.Length; i++)
            {
                Sliders[i].value = 1.0f;
            }
        }
        //if value =  Deep Mids
        else if (presetEqualisersDropdown.value == 6)
        {
            for (int i = 0; i < 4; i++)
            {
                Sliders[i].value = 1.0f;
            }

            slider5.value = 1.33f;
            slider6.value = 1.44f;
            slider7.value = 1.44f;
            slider8.value = 1.33f;

            for (int i = 8; i < Sliders.Length; i++)
            {
                Sliders[i].value = 1.0f;
            }
        }
        //if value =  Mini Highs
        else if (presetEqualisersDropdown.value == 7)
        {
            for (int i = 0; i < 8; i++)
            {
                Sliders[i].value = 1.0f;
            }

            slider9.value = 1.11f;
            slider10.value = 1.22f;
        }
        //if value =  Medium Highs
        else if (presetEqualisersDropdown.value == 8)
        {
            for (int i = 0; i < 8; i++)
            {
                Sliders[i].value = 1.0f;
            }

            slider9.value = 1.22f;
            slider10.value = 1.33f;
        }
        //if value =  Deep Highs
        else if (presetEqualisersDropdown.value == 9)
        {
            for (int i = 0; i < 8; i++)
            {
                Sliders[i].value = 1.0f;
            }

            slider9.value = 1.33f;
            slider10.value = 1.44f;
        }
        //if value =  Delta Lite
        else if (presetEqualisersDropdown.value == 10)
        {
            slider1.value = 1.165f;
            slider2.value = 1.11f;
            slider3.value = 1.055f;
            slider4.value = 1.0f;
            slider5.value = 1.165f;
            slider6.value = 1.165f;
            slider7.value = 1.0f;
            slider8.value = 1.055f;
            slider9.value = 1.11f;
            slider10.value = 1.165f;

        }
        //if value =  Delta Normal
        else if (presetEqualisersDropdown.value == 11)
        {
            slider1.value = 1.33f;
            slider2.value = 1.22f;
            slider3.value = 1.11f;
            slider4.value = 1.0f;
            slider5.value = 1.33f;
            slider6.value = 1.33f;
            slider7.value = 1.0f;
            slider8.value = 1.11f;
            slider9.value = 1.22f;
            slider10.value = 1.33f;
        }
        updateEqualiser();
    }


        //private void toggleSliders()
        //{
        //    if (slider1.interactable == true)
        //    {
        //        for (int i = 0; i < Sliders.Length; i++)
        //        {
        //            Sliders[i].interactable = false;
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < Sliders.Length; i++)
        //        {
        //            Sliders[i].interactable = true;
        //        }
        //    }
        //}


    }