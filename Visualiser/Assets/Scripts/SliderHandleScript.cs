
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SliderHandleScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Slider theSlider;
    public bool sliderIsBeingHeldDown;

    public void OnPointerDown(PointerEventData data)
    {
        sliderIsBeingHeldDown = true;
    }
    public void OnPointerUp(PointerEventData data)
    {
        sliderIsBeingHeldDown = false;
    }
}