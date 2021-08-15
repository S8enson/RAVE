using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EqualiserSliderScript : MonoBehaviour
{
    public Text text;
    public Slider slider;
    
    
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = 3.00f;
        slider.minValue = 0.33f;
        slider.value = 1.00f;
        updateText();
    }

    public void updateText()
    {
        text.text = slider.value.ToString("0.00");
    }

    public void resetSlider()
    {
        slider.value = 1.00f;
        updateText();
    }

    public void toggleInteractable()
    {
        if(slider.IsInteractable())
        {
            slider.interactable = false;
        }
        else
        {
            slider.interactable = true;
        }
    }
}
