using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows for user specification of colours in visualiser
public class GradientPickerBasic : MonoBehaviour
{
    public BarVis basic;

    private Gradient myGradient;

    private Gradient initGradient;

    void Start()
    {
        myGradient = basic.gradient;
        initGradient = basic.gradient;
    }

    private void Update()
    {
        basic.gradient = myGradient;
    }

    public void reset()
    {
        myGradient = initGradient;
    }

    public void ChooseGradientButtonClick()
    {
        GradientPicker.Create(myGradient, "Choose the bars's colours!", SetGradient, GradientFinished);
    }

    private void SetGradient(Gradient currentGradient)
    {
        myGradient = currentGradient;
    }

    private void GradientFinished(Gradient finishedGradient)
    {
        Debug.Log("You chose a Gradient with " + finishedGradient.colorKeys.Length + " Color keys");
    }
}
