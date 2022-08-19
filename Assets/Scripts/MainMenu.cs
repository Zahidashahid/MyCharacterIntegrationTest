using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource onClickBtnSound;
    public AudioSource bgSound;
    public AudioSource level1Music;
    public AudioSource level2Music;
    public GameObject QuitGameMenuUI;
    public static string currentLevel;
    public static string levelReachedName;
    public static string difficultyLevel;
    public static bool isNewGame;
    public Button[] levelBtns;
    public Button newGameBtn;
    public Button levelInOnPlayBtn;
    public GameObject continueBtn;
    GameMaster gm;
    private void Awake()
    {
        isNewGame = true;
        
    }
    private void Start()
    {
       
        currentLevel = SaveSystem.instance.playerData.level; //If non of any levels played yet current level will be null i.e "" due to string
        difficultyLevel = SaveSystem.instance.playerData.difficultyLevel;
        onClickBtnSound = GameObject.FindGameObjectWithTag("SoundEffectGameObject").GetComponent<AudioSource>();
        bgSound = GameObject.FindGameObjectWithTag("BGmusicGameObject").GetComponent<AudioSource>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        /*
        -----------------Continue button disable if game is install for 1st time -----------------------------------
        */
        if (currentLevel == null || currentLevel == "")
        {
            continueBtn.SetActive(false);
            
            newGameBtn.transform.position = new Vector3(newGameBtn.transform.position.x + 0, newGameBtn.transform.position.y + 80);
            levelInOnPlayBtn.transform.position = new Vector3(levelInOnPlayBtn.transform.position.x + 0, levelInOnPlayBtn.transform.position.y + 80);
        }
        /*
         -----------------Level Lock Logic Start here -----------------------------------
         */


            /*
             -------------- Get Level Reached from data file of player  ---------------------------------
            */


            // levelReachedName = SaveSystem.instance.playerData.level;

            levelReachedName = "Level 3"; // to unlock all level , last level must be reached
            int levelReached = 3;
       
            switch (levelReachedName)
            {
                case "Level 1":
                    levelReached = 1;
                    break;
                case "Level 2":
                    levelReached = 2;
                    break;
                case "Level 3":
                    levelReached = 3;
                    break;

                default:

                    break;
            }
            Debug.Log("Level reached" + levelReachedName);
            for (int i = 0; i < levelBtns.Length; i++)
            {
                if (i + 1 > levelReached)
                {
                    levelBtns[i].interactable = false;
                }
            }
        /*
         -----------------Level Lock Logic ends here -----------------------------------
         */
    }
    public void OnPlayBtnclick()
    {
       
    }
    public void PlayGame()
    {
        OnBtnClickSound();

        difficultyLevel = SaveSystem.instance.playerData.difficultyLevel;
        isNewGame = false;
        if (currentLevel == null || currentLevel == "")
        {
            Debug.LogError("Contiune but current level is null");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            SceneManager.LoadScene(currentLevel);
    } 
    public void ContinueGame()
    {
        OnBtnClickSound();
        isNewGame = false;
        difficultyLevel = SaveSystem.instance.playerData.difficultyLevel;
        Debug.Log("current level you will continue is : " + currentLevel);
        if (currentLevel == null || currentLevel == "")
        {
            Debug.LogError("Contiune but current level is null");
           // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            SceneManager.LoadScene(currentLevel);
    } 
    
    public void NewGame()
    {
        OnBtnClickSound();
        isNewGame = true;
        difficultyLevel = SaveSystem.instance.playerData.difficultyLevel;


        if (currentLevel == null || currentLevel == "")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            currentLevel = "Level 1";
            PlayerPrefs.SetString("CurrentLevel", currentLevel);
            PlayerPrefs.SetString("difficultyLevel", "Easy");
            //SaveSystem.instance.SavePlayer();
        }

        else
        {
            SceneManager.LoadScene(currentLevel);
            isNewGame = true;
        }
            
    }

    public void Level1()
    {
        PlayerPrefs.SetString("MultiplayerGame", "False");
        OnBtnClickSound();
        PlayerPrefs.SetString("CurrentLevel", "Level 1");
        //SaveSystem.instance.SavePlayer();
        isNewGame = false;
        // SceneManager.LoadScene("Level 1");
    }
    public void Level2()
    {
        PlayerPrefs.SetString("MultiplayerGame", "False");
        OnBtnClickSound();
        PlayerPrefs.SetString("CurrentLevel", "Level 2");
       //SaveSystem.instance.SavePlayer();
        isNewGame = false;
        // SceneManager.LoadScene("Level 2");
    }
    public void Level3()
    {
        PlayerPrefs.SetString("MultiplayerGame", "False");
        OnBtnClickSound();
        PlayerPrefs.SetString("CurrentLevel",  "Level 3");
        //SaveSystem.instance.playerData.level =  "Level 3";
        //SaveSystem.instance.SavePlayer();
        isNewGame = false;
        /// SceneManager.LoadScene("Level 3");
    }
    
    public void QuitGameMenu()
    {
        QuitGameMenuUI.SetActive(true);
    }
    public void QuitGame()
    {
        OnBtnClickSound();
        Application.Quit();
        QuitGameMenuUI.SetActive(false);
        Debug.Log("Game Quiting");
    }
    public void NotQuitGame()
    {
        QuitGameMenuUI.SetActive(false);
        Debug.Log("Game Not  Quiting");
    }
    public void OnBtnClickSound()
    {
        onClickBtnSound.Play();
    }
    public void MultiplayerGame()
    {
        PlayerPrefs.SetString("MultiplayerGame", "True");
        bgSound.clip = level2Music.clip;
        bgSound.Play();
        SceneManager.LoadScene("Multiplayer");
    }
    public void CheckLevel()
    {
        isNewGame = true;
        currentLevel = PlayerPrefs.GetString("CurrentLevel");
        switch (currentLevel)
        {
            case "Level 1":
                bgSound.clip = level1Music.clip;
                bgSound.Play();
                SceneManager.LoadScene(currentLevel);
                break;
            case "Level 2":

                bgSound.clip = level2Music.clip;
                bgSound.Play();
                SceneManager.LoadScene(currentLevel);
                break;
            case "Level 3":
                bgSound.clip = level2Music.clip;
                bgSound.Play();
                SceneManager.LoadScene(currentLevel);
                break;  
            case "Boss Enemy":
                bgSound.clip = level2Music.clip;
                bgSound.Play();
                SceneManager.LoadScene(currentLevel);
                break;

            default:
                Debug.Log("Not level in selection");
                break;
            }
    }
    public void Easy()
    {
        difficultyLevel = "Easy";
        PlayerPrefs.SetString("difficultyLevel", "Easy");
       //SaveSystem.instance.SavePlayer();
        NewGameStrat();
        CheckLevel();
    }
    public void Medium()
    {
        difficultyLevel = "Medium";
        NewGameStrat();
        PlayerPrefs.SetString("difficultyLevel", "Medium");
       //SaveSystem.instance.SavePlayer();
        CheckLevel();
    }
    public void Hard()
    {
        difficultyLevel = "Hard";
        NewGameStrat();
        PlayerPrefs.SetString("difficultyLevel" , "Hard");
        //SaveSystem.instance.SavePlayer();
        //gm.lastCheckPointPos = null;
        CheckLevel();
    }
     void NewGameStrat()
     {
        Debug.Log(" NewGameStrat()");
        PlayerPrefs.SetInt("PlayerGem" , 0);
        PlayerPrefs.SetInt("PlayerCherry" ,0 );
        PlayerPrefs.SetInt("PlayerHealth", 100);
        PlayerPrefs.SetInt("PlayerLives" ,3);
       //SaveSystem.instance.SavePlayer();

       
        //PlayerPrefs.SetInt("ArrowPlayerHas", 10);
        PlayerPrefs.SetInt("RecentGemCollected", 0);
        PlayerPrefs.SetInt("RecentCherryCollected", 0);
        PlayerPrefs.SetInt("GemCollectedTillLastCheckPoint", 0);
        PlayerPrefs.SetInt("CherryCollectedTillLastCheckPoint",0);
        isNewGame = false;
    }
}
