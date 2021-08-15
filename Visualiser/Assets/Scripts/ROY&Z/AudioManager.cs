using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
//using UnityEditor;
using System.IO;
using System.Text;
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using System.IO;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

/*
 * This class should not directly deal with GUI related stuff,
 * rather, it should deal with audio related stuff.
 * What I mean is the responsibilities of updating/removing/adding GUI components
 * are preferrably done in some other classes such as the PlayListGUIManager
 * However, the removing of AudioClips or changing "currentTrackIdx" should be done here.
 */
public class AudioManager : MonoBehaviour
{
    //public TextAsset songs;
    public string currentPlaylist {set; get;}
    public Audio audioSam;
    public AudioSource micSource;
    public bool useMic;
    public Slider volSlider;
    public Settings settingsSam;
    public bool shuffle;
    int prevTrack;

    public bool startup;
    public void setUseMic()
    {
        if (useMic)
        {
            //useMic=!useMic;
            //audioSam.audioSource = audioSrc;
            //audioSam.changeMixer();
            //audioSrc.UnPause();
            TogglePlayPauseButton();
        }
        else
        {
            //audioSam.audioSource = micSource;
            TogglePlayPauseButton();
            //audioSrc.Pause();
            //useMic=!useMic;


        }
        useMic = !useMic;
    }
    ////////////////////////
    public Text debugMessage;
    public List<AudioClip> playList;
    public List<AudioClip> allSongs = new List<AudioClip>();
    public List<int> shuffledPlayList;
    private int currentTrackIdx = 0;
    public AudioSource audioSrc;
    public AudioMixer mixer;
    public static AudioManager instance;
    public bool enableAutoplay = false;
    public bool enableLooping = true;
    public LibraryManager libManager;

    public bool isPaused;// = true;
    private string path;
    public bool fading;
    public TextAsset songs;
    public float[] customEqualiserValues;

    public bool customPlaylist;

    void Start()
    {
        customPlaylist = false;
        customEqualiserValues = new float[10] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
        startup = true;
        shuffle = false;
        isPaused = false;
        
        ReadString();
        
        ShufflePlaylist();
        //playList = new List<AudioClip>();
        useMic = false;
        //////
        if (playList.Count != 0)
        {

            audioSrc.playOnAwake = false;
            audioSrc.clip = playList[0];
            LoadClip(0);
            ShufflePlaylist();

        }
        else
        {
            Debug.Log("Clip empty ");
            //AddSong();
            //GameObject.Find("SelectSongButton").GetComponentInChildren<Button>().interactable = false;

            //to do:
            //BottomPanelGUIManager.instance.playButton.enabled = false;
        }
        //StartCoroutine("PlayListDaemon");
        
        audioSam.audioSource = audioSrc;
        startup = false;
        //currentTrackIdx = 0;

        fading = true;
        LoadClip(0);
        

    }
    public static AudioManager Instance
    {
        get { return instance; }
    }

