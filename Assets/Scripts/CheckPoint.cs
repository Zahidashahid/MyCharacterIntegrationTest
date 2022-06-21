
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //private GameMaster gm;
    PlayerMovement playerMovement;
    private void Start()
    {
        //gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") )
        {
            Debug.Log("player check point");
            Debug.Log("transform.position.x " + transform.position.x + "\n transform.position.y "+ transform.position.y);
            GameMaster.lastCheckPointPos[0] = transform.position.x;
            GameMaster.lastCheckPointPos[1] = transform.position.y;
            SaveSystem.instance.playerData.lastCheckPointPos[0] = transform.position.x;
            SaveSystem.instance.playerData.lastCheckPointPos[1] = transform.position.y;

            PlayerPrefs.SetInt("GemCollectedTillLastCheckPoint", SaveSystem.instance.playerData.gemPlayerHas);
            PlayerPrefs.SetInt("CherryCollectedTillLastCheckPoint", SaveSystem.instance.playerData.cherryPlayerHas);

           SaveSystem.instance.SavePlayer();
            

        }
    }
}
