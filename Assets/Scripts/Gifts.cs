/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;

public class Gifts : MonoBehaviour
{
    public static int gemAmount;//Total amount of gems collected after end of level 
    public static int gemCount;//Amount of gems in prefrences
    public static int cherryAmount;//Total amount of cherry collected after end of level 
    public static int cherryCount;//Amount of cherry in prefrences
    public ScoreManager scoreManager;
   // Collider collider;
    //public AudioSource giftSound;
    private void Start()
    {
        
       /* SaveSystem.instance.playerData.gemPlayerHas = PlayerPrefs.GetInt("GemCollectedTillLastCheckPoint");
        SaveSystem.instance.playerData.cherryPlayerHas = PlayerPrefs.GetInt("CherryCollectedTillLastCheckPoint");*/

        //collider = GetComponent<Collider>();
        cherryAmount = SaveSystem.instance.playerData.gemPlayerHas;
        gemAmount = SaveSystem.instance.playerData.cherryPlayerHas;
        /*   Debug.Log("gemAmount =" + SaveSystem.instance.playerData.gemPlayerHas);
           Debug.Log("cherryAmount =" + = SaveSystem.instance.playerData.cherry);
           Debug.Log("gift data fatched");*/

    }
    private void Update()
    {
        /*-------Why This-----------*/
       string currentLevel  = PlayerPrefs.GetString("CurrentLevel");
        //Debug.Log("Gift update" );
        if (currentLevel == "Level 1" || currentLevel == "Level 2")
        {
            PlayerPrefs.SetInt("GemCollectedTillLastCheckPoint", PlayerPrefs.GetInt("PlayerCGem"));
            PlayerPrefs.SetInt("CherryCollectedTillLastCheckPoint", PlayerPrefs.GetInt("PlayerCherry"));
        }
        /*------------------*/
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        
        if ( tag == "Cherry")
        {
            collision.enabled = false;
            Destroy(collision.gameObject);
            cherryCount += 1;
            cherryAmount += 1;
            Debug.Log("Amount Cherry " + cherryAmount);
            
            //PlayerPrefs.SetInt("RecentCherryCollected", cherryAmount);
            scoreManager.CherryCollect();
            SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.giftSound);
            
        }
        if ( tag == "Gem")
        {
            collision.enabled = false;
            Destroy(collision.gameObject);
            gemAmount += 1;
            gemCount += 1;
            Debug.Log("gem Amount " + gemAmount);
            
            //PlayerPrefs.SetInt("RecentGemCollected", gemAmount);
            scoreManager.GemCollect();
            SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.giftSound);
            
        }
        
    }
   
}
