using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveDataToDB : MonoBehaviour
{
    SaveSystem saveSys;
    PauseGame pauseGameScript;
    public Scrollbar horizontalScrollbar;
    public TMP_InputField input;
    public string[] saveFiles;
    public GameObject loadButtonPrefab;
    public Transform loadArea;
    int numOfFilesInAslot = 5;
    void Start()
    {

        saveSys = FindObjectOfType<SaveSystem>();
        pauseGameScript = GameObject.Find("PauseGameCanvas").GetComponent<PauseGame>();
    }
    public void ShowLoadScreen()
    {
        GetLoadFiles();
        foreach(Transform button in loadArea)
        {
            Destroy(button.gameObject);
        }
        
        for (int i = 0; i < saveFiles.Length; i++)
        {
            GameObject buttonObj = Instantiate(loadButtonPrefab);
            buttonObj.transform.SetParent(loadArea.transform, false);
            Debug.Log("btn Instantiate");
            var index = i;
            /*buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
               saveSys.LoadPlayer(saveFiles[index]);
            });*/
            string _fName = saveFiles[index].Replace(Application.persistentDataPath + "/PlayerFiles/", "");
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = _fName.Replace(".txt",  "");
        }
    }
    void GetLoadFiles()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/PlayerFiles/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/PlayerFiles/");
        }
        saveFiles = Directory.GetFiles(Application.persistentDataPath + "/PlayerFiles/");
        horizontalScrollbar.GetComponentInChildren<Scrollbar>().numberOfSteps = saveFiles.Length / numOfFilesInAslot;
    }
    public void SaveNewFile()
    {
        pauseGameScript.Resume();
        input.text = input.text.Trim();
        SaveSystem.instance.playerData.fileName = input.text;

        SaveSystem.instance.playerData.lives = PlayerPrefs.GetInt("PlayerLives");
        SaveSystem.instance.playerData.avatarSelected = PlayerPrefs.GetInt("AvatarSelected");
        SaveSystem.instance.playerData.level = PlayerPrefs.GetString("CurrentLevel");
        SaveSystem.instance.playerData.gemPlayerHas = PlayerPrefs.GetInt("PlayerGem");
        SaveSystem.instance.playerData.cherryPlayerHas = PlayerPrefs.GetInt("PlayerCherry");

        SaveSystem.instance.playerData.lastCheckPointPos[0] = PlayerPrefs.GetFloat("lastCheckPointPosX");
        SaveSystem.instance.playerData.lastCheckPointPos[1] = PlayerPrefs.GetFloat("lastCheckPointPosY");

        SaveSystem.instance.playerData.health = PlayerPrefs.GetInt("PlayerHealth");
        SaveSystem.instance.playerData.numOfArrows = PlayerPrefs.GetInt("PlayerHasNumOfArrows");
        SaveSystem.instance.playerData.difficultyLevel = PlayerPrefs.GetString("difficultyLevel");
       




        saveSys.SaveNewPlayer(input.text);
        DeletePlayerPrefs();
    }
    void DeletePlayerPrefs()
    {

         PlayerPrefs.SetInt("PlayerLives", 0);
         PlayerPrefs.SetInt("AvatarSelected" ,0);
         PlayerPrefs.SetString("CurrentLevel", null);
         PlayerPrefs.SetInt("PlayerGem",0);
         PlayerPrefs.SetInt("PlayerCherry",0);

         PlayerPrefs.SetFloat("lastCheckPointPosX",0);
         PlayerPrefs.SetFloat("lastCheckPointPosY",0);
         PlayerPrefs.SetInt("PlayerHealth",0);
         PlayerPrefs.SetInt("PlayerHasNumOfArrows",0);
         PlayerPrefs.SetString("difficultyLevel", null);





        saveSys.SaveNewPlayer(input.text);
    }
    public void SaveSameFile()
    {
        PlayerPrefs.SetString("FileName", SaveSystem.instance.playerData.fileName);
        SaveSystem.instance.playerData.lives = PlayerPrefs.GetInt("PlayerLives");
        SaveSystem.instance.playerData.avatarSelected = PlayerPrefs.GetInt("AvatarSelected");
        SaveSystem.instance.playerData.level = PlayerPrefs.GetString("CurrentLevel");
        SaveSystem.instance.playerData.gemPlayerHas = PlayerPrefs.GetInt("PlayerGem");
        SaveSystem.instance.playerData.cherryPlayerHas = PlayerPrefs.GetInt("PlayerCherry");

        SaveSystem.instance.playerData.lastCheckPointPos[0] = PlayerPrefs.GetFloat("lastCheckPointPosX");
        SaveSystem.instance.playerData.lastCheckPointPos[1] = PlayerPrefs.GetFloat("lastCheckPointPosY");

        SaveSystem.instance.playerData.health = PlayerPrefs.GetInt("PlayerHealth");
        SaveSystem.instance.playerData.numOfArrows = PlayerPrefs.GetInt("PlayerHasNumOfArrows");
        SaveSystem.instance.playerData.difficultyLevel = PlayerPrefs.GetString("difficultyLevel");





        saveSys.SavePlayer();
        DeletePlayerPrefs();
    }
}
