using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHyperlink : MonoBehaviour
{
    public void OpenChannel(string url)
    {
        Application.OpenURL(url);
    }
}