    private float fadePeriod = 1f;
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            audioSrc = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    
    //Write to a text file the path of new songs when they are added.
    void WriteString(string songpath)
    {// creates path for song save file if not already created
        if (!Directory.Exists(Application.persistentDataPath + "/songs"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/songs");
        }
        string path = Application.persistentDataPath + "/songs/All Songs.txt";


        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(songpath);
        writer.Close();

    }

    //Reads in a string from a text file, is used to add songs to RAVE on startup that have previously been addedd.
    public void ReadString()
    {
        
        if (!Directory.Exists(Application.persistentDataPath + "/songs"))
        {
            return;
        }
        string path = Application.persistentDataPath + "/songs/All Songs.txt";

        Debug.Log("path" + path);

        StreamReader reader = new StreamReader(path);


        String[] Readfile = new string[File.ReadAllLines(path).Length];

        String line;
        int t = 0;

        // adds each song to playlist
        while ((line = reader.ReadLine()) != null)
        {
            Debug.Log(Readfile[t]);
            loadSingle(line);
            // StartCoroutine(SingleOutputRoutine(line));
            Readfile[t] = line;

            t += 1;
        }


    }

    //Ensures that songs are added one at a time to the library to ensure no bugs occur
    public void loadSingle(string path){
        StartCoroutine(SingleOutputRoutine(path));
    }


    //Mutes or Unmutes the audio source
    public void ToggleMute()
    {
        audioSrc.mute = !audioSrc.mute;
    }

    void Update()
    {   
        if(!startup){
        if(audioSrc.clip == null){
            LoadClip(0);
            ShufflePlaylist();
        }

        //Debug.Log(currentTrackIdx);
        //Debug.Log("suck =my balls unity");
        if (audioSrc.time >= audioSrc.clip.length && !isPaused)
        {


            if (enableAutoplay)
            {
                Debug.Log("next track");
                NextTrack();
            }
            else
            {
                Debug.Log("finished");
                //no autoplay
                BottomPanelGUIManager.instance.ChangePlayButtonState(false);
                audioSrc.Stop();
            }

        }
        }else{

            ShufflePlaylist();
        }

    }

    //Returns the  currently playing audio clip, assume the playlist size is not 0
    public AudioClip GetCurrentClip()
    {
        if (playList.Count != 0) return playList[currentTrackIdx];
        else
        {
            return null;
        }

    }

    //Opens a file explorer to add song to the library.
    public void AddSong()
    {
        //Standalone File Browser is used to open a file explorer which only show certain types of audio files. 
        var paths = StandaloneFileBrowser.OpenFilePanel("Select Audio Tracks", "", "mp3", true);
        if (paths.Length > 0)
        {
            var urlArr = new List<string>(paths.Length);
            for (int i = 0; i < paths.Length; i++)
            {
                urlArr.Add(paths[i]);
            }
            StartCoroutine(OutputRoutine(urlArr.ToArray()));
        }
    }

    //A bool that lets other functions know if the playlist has reached the end.
    public bool ReachedEndOfPlayList()
    {
        return currentTrackIdx == playList.Count - 1;
    }

    //A bool that lets other functions know if the playlist has reached the start.
    public bool ReachedStartOfPlayList()
    {
        return currentTrackIdx == 0;
    }



    // Used to add a song to playlist from a file on computer
    private IEnumerator SingleOutputRoutine(string songpath)
    {

        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + songpath, AudioType.MPEG);

        yield return www.SendWebRequest();

        if (www.isNetworkError){
            Debug.Log(www.error);
            //callback(null);
        }
        else
        {

            AudioClip ac = DownloadHandlerAudioClip.GetContent(www);
            ac.name = Path.GetFileName(songpath).Replace(".mp3", String.Empty);

            playList.Add(ac);
            // if(!allSongs.Contains(ac)){
            // allSongs.Add(ac);}

        }
        ShufflePlaylist();
    }

