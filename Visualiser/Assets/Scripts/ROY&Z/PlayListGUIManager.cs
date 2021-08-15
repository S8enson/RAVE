using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//v5 jul to merge to v6
public class PlayListGUIManager : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> trackLabelsInPlayList;
    public Button autoplayButton;
    public RectTransform contentTransform;
    public static PlayListGUIManager instance = null;
    public AudioManager audioManager;

    private string tickSymbol = "\u2713";
   
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
          
        }
        else
        {
            Destroy(gameObject);
        }

       
    }

    public void RemoveTrackFromPlayList()
    {
        Destroy(contentTransform.GetChild(audioManager.GetCurrentTrackIdx()));

    }
    
    void Start()
    {
        BuildPlayList();
    }

    public void ToggleEnableAutoPlay()
    {

        bool autoplayCurrently = audioManager.enableAutoplay;
        if (autoplayCurrently)
            autoplayButton.GetComponent<Image>().color = Color.gray;
        else
            autoplayButton.GetComponent<Image>().color = Color.white;

        audioManager.enableAutoplay = !autoplayCurrently;

        
    }

    public void ShufflePlaylist()
    {

        int numTrackLabels = contentTransform.childCount;
        List<AudioClip> playList = audioManager.playList;
        int currentTrackIdx = audioManager.GetCurrentTrackIdx();
        if (numTrackLabels > 1)
        {

            int currentHighlightedIdx = currentTrackIdx;
            bool enableLooping = audioManager.enableLooping;
            bool enableAutoplay = audioManager.enableAutoplay;

            for (int i = 0; i < numTrackLabels - 1; i++)
            {

                int swapTo = Random.Range(i + 1, numTrackLabels - 1);
                Transform swapFromLabel = contentTransform.GetChild(i);

               
                Transform swapToLabel = contentTransform.GetChild(swapTo);
                swapToLabel.SetSiblingIndex(i);
                swapFromLabel.SetSiblingIndex(swapTo);
                
                print(swapToLabel.GetComponent<Button>().GetComponentInChildren<Text>().text);

                Debug.Log("swaptolabel sibling index "  + contentTransform.GetChild(swapToLabel.GetSiblingIndex()).GetComponent<Button>().GetComponentInChildren<Text>().text);
                Debug.Log("swapTolabel swapTo = " + swapTo);
                print("-------------------");

                SwapInList(i, swapTo, ref playList);
                swapFromLabel.GetComponentInChildren<Button>().name = swapTo + "";
                swapToLabel.GetComponentInChildren<Button>().name = "" + i;

                if (swapTo == currentHighlightedIdx || i == currentHighlightedIdx)
                {
                    //currentTrackIdx is guaranteed to swapped only once.
                    if (i == currentHighlightedIdx)
                    {
                        audioManager.SetCurrentTrackIdx(swapTo);
                    }
                    else if (swapTo == currentHighlightedIdx)
                    {
                        audioManager.SetCurrentTrackIdx(i);

                    }

                    BottomPanelGUIManager.instance.AdjustNextTrackButtonState(enableLooping);
                    BottomPanelGUIManager.instance.AdjustPrevTrackButtonState(enableAutoplay);
                
                }
            }
        }

    }

public void addSong(){
    Debug.Log("why");
    audioManager.AddSong();
}

    private void SwapInList<T>(int i, int j, ref List<T> container)
    {
        T temp = container[i];
        container[i] = container[j];
        container[j] = temp;
    }


    public void BuildPlayList()
    {

        List<AudioClip> clips = audioManager.playList;
        contentTransform = transform.Find("Scroll View/Viewport/Content") as RectTransform;
     
        for (int i = 0; i < clips.Count; i++)
        {
            string trackName = clips[i].name;

            GameObject newLabel = Instantiate(prefab, contentTransform);
            Text txt = newLabel.GetComponentInChildren<Text>();
            txt.color = Color.gray;

            Button playListBtn = newLabel.GetComponent<Button>();
            playListBtn.name = i.ToString();

            txt.text = trackName;
           

        }
       
        if (contentTransform.childCount > 0)
        {
            Highlight(0);
        }
       

    }


    public void UpdatePlayListAppearance(int currentTrackIdx, bool loop)
    {
        AudioSource src = audioManager.audioSrc;
     
        if (!src.loop)
        {
            Destroy(contentTransform.GetChild(currentTrackIdx));

        }
        else
        {
            Unhighlight(currentTrackIdx);
        }

        if (!audioManager.PlayListEmpty())
        {
            Highlight((currentTrackIdx + 1) % contentTransform.childCount );
        }

    }

    public void Highlight(int i)
    {

        Text txt = contentTransform.GetChild(i).GetComponentInChildren<Text>();
        txt.color = Color.white;

    }

    public void Unhighlight(int i)
    {
        Text txt = contentTransform.GetChild(i).GetComponentInChildren<Text>();
        txt.color = Color.gray;
    }

    public void RemoveFinishedTrackLabel(int i)
    {

        Destroy(contentTransform.GetChild(i));

    }
     
    //this function is assumed to always be called before AudioManager.currentTrackIdx is incremented (because NextTrack is called)
    public void AdjustPlayListAppearance(bool looping)
    {

        int numTracks = audioManager.GetNumTracks();
        int currentTrackIdx = audioManager.GetCurrentTrackIdx();

        if (numTracks - 1 > 0)
        {
            Highlight((currentTrackIdx + 1) % numTracks);

            if (looping)
            {
                Unhighlight(currentTrackIdx);
                Debug.Log("unhighlighted");
            }
            else
            {
                Destroy(contentTransform.GetChild(currentTrackIdx));
            }
        }
    }
     
   
         
        
}

