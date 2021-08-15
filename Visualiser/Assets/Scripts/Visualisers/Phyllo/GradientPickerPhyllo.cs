using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows for user specification of colours in visualiser
public class GradientPickerPhyllo : MonoBehaviour
{
    public TrailRenderer phyllo;

    private Gradient myGradient;
            private Gradient initGradient;
    void Start()
    {

        myGradient = phyllo.colorGradient;
        initGradient = phyllo.colorGradient;
    }
    private void Update()
    {
        phyllo.colorGradient = myGradient;

    }
    public void reset(){
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
