using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows for user specification of colours in visualiser
public class GradientPickerAFF : MonoBehaviour
{
    public AudioFlowField aFF;

    private Gradient myGradient;
    private Gradient initGradient;
    void Start()
    {

        myGradient = aFF.gradient2;
        initGradient = aFF.gradient2;
    }
    private void Update()
    {
        aFF.gradient2 = myGradient;

    }
    public void reset(){
        myGradient = initGradient;
    }
    public void ChooseGradientButtonClick()
    {
        GradientPicker.Create(myGradient, "Choose the particle's colours!", SetGradient, GradientFinished);
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
