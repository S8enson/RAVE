using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ricimi
{
    // This class is responsible for loading the next scene in a transition (the core of
    // this work is performed in the Transition class, though).
    [System.Serializable]
    public class SceneTransitionManagerScript : MonoBehaviour
    {
        public string[] scene = new string[2] {"Home", "Equaliser"}; 
        public float duration = 1.0f;
        public Color color = Color.black;

        public void PerformTransition(string scene)
        {
            Transition.LoadLevel(scene, duration, color);
        }

        public void transitionToHome()
        {
            Transition.LoadLevel(scene[0], duration, color);
        }

        public void transitionToEqualiser()
        {
            Transition.LoadLevel(scene[1], duration, color);
        }

    }
}