    // Used to add multiple songs to playlist from files on computer
    private IEnumerator OutputRoutine(string[] urlArr)
    {

        for (int i = 0; i < urlArr.Length; i++)
        {

            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + urlArr[i], AudioType.MPEG);
            WriteString(urlArr[i]);
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);

            }
            else
            {

                AudioClip ac = DownloadHandlerAudioClip.GetContent(www);

                ac.name = Path.GetFileName(urlArr[i]).Replace(".mp3", String.Empty);
                String songName = ac.name;

                libManager.addSongLabel(songName);

                playList.Add(ac);
            if(!allSongs.Contains(ac)){
            allSongs.Add(ac);}
            }

        }
        ShufflePlaylist();
    }


    //sets the current track ID to the interger this function is passsed
    internal void SetCurrentTrackIdx(int i)
    {
        currentTrackIdx = i;
    }

    public void ToggleEnableAutoplay()
    {

    }

    //Returns the current track ID
    public int GetCurrentTrackIdx()
    {
        return currentTrackIdx;
    }
    //This function is necessary because it enables the user to select a clip from the playlist,
    //not to be confused with the PlayMusic function attached to the play button.
    public void LoadClip(int i)
    {


        currentTrackIdx = i;
        audioSrc.clip = playList[currentTrackIdx];

        if (!IsPlaying())
        {
            TogglePlayPauseButton();
        }
        else
        {
            audioSrc.Play();
        }
        Highlight(audioSrc.clip);
    }

    public void EnableLooping()
    {
        enableLooping = true;
    }
    //this function is attached to the Play button
    public void TogglePlayPauseButton()
    {
        isPaused = !isPaused;
        //BottomPanelGUIManager.instance.ChangePlayButtonState(!audioSrc.isPlaying);
        settingsSam.playSwap();
        if (!audioSrc.isPlaying)
        {

            audioSrc.Play();

        }
        else
        {
            if (fading)
            {
                StartCoroutine(FadeAudioSource.StartFade(audioSrc, 0.7f, 0.0001f));
            }
            else
            {
                audioSrc.Pause();
            }
        }

    }

    //Plays the next track in the playlist, if at the end of the playlist it also checks that looping is currently enabled
    public void NextTrack()
    {
        if(shuffledPlayList.Count < playList.Count){
                        startup = true;
            ShufflePlaylist();
            startup = false;
        }
        if (!enableLooping && ReachedEndOfPlayList())
        {
            if (audioSrc.time < audioSrc.clip.length)
                return;

            audioSrc.Stop();
            audioSrc.time = 0;
            return;

        }


        currentTrackIdx = (currentTrackIdx + 1) % shuffledPlayList.Count;
        Debug.Log(shuffledPlayList[currentTrackIdx]);
        audioSrc.clip = playList[shuffledPlayList[currentTrackIdx]];
        audioSrc.time = 0f;
        if (!isPaused)
        {
            audioSrc.Play();
        }
        Highlight(audioSrc.clip);
    }

    //Highlights the currently playing clip in the Library Scene
    public void Highlight(AudioClip clip)
    {
        if (SceneManager.GetActiveScene().name == "Library")
        {
            Text[] objects = Resources.FindObjectsOfTypeAll<Text>();
            foreach (Text t in objects)
            {

                if (t.text == clip.name)
                {
                    t.transform.parent.gameObject.GetComponent<Button>().Select();

                }
            }
        }
    }

    //Return a bool depending on if the playlist is empty or not
    public bool PlayListEmpty()
    {
        return playList.Count == 0;
    }

    //Play the previous track in the playlist
    public void PrevTrack()
    {

        //if enablelooping is false, and that we are at the start of the playlist, then
        //it's impossible to call this function since looping is disabled.
        // fixed previous problems. Now properly highlights song playing
        if (!enableLooping && ReachedStartOfPlayList())
        {
            if (audioSrc.time < audioSrc.clip.length)
                return;

            audioSrc.Stop();
            audioSrc.time = 0;
            return;
        }
        //Ensure that the current track is correctly in bounds
        currentTrackIdx = (currentTrackIdx - 1 + playList.Count) % shuffledPlayList.Count;

        audioSrc.time = 0f;
        //makes sure we play the correct next song, even if the playlist has been shuffled.
        audioSrc.clip = playList[shuffledPlayList[currentTrackIdx]];
        if (!isPaused)
        {
            audioSrc.Play();
        }
        Highlight(audioSrc.clip);
    }

    //return the current number of tracks in the library or playlist
    internal int GetNumTracks()
    {
        return playList.Count;
    }

    //Sets the Audio Sources volume to the float value that is passed.
    public float SetVolume(float val)
    {
        float converted = ConvertToDecibel(val);
        audioSrc.volume = converted;
        //returns a value so we can check it was converted correctly.
        return converted;
    }

    public float GetSliderValue()
    {
        return volSlider.value;
        //return BottomPanelGUIManager.instance.volumeSlider.value;
    }

    public float ConvertToDecibel(float value)
    {
        //the returned value can't be smaller than or equal to 0
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;

    }

    public bool IsPlaying()
    {
        return !isPaused;

    }

    //swaps the enableLooping booleab to its opposite value
    public void ToggleEnableLooping()
    {
        enableLooping = !enableLooping;

    }

    //swaps the shuffle boolean to its opposite value
    public void ToggleShuffle()
    {
        shuffle = !shuffle;
    }

    //A method to get the current audioClip from the audio manager so that other scripts can get information about it.
    public AudioClip GetCurrentSong()
    {
        return audioSrc.clip;
        //return playList[currentTrackIdx];
    }

    //A method to send the audio source to other scripts so they can control it, or get infromation from it. 
    public AudioSource GetAudioSource()
    {
        return audioSrc;
    }


    //returns the song name of the last song in the playlist
    public string getLastSongName()
    {
        return playList[playList.Count - 1].name;
    }


    //Shuffles the playlist into a random order. 
    public void ShufflePlaylist()
    {
        System.Random rnd = new System.Random();
        //Debug.Log(playList.Count);
        if(customPlaylist){
            if(shuffle){
                //makes the playlist shuffled
                shuffledPlayList = shuffledPlayList.OrderBy(c => rnd.Next()).ToList();
            }else{
                //unshuffles the playlist
                shuffledPlayList = shuffledPlayList.OrderBy(x => x).ToList();
            }
        }
        else if (!startup && !shuffle)
        {
            currentTrackIdx = shuffledPlayList[currentTrackIdx];
            Debug.Log(currentTrackIdx);
        }
        else if (shuffle)
        {
            
            shuffledPlayList = Enumerable.Range(0, playList.Count).OrderBy(c => rnd.Next()).ToList();
            if (!startup)
            {
                //Debug.Log(shuffledPlayList.Find(x => x == currentTrackIdx));
                currentTrackIdx = shuffledPlayList.FindIndex(x => x == currentTrackIdx);
            }
        }
        else
        {
            shuffledPlayList = Enumerable.Range(0, playList.Count).ToList();
            if (startup) { currentTrackIdx = 0; }
        }
        //shuffle = !shuffle;


    }


    //swaps the fading boolean to its opposite value
    public void ToggleFade()
    {
        fading = !fading;
    }

    //Delets a song from the library based on the index it has been passed
    public void DeleteSong(int index)
    {
        if (index == currentTrackIdx)
        {
            NextTrack();
        }
        print("Removing " + playList[index] + " from playlist.");
        var oldLines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/songs/All Songs.txt");

        var newLines = oldLines.Where(line => !line.Contains(playList[index].name));
        System.IO.File.WriteAllLines(Application.persistentDataPath + "/songs/All Songs.txt", newLines);
        playList.RemoveAt(index);

    }


    //If the audio source is playing, pauses it. 
    public void stop()
    {
        if (audioSrc.isPlaying)
        {
            audioSrc.Pause();
            settingsSam.playSwap();
        }
    }

    //Loads a playlist based on the path it has been passed. 
    public void loadPlaylist(string path)
    {
        customPlaylist = true;

        path = Application.persistentDataPath + "/playlists/" + path + ".txt";
        //Debug.Log("path" + path);
        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);


            String[] Readfile = new string[File.ReadAllLines(path).Length];

            String line;
            int t = 0;

            // adds each song to playlist
            shuffledPlayList = new List<int>();
            while ((line = reader.ReadLine()) != null)
            {

                
               for (int i = 0; i < playList.Count; i++)
               {
                   if(playList[i].name == line){
                       shuffledPlayList.Add(i);
                       break;
                   }
               } 
                Readfile[t] = line;

                t += 1;
            }
            ShufflePlaylist();
            LoadClip(shuffledPlayList[0]);


        }
    }



}

