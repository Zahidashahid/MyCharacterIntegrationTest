using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


using System;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    //PlayerData data;
    //public Button saveBtnClick;
    
    public bool hasLoaded;
    //public TMP_InputField.SubmitEvent se;
    string path;
    string fileName = "zee";

    private void Awake()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/PlayerFiles/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/PlayerFiles/");
        }
        path = Application.persistentDataPath + "/PlayerFiles/" + fileName +".txt";


        
        if (!File.Exists(path))
            SavePlayer();
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (File.Exists(path))
                LoadPlayer();
        }

        if(hasLoaded)
        {
        }
    }
    public PlayerData playerData;
    private void Start()
    {
        //data = new PlayerData();
        //saveBtnClick.onClick.AddListener(() => SaveNewPlayer());
        /* se = new TMP_InputField.SubmitEvent();
         se.AddListener(SaveNewPlayer);
         input.onEndEdit = se;*/
    }


    public void SavePlayer()
    {
        /*------Using binary system -----*/
        if (!Directory.Exists(Application.persistentDataPath + "/PlayerFiles/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/PlayerFiles/");
        }
        Debug.Log("Data saved\n "+path + " Path");
        BinaryFormatter formatter = GetBinaryFormatter();
        
        FileStream stream;
        stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData();
        formatter.Serialize(stream, playerData);

        /*------Using XML file  system -----*/
       /* var serliaizer = new XmlSerializer(typeof(PlayerData));
        var stream = new FileStream(path, FileMode.Create);
        serliaizer.Serialize(stream, playerData); 
        stream.Close();*/
        Debug.Log("Saved same");
    }

    public void SaveNewPlayer(string fileName)
    {
        Debug.Log("btn name " +  name);
        
        if (!String.IsNullOrEmpty(fileName))
        {

            Debug.Log("SaveNewPlayer() called");
            /*------Using binary system -----*/
            if (!Directory.Exists(Application.persistentDataPath + "/PlayerFiles/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/PlayerFiles/");
            }
            path = Application.persistentDataPath + "/PlayerFiles/" + fileName + ".txt";
            Debug.Log("Data saved\n " + path + " Path");
            PlayerData data = new PlayerData( );
            /* PlayerData data = new PlayerData(fileName,
                PlayerPrefs.GetInt("PlayerLives"), PlayerPrefs.GetInt("AvatarSelected"),
                PlayerPrefs.GetString("CurrentLevel"), PlayerPrefs.GetInt("PlayerGem"), PlayerPrefs.GetInt("PlayerCherry"),
                PlayerPrefs.GetFloat("lastCheckPointPosX"), PlayerPrefs.GetFloat("lastCheckPointPosY"),
                PlayerPrefs.GetInt("PlayerHealth"), PlayerPrefs.GetInt("PlayerHasNumOfArrows"), PlayerPrefs.GetString("difficultyLevel")
                );*/
            BinaryFormatter formatter = GetBinaryFormatter();
            FileStream stream;
            stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, playerData);

            /*------Using XML file  system -----*/
         /*   var serliaizer = new XmlSerializer(typeof(PlayerData));
            var stream = new FileStream(path, FileMode.Create);
            serliaizer.Serialize(stream, playerData); */


            stream.Close();
            Debug.Log("Saved");
            LoadPlayer();

        }
        else 
        {
            Debug.Log("File name is null " + fileName);
        }
    }
    public PlayerData LoadPlayer()
    {
        if (!File.Exists(path))
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
        else
        {
            /*------Using binary system -----*/
            BinaryFormatter formatter = GetBinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            try
            {
                playerData = formatter.Deserialize(stream) as PlayerData;


                /*------Using XML file  system -----*/
                /*    XmlSerializer serliaizer = new XmlSerializer(typeof(PlayerData));
                    var stream = new FileStream(path, FileMode.Open);
                    playerData = serliaizer.Deserialize(stream) as PlayerData; */
                Debug.Log("Path " + path);
                Debug.Log("Load ");
                Debug.Log(" playerData.difficultyLevel " + playerData.difficultyLevel + "1: " + playerData.level + "2: " + playerData.lives +
                    "3: " + playerData.health +"4: " + playerData.numOfArrows +"5: " + playerData.fileName +"6: " + playerData.avatarSelected +
                    "7: " + playerData.cherryPlayerHas + " playerData.gemPlayerHas " + playerData.gemPlayerHas);
                hasLoaded = true;
                stream.Close();
                return playerData;
            }
            catch
            {
                Debug.LogErrorFormat("Failed to load file at {0} " + path);
                stream.Close();
                return null;
            }
            
        }
        
        
    }
    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        return formatter;
             
    }
    void DeleteData()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        } 
    }
}


[System.Serializable]
public class PlayerData
{
    public string fileName;
    public string level;
    public string difficultyLevel;
    public int health;
    public int lives;
    public int numOfArrows;

    public int avatarSelected;
    public int cherryPlayerHas;
    public int gemPlayerHas;
    public float[] lastCheckPointPos;
   /*public PlayerData()
    {
        lastCheckPointPos = new float[2];
    }*/
  /* public PlayerData(int live,int avatar,string clevel, int gem,int cherry, float posX, float posY, int phealth ,int numOfArrow,String diffculty) 
   {
        lives = live;
        avatarSelected = avatar;
        level = clevel;
        gemPlayerHas = gem;
        cherryPlayerHas = cherry;

        lastCheckPointPos[0] = posX;
        lastCheckPointPos[1] = posY;

        health = phealth;
        numOfArrows = numOfArrow;
        difficultyLevel = diffculty;
    }*/
   
    /*public PlayerData(string filename ,int live, int avatar, string clevel, int gem, int cherry, float posX, float posY, int phealth, int numOfArrow, String diffculty)
     {
        Debug.Log("Override constructor called" );
         fileName = filename;

         lives = live;
         avatarSelected = avatar;
         level = clevel;
         gemPlayerHas = gem;
         cherryPlayerHas = cherry;

         lastCheckPointPos[0] = posX;
         lastCheckPointPos[1] = posY;

         health = phealth;
         numOfArrows = numOfArrow;
         difficultyLevel = diffculty;
        Debug.Log( filename+ " ---" + live + "---" + avatar + "---" +
                clevel + "---" +gem + "---" + cherry+ "---" +
               posX + "---" + posY + "---" +
                phealth + "---" + numOfArrow + "---" + diffculty
               );
        
        
        Debug.Log( filename+ " ---" + PlayerPrefs.GetInt("PlayerLives") + "---" + PlayerPrefs.GetInt("AvatarSelected") + "---" +
                PlayerPrefs.GetString("CurrentLevel") + "---" + PlayerPrefs.GetInt("PlayerGem") + "---" + PlayerPrefs.GetInt("PlayerCherry") + "---" +
                PlayerPrefs.GetFloat("lastCheckPointPosX") + "---" + PlayerPrefs.GetFloat("lastCheckPointPosY") + "---" +
                PlayerPrefs.GetInt("PlayerHealth") + "---" + PlayerPrefs.GetInt("PlayerHasNumOfArrows") + "---" + PlayerPrefs.GetString("difficultyLevel")
               );
        Debug.Log(filename + " ---" + lives + "---" + avatarSelected + "---" +
                level + "---" + gemPlayerHas + "---" + cherryPlayerHas + "---" +
                lastCheckPointPos[0] + "---" + lastCheckPointPos[1] + "---" +
                health + "---" + numOfArrows + "---" + difficultyLevel
               );


        
    }*/




}

