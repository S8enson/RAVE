using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Lasp;

//Core Audio Analysis Functionality
[RequireComponent(typeof (AudioSource))]
public class Audio : MonoBehaviour
{
    private AudioSource currentSource;

    public SpectrumAnalyzer micLeft;
    public SpectrumAnalyzer micRight;
    public void changeMixer()
    {
        audioSource.outputAudioMixerGroup = mixerGroupMaster;
    }

    public AudioSource audioSource;

    public AudioSource micSource;

    public AudioSource karaokeMic;

    AudioClip previous;

    //Mic input
    public AudioClip audioClip;

    public bool useMicrophone { get; set; }

    public bool micChange;

    public bool kMicChange;

    public bool karaokeMode { get; set; }

    public void setMicChange()
    {
        micChange = true;
    }

    public void setKMicChange()
    {
        kMicChange = true;
    }

    public Toggle karaokeToggle;

    public Text karaokeToggleText;


    public AudioMixerGroup

            mixerGroupMic,
            mixerGroupMaster;

    public Dropdown micDropdown;

    public Dropdown karaokeMicDropdown;

    public string microphone;

    public string kMicrophone;

    private List<string> options = new List<string>();

    private Unity.Collections.NativeArray<float> samplesLeft;// = new float[512];

    private Unity.Collections.NativeArray<float> samplesRight;// = new float[512];

    private float[] karaokeSamples = new float[512];

    // 8 band
    private float[] freqBand = new float[8];

    private float[] bandBuffer = new float[8];

    private float[] bufferDecrease = new float[8];

    private float[] freqBandHighest = new float[8];

    [HideInInspector]
    public float[]

            audioBand,
            audioBandBuffer;

    // 64 band
    private float[] freqBand64 = new float[64];

    private float[] bandBuffer64 = new float[64];

    private float[] bufferDecrease64 = new float[64];

    private float[] freqBandHighest64 = new float[64];

    [HideInInspector]
    public float[]

            audioBand64,
            audioBandBuffer64;

    [HideInInspector]
    public float

            amplitude,
            amplitudeBuffer;

    private float amplitudeHighest;

    public float audioProfile;

    public enum channel
    {
        Stereo,
        Left,
        Right
    }

    public channel channelInUse = new channel();

    private static Audio instance = null;

    private bool windows;

    // Start is called before the first frame update
    void Start()
    {
        // checks platform 
        if (
            Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor
        )
        {
            windows = true;
        }
        karaokeMode = false;

        // Do Not Destroy allows for carryover between scenes
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        audioBand = new float[8];
        audioBandBuffer = new float[8];
        audioBand64 = new float[64];
        audioBandBuffer64 = new float[64];
        audioSource = GetComponent<AudioSource>();
        AudioProfile (audioProfile);
        useMicrophone = false;

        // Mic input
        // Create Mixer with mic group turned to zero to avoid echoes
        // get all available microphones
        foreach (string device in Microphone.devices)
        {
            if (microphone == null)
            {
                //set default mic to first mic found.
                microphone = device;
            }
            options.Add (device);
        }

        		//add mics to dropdown
		micDropdown.AddOptions(options);
		micDropdown.onValueChanged.AddListener(delegate {
			micDropdownValueChangedHandler(micDropdown);
		});
        // repeat for karaoke dropdown
        		karaokeMicDropdown.AddOptions(options);
		karaokeMicDropdown.onValueChanged.AddListener(delegate {
			karaokeMicDropdownValueChangedHandler(karaokeMicDropdown);
		});

        // set mixer groups
        karaokeMic.outputAudioMixerGroup = mixerGroupMic;
        audioSource.outputAudioMixerGroup = mixerGroupMaster;
        audioSource.clip = audioClip;

        currentSource = audioSource;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // disables karaoke mode if mic is being used as input on mac, due to mac's one mic limitation
        if (!windows && useMicrophone)
        {
            karaokeToggle.interactable = false;
            karaokeToggleText.text =
                "Karaoke Mode, not available when using virtual mic on Mac";
        }
        else
        {
            karaokeToggle.interactable = true;
            karaokeToggleText.text = "Karaoke Mode";
        }
        // checks if mic input is selected and initiates mic as source if it has not been done or if a new mic is selected
        if (micChange && useMicrophone)
        {
            audioSource.Pause();
            UpdateMicrophone();
            micChange = false;
        }
        else if (micChange && !useMicrophone) // reverts to non-mic audio if mic de-selected
        {
            //play audio
            micChange = false;
            micSource.Stop();
            currentSource = audioSource;

            audioSource.UnPause();
        }
        // initiates karaoke mic & mode 
        if (kMicChange && karaokeMode && (!useMicrophone || windows))
        {
            KUpdateMicrophone();
            kMicChange = false;
        }
        else if (kMicChange && karaokeMode && useMicrophone && !windows)
        {
            karaokeMode = false;
            kMicChange = false;

        }
        // audio analysis
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        MakeFrequencyBands64();

        BandBuffer();
        BandBuffer64();

        CreateAudioBands();
        CreateAudioBands64();
        GetAmplitude();
    }

    public static Audio Instance
    {
        get
        {
            return instance;
        }
    }

    // used to change audio source to mic
    void UpdateMicrophone()
    {
        previous = audioSource.clip;

        //Start recording to audioclip from the mic
        micSource.clip = Microphone.Start(microphone, true, 10, 44100);
        micSource.loop = true;

        // Mute the sound with an Audio Mixer group becuase we don't want the player to hear it
        micSource.outputAudioMixerGroup = mixerGroupMaster;
        Debug.Log(Microphone.IsRecording(microphone).ToString());

        if (Microphone.IsRecording(microphone))
        {
            //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
            while (!(Microphone.GetPosition(microphone) > 0))
            {
            } // Wait until the recording has started.

            Debug.Log("recording started with " + microphone);

            // Start playing the audio source
            currentSource = micSource;
            micSource.Play();
        }
        else
        {
            //microphone doesn't work for some reason
            Debug.Log(microphone + " doesn't work!");
        }
    }

