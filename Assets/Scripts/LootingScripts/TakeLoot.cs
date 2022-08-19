/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;

public class TakeLoot : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public  ArrowStore  arrowStoreScript;
    public HealthBar healthBar;
    int valueOfThisLoot;
    private void Start()
    {
      
        if ((PlayerPrefs.GetInt("AvatarSelected") == 1))
        {
            playerMovement = GameObject.Find("Player_Goblin").GetComponent<PlayerMovement>();
        }
        else
        {
            playerMovement = GameObject.Find("MushrromPlayer").GetComponent<PlayerMovement>();
        }
        healthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<HealthBar>();
         arrowStoreScript = GameObject.FindGameObjectWithTag("ArrowStore").GetComponent< ArrowStore>();
        if (this.tag == "HealthLoot")
        {
            valueOfThisLoot = 50;
            
        }
        else if (this.tag == "ArrowLoot")
        {
            valueOfThisLoot = 5;
        }
     }
    private void Update()
    {
       if ( valueOfThisLoot <= 0)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //if (Input.GetKey(KeyCode.L))
           // {
            if (this.tag == "HealthLoot")
            {
                Debug.Log(this.tag);
                int healthValuePlayerHas = SaveSystem.instance.playerData.health ;
                Debug.Log(healthValuePlayerHas);
                /*
                    ---------------------- Logic if player has full health  
                */
                Debug.Log(this.tag);
                if (healthValuePlayerHas >= playerMovement.maxHealth)
                {
                    return;
                }
                else
                {
                    int healthToAddInStore = playerMovement.maxHealth - healthValuePlayerHas;
                    if (healthToAddInStore <= valueOfThisLoot)
                    {
                        valueOfThisLoot = valueOfThisLoot - healthToAddInStore;
                        int healthCount = healthValuePlayerHas + healthToAddInStore;
                        Debug.Log("numOfhealth PlayerHas  " + healthValuePlayerHas);
                        Debug.Log("healthToAddInStore " + healthToAddInStore);
                        Debug.Log("valueOfThisLoot  left " + valueOfThisLoot);
                        Debug.Log(" new CurrentHealth" + healthCount);

                        PlayerMovement.currentHealth = playerMovement.maxHealth;
                        /*  SaveSystem.instance.playerData.health  =playerMovement.maxHealth;
                            SaveSystem.instance.playerData.health = healthCount;*/
                        PlayerPrefs.SetInt("PlayerHealth", playerMovement.maxHealth);
                        PlayerPrefs.SetInt("PlayerHealth", healthCount);
                        healthBar.SetHealth(healthCount);
                        SaveSystem.instance.SavePlayer();
                    }
                    else
                    {
                        int healthCount = healthValuePlayerHas + valueOfThisLoot;
                        /*SaveSystem.instance.playerData.health = healthCount;*/
                        PlayerPrefs.SetInt("PlayerHealth", healthCount);
                        healthBar.SetHealth(healthCount);
                        valueOfThisLoot = valueOfThisLoot - valueOfThisLoot;
                        SaveSystem.instance.SavePlayer();
                    }
                    if (healthToAddInStore == 50)
                    {

                        Destroy(gameObject);
                    }

                }

            }
            else if (this.tag == "ArrowLoot")
            {
                    int numOfArrowsPlayerHas = PlayerPrefs.GetInt("ArrowPlayerHas");
                    /*
                        ---------------------- Logic if player has Full arrow i.e store is full can't take the loots
                     */
                    Debug.Log(this.tag);
                    if (numOfArrowsPlayerHas >=  arrowStoreScript.maxNumOfArrow)
                    {
                        return;
                    }
                    else
                    {
                        int arrowsToAddInStore = arrowStoreScript.maxNumOfArrow - numOfArrowsPlayerHas;
                        if(arrowsToAddInStore <= valueOfThisLoot)
                        {
                            valueOfThisLoot = valueOfThisLoot - arrowsToAddInStore;
                            int arrowCount = numOfArrowsPlayerHas + arrowsToAddInStore;
                            Debug.Log("numOfArrowsPlayerHas  " + numOfArrowsPlayerHas);
                            Debug.Log("arrowsToAddInStore " + arrowsToAddInStore);
                            Debug.Log("valueOfThisLoot  left" + valueOfThisLoot);
                            Debug.Log(" new arrowCount" + arrowCount);
                             Debug.Log(" Arrow store in data "); 
                            /*SaveSystem.instance.playerData.numOfArrows = arrowCount;*/
                            PlayerPrefs.SetInt("PlayerHasNumOfArrows",  arrowCount);
                            ArrowStore.arrowPlayerHas = arrowCount;
                            arrowStoreScript.UpdateArrowText();
                           SaveSystem.instance.SavePlayer();
                        }
                        else
                        {
                            int arrowCount = numOfArrowsPlayerHas + valueOfThisLoot;
                             Debug.Log(" Arrow store in data "); 
                            /*SaveSystem.instance.playerData.numOfArrows = arrowCount;*/
                            PlayerPrefs.SetInt("PlayerHasNumOfArrows", arrowCount);
                            valueOfThisLoot = valueOfThisLoot - valueOfThisLoot;
                            ArrowStore.arrowPlayerHas = arrowCount;
                            arrowStoreScript.UpdateArrowText();
                           SaveSystem.instance.SavePlayer();
                        }
                        if (arrowsToAddInStore == valueOfThisLoot)
                        {
                            Destroy(gameObject);
                        }
                       
                    }
                    
            }
            else 
            {
                return;
            }


         
        }

    }
}
