using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButonToggleInteractable : MonoBehaviour
{
    public Button button;
    public void toggleInteractable()
    {
        if (button.IsInteractable())
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
