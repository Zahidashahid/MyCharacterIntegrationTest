using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    //PlayerData data;
    public bool hasLoaded;
    string path;
    private void Awake()
    {
         path = Application.persistentDataPath + "/player.txt";
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
            LoadPlayer();
            DontDestroyOnLoad(gameObject);
        }

        if(hasLoaded)
        {
        }
    }
    public PlayerData playerData;
    private void Start()
    {
        //data = new PlayerData();
    }
    public void SavePlayer()
    {
        /*------Using binary system -----*/
        Debug.Log("Data saved\n "+path + " Path");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream;
         stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData();
        formatter.Serialize(stream, playerData);

        /*------Using XML file  system -----*/
        /*var serliaizer = new XmlSerializer(typeof(PlayerData));
        var stream = new FileStream(path, FileMode.Create);
        serliaizer.Serialize(stream, playerData); */


        stream.Close();
        Debug.Log("Saved");


    }
    public PlayerData LoadPlayer()
    {
        //string path = Application.persistentDataPath + "/player.dev";
        if (File.Exists(path))
        {
            /*------Using binary system -----*/
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            playerData = formatter.Deserialize(stream) as PlayerData;


            /*------Using XML file  system -----*/
            /*XmlSerializer serliaizer = new XmlSerializer(typeof(PlayerData));
            var stream = new FileStream(path, FileMode.Open);
            playerData = serliaizer.Deserialize(stream) as PlayerData; */
            Debug.Log("Load ");
            hasLoaded = true;
            stream.Close();
            return playerData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    void DeleteData()
    {
       // string path = Application.persistentDataPath + "/player.dev";
        if (File.Exists(path))
        {
            File.Delete(path);
        } 
    }
}


[System.Serializable]
public class PlayerData
{
    public string level;
    public string difficultyLevel;
    public int health;
    public int lives;
    public int numOfArrows;

    public int avatarSelected;
    public int cherryPlayerHas;
    public int gemPlayerHas;
    public float[] lastCheckPointPos;


    public PlayerData( )
     {
    
        lives = PlayerMovement.lives;
         health = PlayerMovement.currentHealth;
        avatarSelected = ChangeAvatar.avatarSelected;
         level = MainMenu.currentLevel;
         difficultyLevel = MainMenu.difficultyLevel;
         gemPlayerHas = GiftData.gemCount;
         cherryPlayerHas = GiftData.cherryCount;
        numOfArrows = ArrowStore.arrowPlayerHas;
        /*lastCheckPointPos = new float[2];
        lastCheckPointPos[0] = GameMaster.lastCheckPointPos.x;
        lastCheckPointPos[1] = GameMaster.lastCheckPointPos.y;*/


    }
}