    // same as above but for karaoke mic
    void KUpdateMicrophone()
    {
        karaokeMic.clip = Microphone.Start(kMicrophone, true, 10, 44100);
        karaokeMic.loop = true;

        if (Microphone.IsRecording(kMicrophone))
        {
            //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
            while (!(Microphone.GetPosition(kMicrophone) > 0))
            {
            } // Wait until the recording has started.

            Debug.Log("Karaoke recording started with " + kMicrophone);

            // Start playing the audio source
            karaokeMic.Play();
        }
        else
        {
            //microphone doesn't work for some reason
            Debug.Log(kMicrophone + "karaoke doesn't work!");
        }
    }

    // handles dropdown usage, micChange turned to true if there is a change which causes trigger of UpdateMicrophone function from Update
    public void micDropdownValueChangedHandler(Dropdown mic)
    {
        microphone = options[mic.value];
        if (useMicrophone)
        {
            micChange = true;
        }
    }

    public void karaokeMicDropdownValueChangedHandler(Dropdown mic)
    {
        kMicrophone = options[mic.value];
        if (karaokeMode)
        {
            kMicChange = true;
        }
    }

    void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            freqBandHighest[i] = audioProfile;
        }
    }

    // finds current total amplitude
    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }
        if (currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }
        amplitude = currentAmplitude / amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }

    // finds highest 
    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBand[i];
            }
            audioBand[i] = (freqBand[i] / freqBandHighest[i]);

            audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
    }

    void CreateAudioBands64()
    {
        for (int i = 0; i < 64; i++)
        {
            if (freqBand64[i] > freqBandHighest64[i])
            {
                freqBandHighest64[i] = freqBand64[i];
            }
            audioBand64[i] = (freqBand64[i] / freqBandHighest64[i]);

            audioBandBuffer64[i] = (bandBuffer64[i] / freqBandHighest64[i]);
        }
    }

    // gets audio data 
    void GetSpectrumAudioSource()
    {
        if (useMicrophone)
        {
samplesLeft = micLeft.spectrumArray;
samplesRight = micRight.spectrumArray;
        }else{
        // AudioListener.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        // AudioListener.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
        }
        if (karaokeMode)
        {
            karaokeMic.GetSpectrumData(karaokeSamples, 0, FFTWindow.Blackman);
        }

    }

// buffer effect so visualisations do not look jittery, values ease from high to low rather than sudden jump.
    void BandBuffer64()
    {
        for (int i = 0; i < 64; i++)
        {
            if (freqBand64[i] > bandBuffer64[i])
            {
                bandBuffer64[i] = freqBand64[i];
                bufferDecrease64[i] = 0.005f;
            }
            if (freqBand64[i] < bandBuffer64[i])
            {
                bandBuffer64[i] -= bufferDecrease64[i];
                bufferDecrease64[i] *= 1.2f;
            }
        }
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
                bufferDecrease[i] = 0.005f;
            }
            if (freqBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

// creates 8 frequency bands
    void MakeFrequencyBands()
    {
        /*
        22,050/512 = 43Hz/sample
        0-86Hz ~ 2 samples 
        87-258 ~ 4 samples
        259-602 ~ 8 samples
        603 - 1290 ~ 16 samples
        1291 - 2666 ~ 32 samples
        2667 - 5418 ~ 64 samples
        5419 - 10922 ~ 128 samples
        10923 - 22,050 ~ 258 samples
        */
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            int sampleCount = (int) Mathf.Pow(2, i) * 2;
            float avg = 0;
            if (i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                if (channelInUse == channel.Stereo)
                {
                    avg +=
                        (
                        samplesLeft[count] +
                        samplesRight[count] +
                        karaokeSamples[count] * 2
                        ) *
                        (count + 1);
                }
                if (channelInUse == channel.Left)
                {
                    avg +=
                        (samplesLeft[count] + karaokeSamples[count]) *
                        (count + 1);
                }
                if (channelInUse == channel.Right)
                {
                    avg +=
                        (samplesRight[count] + karaokeSamples[count]) *
                        (count + 1);
                }

                count++;
            }
            avg /= count;
            freqBand[i] = avg * 10;
        }
    }
// creates 64 frequency bands
    void MakeFrequencyBands64()
    {
        /*
        bands 0-15 ~ 1 sample
        bands 16-31 ~ 2 samples
        bands 32-39 ~ 4 samples
        bands 40-47 ~ 6 samples
        bands 48-55 ~ 16 samples
        bands 56-64 ~ 32 samples
        */
        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for (int i = 0; i < 64; i++)
        {
            float avg = 0;
            if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
            {
                power++;
                sampleCount = (int) Mathf.Pow(2, power);
                if (power == 3)
                {
                    sampleCount -= 2;
                }
            }

            for (int j = 0; j < sampleCount; j++)
            {
                if (channelInUse == channel.Stereo)
                {
                    avg +=
                        samplesLeft[count] + samplesRight[count] * (count + 1);
                }
                if (channelInUse == channel.Left)
                {
                    avg += samplesLeft[count] * (count + 1);
                }
                if (channelInUse == channel.Right)
                {
                    avg += samplesRight[count] * (count + 1);
                }

                count++;
            }
            avg /= count;
            freqBand64[i] = avg * 80;
        }
    }
}
