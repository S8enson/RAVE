// using System.Collections;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;
// using UnityEngine.UI;

// public class Karaoke
// {
//     private GameObject testObject;
//     private Audio audio;

//     [Setup]
//     public void Setup(){
//         testObject = GameObject.Instantiate(new GameObject());
//         audio = testObject.AddComponent<Audio>();
//     }
//     // A Test behaves as an ordinary method
//     [Test]
//     public void KaraokeModeFalseInit()
//     {
//         // Tests Karaoke Mode is False on Startup
        
//         Assert.IsFalse(audio.karaokeMode);
        
//     }

//     [Test]
//     public void KaraokeModeMicIsNull()
//     {
//         // Tests Karaoke Mic is Null on Startup
//         var audio = new Audio();

//         Assert.IsNull(audio.karaokeMic);
//     }

//     [Test]
//     public void KaraokeModeUpdate()
//     {
//         // Tests Karaoke Mic is Null on Startup
//         var audio = new Audio();
//         audio.kMicrophone = Microphone.devices[0];
//         audio.kMicChange = true;
//         audio.karaokeMode =  true;

//         Assert.NotNull(audio.karaokeMic);
//     }

//     [Test]
//     public void KaraokeModeMicNull()
//     {
//         // Tests Karaoke Mode is False on Startup
//         var audio = new Audio();
//         var microphone = new Microphone(); 
//         var micSource = new AudioSource();
//         audio.karaokeMic = micSource;
//         Assert.IsNull(audio.karaokeMic);
//     }
// }
