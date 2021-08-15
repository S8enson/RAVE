using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class LibraryManager : MonoBehaviour
{
    public AudioManager audioManager;
    public PlaylistManager playlistManager;
    //public Button addSongButton;
    public int previousPlaylistSize;
    private int oneSecond = 1;

    string sceneName = "Library";
    //public bool hasAddSongButtonBeenSetup = false;

    //drag label in prefabForUI
    public GameObject prefab;

    //need to drag the content transform in scroll view
    public RectTransform contentTransform;

    // for deleting tracks
    private static bool deleteSongs;
    public List<GameObject> trackLabels;

    void Start(){
       
            deleteSongs = false;
            previousPlaylistSize = 0;
            trackLabels = new List<GameObject>();
            SetupLibrary();
            audioManager = AudioManager.Instance;
            audioManager.Highlight(audioManager.audioSrc.clip);
    }
    


    //Waits for a number of seconds.
    //Used in this script to ensure that songs are added to the library and to the labels properly
    IEnumerator waiter(int numberOfSeconds)
    {  
        yield return new WaitForSeconds(numberOfSeconds);
    }

    public void AddSong(){
        Debug.Log("RAVERAVERAVERAVERAVE");
        audioManager.AddSong();
        waiter(oneSecond);
        DeleteLibrary();
        SetupLibrary();
    }


    private void AddSongToLibrary()
    {
        //this content transform is the scrollview we want new song labels to be added to
        //this prefad is the a prefab for a track label and it related button. So that the button can be pressed to play that song
        GameObject label = Instantiate(prefab, contentTransform);
        label.GetComponent<Button>().GetComponentInChildren<Text>().text = audioManager.getLastSongName();
        label.GetComponent<Button>().name = "" + (audioManager.playList.Count-1);
    }

    private void DeleteLibrary()
    {
        //this content transform is the scrollview we want the labels to be deleted from.
        Transform[] ts = contentTransform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < ts.Length; i++)
        {
            Destroy(ts[i].GetComponent<Button>());
        }
                for (int i = 0; i < trackLabels.Count; i++)
        {
            Destroy(trackLabels[i]);
        }
        trackLabels.Clear();
    }

    private void SetupLibrary()
    {
        // Destroy(contentTransform);
        contentTransform = GameObject.Find("LibraryContentTransform").GetComponent<RectTransform>();
        UpdateLibrary();
    }

    private void UpdateLibrary()
    {
        List<AudioClip> clips = AudioManager.instance.playList;

        for (int i = 0; i < clips.Count; i++)
        {
            string trackName = clips[i].name;

            //this content transform is the scrollview we want new song labels to be added to
            //this prefad is the a prefab for a track label and it related button. So that the button can be pressed to play that song
            GameObject newLabel = Instantiate(prefab, contentTransform);
            Text txt = newLabel.GetComponent<Button>().GetComponentInChildren<Text>();
            txt.name = i.ToString();
            txt.text = trackName;
            trackLabels.Add(newLabel);
        }


    }

    public void addSongLabel(string songName)
    {
        GameObject newLabel = Instantiate(prefab, contentTransform);
        Text txt = newLabel.GetComponent<Button>().GetComponentInChildren<Text>();
        txt.name = audioManager.playList.Count.ToString();
        txt.text = songName;
    }

    // added by Julius
    public void toggleDelete(bool toggle){
        if (deleteSongs == false)
        {
            deleteSongs = true;
            print("Delete Songs on...");
        } 
        else
        {
            deleteSongs = false;
            print("Delete Songs off...");
        }
        // print(deleteSongs);
    }

    public void isSelected(GameObject buttonClicked){
        print("Button clicked!");
        
        //GameObject buttonClicked = EventSystem.current.currentSelectedGameObject;
        print("Selected: " + buttonClicked.transform.GetChild(0));

        if(SceneManager.GetActiveScene().name == "Playlist"){
            playlistManager = PlaylistManager.Instance;
            playlistManager.songSelected(buttonClicked);
        }else {
            if (deleteSongs)
        {
            
            
            print("Removing "+buttonClicked.transform.GetChild(0).GetComponentInChildren<Text>());

            int n = Int32.Parse(buttonClicked.transform.GetChild(0).name);
            AudioManager.instance.DeleteSong(n);
            // print("Child count: " + contentTransform.childCount);
            
            AdjustTrackIndices(n);

            Destroy(buttonClicked);
            
            
            
        }
        else
        {
            int n = Int32.Parse(buttonClicked.transform.GetChild(0).name);
            AudioManager.instance.LoadClip(n);
        }
        }
    }

    private void AdjustTrackIndices(int n){
        
        string indices = "Indices: ";
        print("Child count: " + contentTransform.childCount);
        for (int i = 0; i < n; i++){
            
            contentTransform.transform.GetChild(i).transform.GetChild(0).name = i.ToString();
            print(i.ToString() + ": "+contentTransform.transform.GetChild(i).transform.GetChild(0).name);
            indices += contentTransform.transform.GetChild(i).transform.GetChild(0).name + ", ";

        }

        for (int i = n + 1; i < contentTransform.childCount; i++){
            
            contentTransform.transform.GetChild(i).transform.GetChild(0).name = (i - 1).ToString();
            print(i.ToString() + ": "+contentTransform.transform.GetChild(i).transform.GetChild(0).name);
            indices += contentTransform.transform.GetChild(i).transform.GetChild(0).name + ", ";

        }
        print(indices);
    }



}

