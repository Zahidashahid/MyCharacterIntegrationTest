using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rigidbody2D;

    public GameUIScript gameUIScript;
    PlayerMovement playerMovement;
    Enemies enemies;
    bool isColliding;
    bool hitByEnemy;
    void Awake()
    {
       
    }
    void Start()
    {
        // animator = GetComponent<Animator>();
        //Debug.Log(playerMovement.lives + "lives left");
        if ((SaveSystem.instance.playerData.avatarSelected) == 1)
        {
            playerMovement = GameObject.Find("Player_Goblin").GetComponent<PlayerMovement>();
            animator = GameObject.Find("Player_Goblin").GetComponent<Animator>();
            rigidbody2D = GameObject.Find("Player_Goblin").GetComponent<Rigidbody2D>();
        }
        else
        {
            playerMovement = GameObject.Find("MushrromPlayer").GetComponent<PlayerMovement>();
            animator = GameObject.Find("MushrromPlayer").GetComponent<Animator>();
            rigidbody2D = GameObject.Find("MushrromPlayer").GetComponent<Rigidbody2D>();
        }
       
        gameUIScript = GameObject.Find("GameManager").GetComponent<GameUIScript>();
        enemies = GetComponentInChildren<Enemies>();
        hitByEnemy = false;
    }
    private void FixedUpdate()
    {
       // isColliding = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(this.tag + "  hit " + collision.tag);
        if (!hitByEnemy)
        {
            GameObject collisionGameObject = collision.gameObject;

            if (collision.tag == "Player")
            {
                hitByEnemy = true;
                Debug.Log(this.tag + " hit " + collision.tag);
                Debug.Log( " playerMovement.lives  "  + PlayerMovement.lives);
               
                if (PlayerMovement.lives <= 1)
                {
                    // bgSound.Stop();
                    SaveSystem.instance.playerData.health = playerMovement.maxHealth;
                    SaveSystem.instance.playerData.lives = 3;
                    SaveSystem.instance.SavePlayer();
                    SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.deathSound);
                    
                    StartCoroutine(playerMovement.Die());
                    this.enabled = false;
                }
                else
                {
                    //playerMovement.lives = playerMovement.lives - 1;
                    StartCoroutine(playerMovement.OnOneDeath());
                }
                StartCoroutine(Reset());
                
            }
        }
    }
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.3f);
        
       if(this.CompareTag("Enemy"))
       {
           Destroy(gameObject);
       }
        hitByEnemy = false;
    }
    
   
}
