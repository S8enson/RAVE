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

using UnityEngine.EventSystems;
using SFB;
using System.IO;
using UnityEngine.SceneManagement;

public class PlaylistManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject createButton;
    public GameObject inputField;
    public GameObject addToggle;
    public GameObject deleteToggle;

    public Dropdown playlistDropdown;

    List<string> options;
    public String currentPlaylist;
    bool input;
    public bool add { get; set; }

    public bool delete { get; set; }
    public GameObject prefab;
    public static PlaylistManager instance;
    public AudioManager audioManager;

    bool startup;
    public List<GameObject> trackLabels;

    bool change;


    public RectTransform contentTransform;
    void Start()
    {
        options = new List<string>();
        trackLabels = new List<GameObject>();
        startup = true;
        inputField.SetActive(false);
        input = false;
        audioManager = AudioManager.Instance;
        currentPlaylist = audioManager.currentPlaylist;
        ReadString();

        updateDropdown();
        startup = false;
    }
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }
    public static PlaylistManager Instance
    {
        get { return instance; }
    }

    // Update is called once per frame
    void Update()
    {
        if (change)
        {
            UpdatePlaylistGUI();
        }
    }

    void ReadString()
    {


        if (!Directory.Exists(Application.persistentDataPath + "/playlists/all"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/playlists/all");

            StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/playlists/all/playlists.txt", true);
            writer.WriteLine(Application.persistentDataPath + "/songs/All Songs.txt");
            writer.Close();
            return;
        }
        string path = Application.persistentDataPath + "/playlists/all/playlists.txt";



        StreamReader reader = new StreamReader(path);


        String[] Readfile = new string[File.ReadAllLines(path).Length];

        String line;
        int t = 0;

        // adds each song to playlist
        while ((line = reader.ReadLine()) != null)
        {
            line = line.Replace(".txt", String.Empty);
            line = line.Replace("/Users/Sam/Library/Application Support/RAVE/RAVE/playlists/", String.Empty);
            line = line.Replace("/Users/Sam/Library/Application Support/RAVE/RAVE/songs/", String.Empty);
            options.Add(line);

            Readfile[t] = line;

            t += 1;
        }

        change = true;
    }

    public void CreatePress()
    {
        createButton.SetActive(input);
        inputField.SetActive(!input);
        input = !input;

    }

    void updateDropdown()
    {
        playlistDropdown.ClearOptions();
        playlistDropdown.AddOptions(options);
        playlistDropdown.onValueChanged.AddListener(delegate
        {
            playlistDropdownValueChangedHandler(playlistDropdown);
        });
        playlistDropdown.value = options.Count;
        change = true;

    }
    public void playlistDropdownValueChangedHandler(Dropdown plist)
    {
        currentPlaylist = options[plist.value];

        if (currentPlaylist == "All Songs" && !startup)
        {
            audioManager.customPlaylist = false;
            audioManager.ShufflePlaylist();
            audioManager.LoadClip(0);

        }
        else
        {
            audioManager.TogglePlayPauseButton();
            audioManager.loadPlaylist(options[plist.value]);




        }
        change = true;
    }

    public void Create(string name)
    {

        string path = Application.persistentDataPath + "/playlists/" + name + ".txt";
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/playlists/all/playlists.txt", true);
        writer.WriteLine(path);
        writer.Close();

        options.Add(name);
        updateDropdown();
        DeleteLabels();

    }

    public void DeleteLabels()
    {
        for (int i = 0; i < trackLabels.Count; i++)
        {
            Destroy(trackLabels[i]);
        }
        trackLabels.Clear();
    }
    public void playlistSelected(GameObject buttonClicked)
    {
        print("Button clicked!");

        //GameObject buttonClicked = EventSystem.current.currentSelectedGameObject;
        //print("Selected: " + buttonClicked.transform.GetChild(0));

        currentPlaylist = buttonClicked.transform.GetChild(0).name;
        buttonClicked.GetComponent<Button>().Select();
    }

    public void songSelected(GameObject buttonClicked)
    {
        if (currentPlaylist != "All Songs")
        {
            print("Button clicked!");



            string name = buttonClicked.transform.GetChild(0).GetComponentInChildren<Text>().text;

            if (delete)
            {
                //Destroy(buttonClicked);
                deleteSong(name);
            }
            else if (add)
            {
                addSong(name);
            }
        }

    }

    public void addSong(string name)
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/playlists/" + currentPlaylist + ".txt", append: true);
        writer.WriteLine(name);
        writer.Close();
        audioManager.loadPlaylist(currentPlaylist);
        change = true;


    }

    public void deleteSong(string name)
    {

        var oldLines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/playlists/" + currentPlaylist + ".txt");

        var newLines = oldLines.Where(line => !line.Contains(name));
        System.IO.File.WriteAllLines(Application.persistentDataPath + "/playlists/" + currentPlaylist + ".txt", newLines);
        audioManager.loadPlaylist(currentPlaylist);
        change = true;
    }
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.1f);
    }


    private void UpdatePlaylistGUI()
    {
        DeleteLabels();

        List<AudioClip> clips = AudioManager.instance.playList;
        List<int> index = AudioManager.instance.shuffledPlayList;
        for (int i = 0; i < index.Count; i++)
        {
            string trackName = clips[index[i]].name;

            GameObject newLabel = Instantiate(prefab, contentTransform);
            Text txt = newLabel.GetComponent<Button>().GetComponentInChildren<Text>();
            txt.name = i.ToString();
            txt.text = trackName;
            trackLabels.Add(newLabel);
        }

        change = false;
    }
}
