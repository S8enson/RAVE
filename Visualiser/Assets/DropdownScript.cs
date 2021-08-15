using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownScript : MonoBehaviour
{

    public TMPro.TMP_Dropdown dropdown;
    public void toggleInteractable()
    {
        if (dropdown.IsInteractable())
        {
            dropdown.interactable = false;
        }
        else
        {
            dropdown.interactable = true;
        }
    }
}
