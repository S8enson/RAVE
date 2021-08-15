using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ricimi;


public class Settings : MonoBehaviour
{
    public GameObject noKeyboardSettingsUI;
    bool noKeyboardSettings;
    //Audio Settings
    public static bool aSettingsVisible = false;
    public GameObject aSettingsUI;
    //Visualiser Settings
    public static bool vSettingsVisible = false;
    public GameObject vSettingsUI;
    //Equaliser
    public static bool eSettingsVisible = false;
    public GameObject eSettingsUI;
    //Info for info about each visualiser~how it works?
    public static bool iSettingsVisible = false;
    public GameObject iSettingsUI;
    // Player
    public static bool playerVisible = true;
    public GameObject playerUI;
    private bool play = false;
    public GameObject playerBTN;
    public GameObject pauseBTN;
    private bool mute = false;
    public GameObject muteBTN;
    public GameObject unMuteBTN;
    public GameObject notShuffleBTN, shuffleBTN;
    public bool shuffle = true;
    public GameObject notLoopBTN, loopBTN;
    public bool loop = true;

    public SceneTransition tran;

    float prevVol;

    //Spotify Player
    public static bool spotifyPlayerVisible = false;
    public GameObject spotifyPlayerUI;
    public GameObject spotifySignOutButton;

    //for changing between scenes
    SceneManager sceneManager;

    public static Settings instance = null;

    // Update is called once per frame
    void Start()
    {
        noKeyboardSettings = false;
        noKeyboardSettingsUI.SetActive(false);
        prevVol = 100;
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
        eSettingsUI.SetActive(false);
        playerBTN.SetActive(false);
        unMuteBTN.SetActive(false);
        notLoopBTN.SetActive(false);
        shuffleBTN.SetActive(false);
        //muteBTN.SetActive(false);
        playerUI.SetActive(true);
        spotifyPlayerUI.SetActive(false);
        sceneManager = new SceneManager();

        DismissA();
        DismissV();
        //makeVisibleP();



        //DismissP();
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Home")
        {
            spotifySignOutButton.SetActive(false);
        }
        else
        {
            spotifySignOutButton.SetActive(true);
        }
if(SceneManager.GetActiveScene().name != "Playlist"){
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (aSettingsVisible)
            {

                DismissA();
            }
            else
            {
                makeVisibleA();
            }

        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            if (vSettingsVisible)
            {

                DismissV();
            }
            else
            {
                makeVisibleV();
            }

        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (spotifyPlayerVisible)
            {
                DismissS();
            }

            if (playerVisible)
            {
                DismissP();
            }
            else
            {
                makeVisibleP();
            }

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (playerVisible)
            {
                DismissP();
            }

            if (spotifyPlayerVisible)
            {
                DismissS();
            }
            else
            {
                makeVisibleS();
            }
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            if (SceneManager.GetActiveScene().name != "Home")
            {
                //SceneManager.LoadScene("Home");
                tran.PerformTransition("Home");
            }
        }
}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "Home")
            {
                if (SceneManager.GetActiveScene().name == "Basic" || SceneManager.GetActiveScene().name == "Phyllo" || SceneManager.GetActiveScene().name == "Fireflies" || SceneManager.GetActiveScene().name == "Acid" || SceneManager.GetActiveScene().name == "Flower" || SceneManager.GetActiveScene().name == "Matrix")
                {
                    tran.PerformTransition("Visualisers");
                }

                else if (SceneManager.GetActiveScene().name == "Visualisers")
                {
                    SceneManager.LoadScene("Home");
                }
                else if (SceneManager.GetActiveScene().name == "Equaliser")
                {
                    SceneManager.LoadScene("Home");
                }
                else if (SceneManager.GetActiveScene().name == "Library")
                {
                    SceneManager.LoadScene("Home");
                }
                else if (SceneManager.GetActiveScene().name == "AudioSetup")
                {
                    SceneManager.LoadScene("Home");
                }
                else if (SceneManager.GetActiveScene().name == "HowToUseRAVE")
                {
                    SceneManager.LoadScene("Home");
                }
                else if (SceneManager.GetActiveScene().name == "Playlist")
                {
                    SceneManager.LoadScene("Library");
                }
            }
        }
    }

    public static Settings Instance
    {
        get { return instance; }
    }

    public void makeVisibleA()
    {
        //Popup
        aSettingsUI.SetActive(true);
        aSettingsVisible = true;
    }

    public void DismissA()
    {
        aSettingsUI.SetActive(false);
        aSettingsVisible = false;
    }
    public void makeVisibleV()
    {
        //Popup
        vSettingsUI.SetActive(true);
        vSettingsVisible = true;
    }

    public void DismissV()
    {
        vSettingsUI.SetActive(false);
        vSettingsVisible = false;
    }

    void makeVisibleE()
    {
        //Popup
        eSettingsUI.SetActive(true);
        eSettingsVisible = true;
    }

    public void DismissE()
    {
        eSettingsUI.SetActive(false);
        eSettingsVisible = false;
    }

    void makeVisibleI()
    {
        //Popup
        iSettingsUI.SetActive(true);
        iSettingsVisible = true;
    }

    public void DismissI()
    {
        iSettingsUI.SetActive(false);
        iSettingsVisible = false;
    }

    public void makeVisibleP()
    {
        //Popup
        playerUI.SetActive(true);
        playerVisible = true;
    }

    public void DismissP()
    {
        playerUI.SetActive(false);
        playerVisible = false;
    }

    public void makeVisibleS()
    {
        //Popup
        spotifyPlayerUI.SetActive(true);
        spotifyPlayerVisible = true;
    }

    public void DismissS()
    {
        spotifyPlayerUI.SetActive(false);
        spotifyPlayerVisible = false;
    }

    public void playSwap()
    {

        playerBTN.SetActive(!play);
        pauseBTN.SetActive(play);


        play = !play;

    }

    public void shuffleSwap()
    {

        notShuffleBTN.SetActive(!shuffle);
        shuffleBTN.SetActive(shuffle);
        shuffle = !shuffle;


    }

    public void loopSwap()
    {

        notLoopBTN.SetActive(loop);
        loopBTN.SetActive(!loop);
        loop = !loop;


    }

    public void muteSwap()
    {
        if (!mute)
        {
            muteBTN.SetActive(false);
            unMuteBTN.SetActive(true);
            prevVol = AudioListener.volume;
            AudioListener.volume = 0;
        }
        else
        {
            muteBTN.SetActive(true);
            unMuteBTN.SetActive(false);
            AudioListener.volume = prevVol;
        }
        mute = !mute;

    }
    public void noKeyboardSettingsSwitch()
    {

        noKeyboardSettings = !noKeyboardSettings;
        noKeyboardSettingsUI.SetActive(noKeyboardSettings);


    }
}


