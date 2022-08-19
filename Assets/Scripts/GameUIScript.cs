using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameUIScript : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject gameOverCanvas;
    public TMP_Text gameOverText;
    public GameObject restartButton;
    public GameObject SkeletonSpwan;
    public GameObject pauseMenuPanel;
    public GameObject EnemyEagleSpwan;
    public GameObject RangeAttackSpwan;
    public GameObject RangeAttackPointSpwan;
   
    public AudioSource restartBtnSound;
    public AudioSource bgSound;
    MainMenu mainMenu;
    //GameMaster GameMaster;
    PlayerMovement playerMovement;
    ArrowStore arrowStoreScript; 
    public ScoreManager scoreManager;
    PauseGame pauseGameScript;
    string difficultyLevel;
    public GameObject avatar1;
    public GameObject avatar2;
    public static bool isNewGame;

    void Awake()
    {
        /* gameOverPanel.SetActive(false);
         restartButton.SetActive(false);
        gameOverText.enabled = false;*/
      
        
    }

    public void Start()
    {
        /*gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        gameOverText.enabled = false;*/
        isNewGame = false;

        avatar1 = GameObject.Find("Player_Goblin");
        avatar2 = GameObject.Find("MushrromPlayer");
        pauseGameScript = GameObject.Find("PauseGameCanvas").GetComponent<PauseGame>();
        arrowStoreScript = GameObject.FindGameObjectWithTag("ArrowStore").GetComponent<ArrowStore>();
        

        bgSound = GameObject.FindGameObjectWithTag("BGmusicGameObject").GetComponent<AudioSource>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        mainMenu = GameObject.FindGameObjectWithTag("GM").GetComponent<MainMenu>();
        CheckForAvatarSelected();
        //GameMaster = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        if (SaveSystem.instance.playerData.level == "Level 3")
        {
            difficultyLevel = PlayerPrefs.GetString("difficultyLevel");
            //Debug.Log("Difficulity" + MainMenu.difficultyLevel);
            if (difficultyLevel == "Medium")
            {
                SkeletonSpwan.SetActive(true);
                // EnemyEagleSpwan.SetActive(true);
            }
            if (difficultyLevel == "Hard")
            {
                SkeletonSpwan.SetActive(true);
                //EnemyEagleSpwan.SetActive(true);
                RangeAttackPointSpwan.SetActive(true);
                RangeAttackSpwan.SetActive(true);
            }
        }
        //SaveSystem.instance.SavePlayer();
    }
    void CheckForAvatarSelected()
    {
        if (MainMenu.isNewGame || GameUIScript.isNewGame)
        {
            if ((PlayerPrefs.GetInt("AvatarSelected") == 1))
            {
                avatar1.SetActive(true);
                avatar2.SetActive(false);
            }

            else if ((PlayerPrefs.GetInt("AvatarSelected") == 2))
            {
                avatar2.SetActive(true);
                avatar1.SetActive(false);

            }

            Debug.Log("Avatar selected" + SaveSystem.instance.playerData.avatarSelected);
        }
        else
        {
            if ((SaveSystem.instance.playerData.avatarSelected == 2))
            {
                avatar2.SetActive(true);
                avatar1.SetActive(false);

            }
            else if ((SaveSystem.instance.playerData.avatarSelected == 1))
            {
                avatar1.SetActive(true);
                avatar2.SetActive(false);
            }
            Debug.Log("Avatar selected" + SaveSystem.instance.playerData.avatarSelected);

        }
    }
    public void GameOver()
    {
        StartCoroutine(waiter());
        //SceneManager.LoadScene("Level 1");
        //SceneManager.LoadScene("GameOver");
    }
    public void RestartGameFromLastCheckPoint() 
    {
        /*        gameOverPanel.SetActive(false);
                restartButton.SetActive(false);
                gameOverText.enabled = false;*/
        //SceneManager.LoadScene("Level 1");
        pauseGameScript.Resume();
        restartBtnSound.Play();
        isNewGame = false;
        pauseGameScript.isGamePaused = false;
        /*
        PlayerPrefs.SetInt("RecentGemCollected", 0);
        PlayerPrefs.SetInt("RecentCherryCollected", 0);
        PlayerPrefs.SetInt("ArrowPlayerHas",  arrowStoreScript.maxNumOfArrow);*/

        //Reset the last check point
        bgSound.Play();
        Time.timeScale = 1f;

        GameMaster.lastCheckPointPos[0] = PlayerPrefs.GetFloat("lastCheckPointPosX");
        GameMaster.lastCheckPointPos[1] = PlayerPrefs.GetFloat("lastCheckPointPosY");
        float x = PlayerPrefs.GetFloat("lastCheckPointPosX");
        float y = PlayerPrefs.GetFloat("lastCheckPointPosY");
        GameMaster.lastCheckPointPos = new Vector2(x, y);
        playerMovement.transform.position = GameMaster.lastCheckPointPos;
        Debug.Log(difficultyLevel);
        if (difficultyLevel == "Easy")
        {
            return;
        }
        else if (difficultyLevel == "Medium" || difficultyLevel == "Hard")
        {
            PlayerPrefs.SetInt("PlayerGem", PlayerPrefs.GetInt("GemCollectedTillLastCheckPoint"));
            PlayerPrefs.SetInt("PlayerCherry", PlayerPrefs.GetInt("CherryCollectedTillLastCheckPoint"));
           //SaveSystem.instance.SavePlayer();
        }
       
        // playerMovement.transform.position = GameMaster.lastCheckPointPos;


        scoreManager.UpdateCherryText(PlayerPrefs.GetInt("PlayerGem"));
        scoreManager.UpdateGemText(PlayerPrefs.GetInt("PlayerCherry"));



        playerMovement.transform.position = GameMaster.lastCheckPointPos;
        pauseMenuPanel.SetActive(false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RestartGame() //When Game is over after complete death i.e zero lives left
    {
        /*        gameOverPanel.SetActive(false);
                restartButton.SetActive(false);
                gameOverText.enabled = false;*/
        //SceneManager.LoadScene("Level 1");
        restartBtnSound.Play();
     
        ResetDataOfLastGame();

        bgSound.Play();
        Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameMaster.lastCheckPointPos = new Vector2(0, 0);
        string currentLevel = SaveSystem.instance.playerData.level;
        PlayerPrefs.SetInt("AvatarSelected", ChangeAvatar.avatarSelected);
        //SaveSystem.instance.SavePlayer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ResetDataOfLastGame()
    {
        MainMenu.isNewGame = true;
        isNewGame = true;

        playerMovement.ResetData();
       
        PlayerPrefs.SetInt("GemCollectedTillLastCheckPoint", 0);
        PlayerPrefs.SetInt("CherryCollectedTillLastCheckPoint", 0);
    }
    public void RestLastCheckPoint()
    {
        PlayerPrefs.SetFloat("lastCheckPointPosX",  -4);
        PlayerPrefs.SetFloat("lastCheckPointPosY",4);
       //SaveSystem.instance.SavePlayer();
    }
    public void RestartLevel()
    {
        pauseGameScript.Resume();
        playerMovement.Reset();
        ResetDataOfLastGame();

        //Reset Last check point
        string currentLevel = PlayerPrefs.GetString("CurrentLevel");
        SceneManager.LoadScene(currentLevel);
        Debug.Log(" RestartLevel() Called");
    }
    public void  Back()
    {
        SceneManager.LoadScene("Main Menu");
    }
    IEnumerator waiter()
    {
        //Wait for 1 seconds
       
        yield return new WaitForSeconds(0.001f);
        Debug.Log("Game is Over.");
        Time.timeScale = 0f;
        gameOverCanvas.SetActive(true);
        gameOverPanel.SetActive(true);
        restartButton.SetActive(true);
        gameOverText.enabled = true;
     
    }
 
}

