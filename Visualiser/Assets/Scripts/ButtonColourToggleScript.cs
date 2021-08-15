using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonColourToggleScript : MonoBehaviour
{
    public Image image;
    public Color defaultColour;
    public Color pressedColour;

    // Start is called before the first frame update
    void Start()
    {
        defaultColour = image.GetComponent<Image>().color;
        pressedColour = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
    public void ToggleColour()
    {
        Debug.Log("pressed");
        if(image.GetComponent<Image>().color == defaultColour)
        {
            image.GetComponent<Image>().color = pressedColour;
        }
        else
        {
            image.GetComponent<Image>().color = defaultColour;
        }
    }
}
