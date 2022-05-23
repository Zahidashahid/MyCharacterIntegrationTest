using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameMaster gm;
    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") )
        {
            //Debug.Log("player check point");
            gm.lastCheckPointPos = transform.position;
            PlayerPrefs.SetFloat("LastcheckPointX", gm.lastCheckPointPos.x);
            PlayerPrefs.SetFloat("LastcheckPointy", gm.lastCheckPointPos.y);
            PlayerPrefs.SetInt("GemCollectedTillLastCheckPoint", PlayerPrefs.GetInt("RecentGemCollected"));
            PlayerPrefs.SetInt("CherryCollectedTillLastCheckPoint", PlayerPrefs.GetInt("RecentCherryCollected"));
            
            

        }
    }
}
