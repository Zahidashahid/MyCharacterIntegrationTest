using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using TMPro;
public class PlayerMovement : MonoBehaviour
{
    PlayerController controls;
    Vector3 move;
    bool isShieldBtnPressed;
    Vector3 m;
    public HealthBar healthBar;
    public CharacterController2D controller;
    [SerializeField] public LayerMask m_WhatIsGround;
    public Rigidbody2D rb;
    private BoxCollider2D boxCollider2d;
   
    public Animator animator;
    public Transform transformObj;
    //public Animator eagle_animator;
    //public Animator skeleton_animator;
    /*public AudioSource jumpSound;
    public AudioSource DeathSound;
    public AudioSource meleeAttackSound;*/
    public AudioSource bgSound;
   // bool jump ;
    //bool crouch = false;
    bool grounded;
  public  int jumpCount = 0;
    public int direction = 2;
    public int currentHealth;
    public int maxHealth = 100;
    public int lifes ;
    public int numberOfDamgeTake ;

    public bool activeShield;

    //private float dashTime = 40f;
    public float attackRange = 0.5f;
    public float attackRate = 1f; //one attack per second
    public float nextAttackTime = 0f;
    float runSpeed = 6f;
    float dashMoveSpeed = 9f;
    float jumpHight = 7f;
    //float horizontalMove = 0f;

    public Transform attackPoint;
    public Transform weaponAttackPoint;
    public LayerMask enemyLayers;

    //private Shield shield; //Player Shield
    private GameObject bodyParts;
    private GameObject weapon;
    private GameMaster gm;
    public TMP_Text lifesText;
    GameUIScript gameUIScript;
   
    private void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        lifes = 3;
        controls = new PlayerController();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Move.canceled += ctx => StopMoving();
        controls.Gameplay.Move.canceled += ctx => SetActiveBodyParts();
      /*  controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;*/
        controls.Gameplay.Jump.performed += ctx => JumpPlayer();
        controls.Gameplay.Shield.performed += ctx => isShieldBtnPressed = ctx.ReadValueAsButton();
        controls.Gameplay.Shield.canceled += ctx => isShieldBtnPressed = false;
        controls.Gameplay.Shield.canceled += ctx => SetActiveBodyParts();
        controls.Gameplay.MelleAttackSinglePlayer.performed += ctx   => MelleAttack();
        controls.Gameplay.MelleAttackSinglePlayer.canceled += ctx   => SetActiveBodyParts();
        
        controls.Gameplay.DashMove.performed +=ctx   => DashMovePlayer();
        //bgSound.Play();
    }
    private void Start()
    {
        numberOfDamgeTake = 0;
        CheckForAwatarSelected();
        bgSound = GameObject.FindGameObjectWithTag("BGmusicGameObject").GetComponent<AudioSource>();
        bodyParts = GameObject.FindGameObjectWithTag("BodyParts");
        weapon = GameObject.FindGameObjectWithTag("WeaponSprite");
        //Eagle_animator = GameObject.FindGameObjectWithTag("Enemy").transform<Animator>();
        animator = GetComponent<Animator>(); 
        currentHealth = maxHealth;
        lifes = PlayerPrefs.GetInt("Lifes");
        currentHealth = PlayerPrefs.GetInt("CurrentHealth");
        lifesText.text = "X " + lifes;
        /*Debug.Log("current health of player is " + currentHealth);
        Debug.Log("Max health of player is " + maxHealth);*/
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        grounded = true;
        // bgSound.Play();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        Debug.Log("gm.lastCheckPointPos "+ gm.lastCheckPointPos  + gm.lastCheckPointPos);
        Debug.Log("Num of Level completed " + PlayerPrefs.GetInt("LevelCompleted"));
        if (PlayerPrefs.GetString("DifficultyLevel") == "Hard")
        {
            lifes = 1;
            lifesText.text = "X " + lifes;
        }
        else if (PlayerPrefs.GetString("DifficultyLevel")== "Medium")
        {
            lifes = 2;
            lifesText.text = "X " + lifes;
        }
        else if (PlayerPrefs.GetString("DifficultyLevel")=="Easy")
        {
            lifes = 3;
            lifesText.text = "X " + lifes;
        }
        if (MainMenu.isNewGame || GameUIScript.isNewGame || (PlayerPrefs.GetInt("LevelCompleted") == 1)) 
        {
            Debug.Log("New Game Started");
            /*------------Reset Gift collected---------------------*/
            PlayerPrefs.SetInt("RecentGemCollected", 0);
            PlayerPrefs.SetInt("RecentCherryCollected", 0);
            PlayerPrefs.SetInt("GemCollectedTillLastCheckPoint", 0);
            PlayerPrefs.SetInt("CherryCollectedTillLastCheckPoint", 0);
            /*-------------Reset arrow Store----------------------*/
            PlayerPrefs.SetInt("ArrowPlayerHas", 10);
            PlayerPrefs.SetInt("CurrentHealth", 100);
            /* -------- Set last check point zero when game restarted-----------*/
            gm.lastCheckPointPos = transformObj.position; 
            PlayerPrefs.SetFloat("LastcheckPointX", transformObj.position.x);
            PlayerPrefs.SetFloat("LastcheckPointy", transformObj.position.y);
        }
        else
        {
            Debug.Log(" Game Continue");
            float x = PlayerPrefs.GetFloat("LastcheckPointX");
            float y = PlayerPrefs.GetFloat("LastcheckPointy");
            gm.lastCheckPointPos = new Vector2( x, y);
            transformObj.position = gm.lastCheckPointPos;
        }
            
    }
    private void Update()
    {
        string currentLevel = PlayerPrefs.GetString("CurrentLevel");
        if(currentLevel == "Level 1")
        {
             PlayerPrefs.SetFloat("LastcheckPointX" , transformObj.position.x);
             PlayerPrefs.SetFloat("LastcheckPointy", transformObj.position.y);
        }
    }
    private void FixedUpdate()
    {
        // Debug.Log("Is Grounded! "+ grounded);
        // Move Player back
        CheckGamePaused();
        // m = new Vector3(move.x, move.y)  * 10f *  Time.deltaTime;
        /*//Debug.Log(" move.x " + move.x);
          Debug.Log(" move.y " + move.y);
        Debug.Log(" move.z " + move.z);*/
        if (move.x > 0)
        {
            MovePlayerRight();
        }
        else if (move.x < 0)
        {
            MoveplayerLeft();
        }
      
        if ( isShieldBtnPressed )
        {
            //Debug.Log("SetShield Called");
            SetShield();
        }
        else
        {
            //Debug.Log("DisableShield Called");
            DisableShield();
        }
        /* if ( (move.y > 0 && move.x < 0 ) || (move.y > 0 && move.x > 0))
        {
            JumpPlayer();
        }*/
        /*animator.SetFloat("Speed", Mathf.Abs(40));
        transformObj.Translate(m, Space.World);*/
        // MovePlayer();
        // MelleAttack();

    }
   void StopMoving()
    {
        if (move.x == 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(0));
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
   void MovePlayerRight()
   {
        DisableBodyParts();
        direction = 2;
        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
        Flip();
        animator.SetFloat("Speed", Mathf.Abs(40));
   }
    void MoveplayerLeft()
    {
        DisableBodyParts();
        direction = 1;
        rb.velocity = new Vector2(-runSpeed, rb.velocity.y);
        Flip();
        animator.SetFloat("Speed", Mathf.Abs(40));
    }
    private void Flip()
    {
        // Rotate the player
        if (transform.localEulerAngles.y != 180 && direction == 1)
            transform.Rotate(0f, 180f, 0f);
        else if(transform.localEulerAngles.y != 0 && direction == 2)
                transform.Rotate(0f, -180f, 0f);


        // player flip point of attck also flip is direction
        //transform.Rotate(0f, 180f, 0f);
    }
    public void PlayerChangeDirection()
    {
        if ( direction == 1)
        {
            direction = 2;
            Flip();
        }
        else if ( direction == 2)
        {
            direction = 1;
            Flip();
        }
    }
    void JumpPlayer()
    {
        if (jumpCount < 2 || IsGrounded())
        {
            jumpCount++;
            grounded = false;
            rb.velocity = new Vector2(1, jumpHight);
            animator.SetBool("IsJumping", true);
           /* Debug.Log(" jump count is " + jumpCount);
            Debug.Log(" IsGrounded() is " + IsGrounded());*/
            SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.jumpSound);
            grounded = false;
            if (direction == 1)
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
            }
            else if (direction == 2)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
            }
            if (jumpCount > 2)
            {
                jumpCount = 0;
            }
        }
        else
        {
            grounded = true;
            animator.SetBool("IsJumping", false);
        }
    }
    void DashMovePlayer()
    {
        
        Debug.Log("DashMovePlayer() Grounded " + grounded);
        if (grounded)
        {
            //rb.velocity = new Vector2(5, rb.velocity.y);
            //animator.SetBool("IsJumping", true);

            Debug.Log("direction " + direction);
            if (direction == 1)
            {
               //rb.velocity =  Vector2.left * 8;
                rb.velocity = new Vector2(-dashMoveSpeed, rb.velocity.y);
                //transformObj.localScale = new Vector2(-1, 1);
                
            }
            else if (direction == 2)
            {
               // rb.velocity =  Vector2.right * 8;
                rb.velocity = new Vector2(dashMoveSpeed, rb.velocity.y);
                //transformObj.localScale = new Vector2(1, 1);
                //animator.SetFloat("Speed", Mathf.Abs(40));
            }

        }
        
    }
    
    void MovePlayer()
    {
        
        /*  if (Input.GetKey(KeyCode.LeftArrow))// && grounded
          {
              rb.velocity = new Vector2(-runSpeed, rb.velocity.y);
              transform.localScale = new Vector2(-1, 1);
              animator.SetFloat("Speed", Mathf.Abs(40));
              direction = 1;
          }
          // Move Player Forward
          else if (Input.GetKey(KeyCode.RightArrow))//&& grounded
          {
              rb.velocity = new Vector2(runSpeed, rb.velocity.y);
              transform.localScale = new Vector2(1, 1);
              animator.SetFloat("Speed", Mathf.Abs(40));
              direction = 2;
          }
          else
          {
              animator.SetFloat("Speed", Mathf.Abs(0));
          }
          // Jump Player if on ground ,  double jump
          if ((jumpCount < 2 || IsGrounded()) && (Input.GetKeyDown(KeyCode.Space)))
          {
              jumpCount++;
              grounded = false;
              //rb.velocity = new Vector2(rb.velocity.x, 11f);
              rb.velocity = new Vector2(rb.velocity.x, 10f);
              animator.SetBool("IsJumping", true);
              //Debug.Log(" jump count is " + jumpCount);
              SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.jumpSound);
              animator.SetFloat("Speed", Mathf.Abs(40));
              if (direction == 1)
              {
                  rb.velocity = new Vector2(-5, rb.velocity.y);
              }
              else if (direction == 2)
              {
                  rb.velocity = new Vector2(5, rb.velocity.y);
              }
              animator.SetFloat("Speed", Mathf.Abs(40));
              if (jumpCount > 2)
              {
                  jumpCount = 0;
              }
          }
          else
          {
              if (Input.GetKeyUp(KeyCode.Space))   //when  Space key is up. 
              {
                  rb.velocity = new Vector2(rb.velocity.x, 0f);
                  animator.SetBool("IsJumping", false);
                  jump = true;

              }
          }
          // Dash move 
          if (Input.GetKey(KeyCode.P) && grounded)
          {
              animator.SetFloat("Speed", Mathf.Abs(40));
              if (direction == 1)
              {
                  rb.velocity = new Vector2(-10, rb.velocity.y);
              }
              else if (direction == 2)
              {
                  rb.velocity = new Vector2(10, rb.velocity.y);
              }
          }*/
    }
    void SetShield()
    {
        DisableBodyParts();
        activeShield = true;
        animator.SetBool("Sheild", true);
    }
    void DisableShield()
    {
        //SetActiveBodyParts();
        activeShield = false;
        animator.SetBool("Sheild", false);
    }
    void MelleAttack()
    {
        Debug.Log(Time.deltaTime + " ||| " + nextAttackTime); 
        if (Time.time >= nextAttackTime)
        {
            DisableBodyParts();
            StartCoroutine(Attack());
           
            //Attack();
            nextAttackTime = Time.time + 1f / attackRate;
            /*
              ----------Melle Attack throgh keyboard
             */
            /*  if (Input.GetKeyDown(KeyCode.K))
              {
                  //Debug.Log("attack Called");

                  //eagle_animator.SetTrigger("Death");
                  StartCoroutine(Attack());
                  //Attack();
                  nextAttackTime = Time.time + 1f / attackRate;

              }*/
        }
    }
    public void OnLanding()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
       // Debug.Log("In OnLanding method");
        animator.SetBool("IsJumping", false);
        grounded = true;
        jumpCount = 0;
    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, m_WhatIsGround);
       // Debug.Log(raycastHit2d.collider);
        return raycastHit2d.collider != null;
    }
    /* <summary>
       ------------Attack on skeleton enemy--------------------------------------
    </summary> */
    IEnumerator Attack() //Melle Attack by player
    {
        animator.SetBool("Attack 2", true);
        Debug.Log("Attacking ");
        yield return new WaitForSeconds(0.5f);
       
        SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.meleeAttackSound);
        animator.SetBool("Attack 2", false);
        string difficultyLevel = PlayerPrefs.GetString("DifficultyLevel");
        //Deteck enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(weaponAttackPoint.position, attackRange, enemyLayers);
        //damage Them
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            if (enemy.name == "Skeleton" || enemy.tag == "Skeleton")
            {
                
                if (difficultyLevel == "Easy")
                {
                    enemy.GetComponent<SkeletonEnemyMovement>().TakeDamage(40);
                }
                else if (difficultyLevel == "Medium")
                {
                    enemy.GetComponent<SkeletonEnemyMovement>().TakeDamage(30);
                }
                else if (difficultyLevel == "Hard")
                {
                    enemy.GetComponent<SkeletonEnemyMovement>().TakeDamage(10);
                }
              /*  enemy.GetComponent<SkeletonEnemyMovement>().StartCoroutine(SkeletonHurtAnimation());*/

            }
            //eagle_animator.SetTrigger("Death");
            // yield return new WaitForSeconds(1);
            else if (enemy.name == "Range Attack Skeleton" || enemy.tag == "RangedAttackSkeleton")
            {

                if (difficultyLevel == "Easy")
                {
                    enemy.GetComponent<SkeletonRangeAttackMovement>().TakeDamage(40);
                }
                else if (difficultyLevel == "Medium")
                {
                    enemy.GetComponent<SkeletonRangeAttackMovement>().TakeDamage(30);
                }
                else if (difficultyLevel == "Hard")
                {
                    enemy.GetComponent<SkeletonRangeAttackMovement>().TakeDamage(10);
                }
               /* enemy.GetComponent<SkeletonRangeAttackMovement>().StartCoroutine(SkeletonSheildtAnimation());
                enemy.GetComponent<SkeletonRangeAttackMovement>().StartCoroutine(RangeAttackSkeletonHurtAnimation());*/
            }
            else   
                break;
        }
    }
    /*-------------Show Attack point oject in scene for better Visibility--------------------*/
    public void SetActiveBodyParts()
    {
        Debug.Log("Setting Active");
        bodyParts.SetActive (true);
        weapon.SetActive (true);
    }
    public void DisableBodyParts()
    {
        Debug.Log("Diable ");
        bodyParts.SetActive(false);
        weapon.SetActive(false);
    }
    public void TakeDamage(int damage)
    {
        if(currentHealth > 0 && lifes > 0)
        {
            
            if (numberOfDamgeTake > 3)
                StartCoroutine(SheildTimer());
            if (!(animator.GetBool("Sheild")))
            {
                currentHealth -= damage;
                PlayerPrefs.SetInt("CurrentHealth", currentHealth);
                healthBar.SetHealth(currentHealth);
                // DisableBodyParts();

                if (currentHealth > 0.01)
                    StartCoroutine(Hurt());
                //SetActiveBodyParts();
                // play hurt animation
                // StartCoroutine(HurtAnimation());

                if (currentHealth <= 0 && lifes <= 1)
                {
                    // bgSound.Stop();
                    PlayerPrefs.SetInt("CurrentHealth", 100);
                    PlayerPrefs.SetInt("Lifes", 3);
                    // Reset gifts collected and last check point

                    SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.deathSound);
                    FindObjectOfType<GameUIScript>().ResetDataOfLastGame();
                    FindObjectOfType<GameUIScript>().RestLastCheckPoint();
                    StartCoroutine(Die());
                    this.enabled = false;
                }
                else if (currentHealth <= 0 && lifes > 1)
                {
                    StartCoroutine(OnOneDeath());
                    PlayerPrefs.SetInt("CurrentHealth", 100);
                    lifes -= 1;
                    lifesText.text = "X " + lifes;
                    PlayerPrefs.SetInt("Lifes", lifes);
                }

            }
            else
                numberOfDamgeTake += 1;

        }
        
    }
    IEnumerator SheildTimer()
    {
        activeShield = false;
        animator.SetBool("Sheild", false);
        yield return new WaitForSeconds(5f);
        activeShield = true;
        animator.SetBool("Sheild", true);
        numberOfDamgeTake = 0;
    }
    IEnumerator Hurt()
    {
        DisableBodyParts();
        animator.SetBool("IsHurt", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("IsHurt", false);
        SetActiveBodyParts();
    }
   
    public IEnumerator Die()
    {
        // Die Animation
        DisableBodyParts();
        animator.SetBool("IsDied", true);
        Debug.Log("Player died!");
        bgSound.Stop();
        yield return new WaitForSeconds(1f);
       // animator.SetBool("IsDied", false);
        // Disable the player
        FindObjectOfType<GameUIScript>().GameOver();
        SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.deathSound);
        
    }
    
    public IEnumerator OnOneDeath()
    {
        DisableBodyParts();
        currentHealth = 100;
        healthBar.SetHealth(currentHealth);
        animator = GetComponent<Animator>(); ;
        rb.bodyType = RigidbodyType2D.Static;
        // Die Animation
        CheckForAwatarSelected();
        animator.SetBool("IsDied", true);
        Debug.Log("Player died!"); 
        Debug.Log(" died!" + animator.GetBool("IsDied"));
       // PlayerPrefs.SetInt("ArrowPlayerHas", 10);
        SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.deathSound);
        // bgSound.Stop();
        yield return new WaitForSeconds(2f);
        // Set the player on check point position
        animator.SetBool("IsDied", false);
        Debug.Log("Player Reactive!");
        if ((PlayerPrefs.GetString("CurrentLevel") == "Level 1"))
            transformObj.position = transformObj.position + new Vector3(0,10f,0);
        else
        {
            transformObj.position = gm.lastCheckPointPos;/*
            Debug.Log("lastCheckPointPos pistion ! " + gm.lastCheckPointPos);
            Debug.Log("Player position transformObj ! " + transformObj.name);*/
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        SetActiveBodyParts();
    }
    public int PlayerMovingDirection()
    {
        if (direction == 1)
            return 1;
        else
            return 2;
    }

    void CheckGamePaused()
    {
        if (PauseGame.isGamePaused)
        {
            //bgSound.pitch *= .5f;
        }
       /* else
            bgSound.pitch = 1f;*/
    }

    public void Reset()
    {
        CheckForAwatarSelected();
        lifes = 0;
        currentHealth = 100;
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("CurrentHealth", 100);
        PlayerPrefs.SetInt("Lifes", 3);
        PlayerPrefs.SetInt("ArrowPlayerHas", 10);
    }

    void CheckForAwatarSelected()
    {
        if ((PlayerPrefs.GetInt("AvatarSelected") == 2))
        {
            animator = GameObject.Find("MushrromPlayer").GetComponent<Animator>();
            transformObj = GameObject.Find("MushrromPlayer").GetComponent<Transform>();
            Debug.Log("Avatar selected" + (PlayerPrefs.GetInt("AvatarSelected")));
        }
        else if ((PlayerPrefs.GetInt("AvatarSelected") == 1))
        {
            animator = GameObject.Find("Player_Goblin").GetComponent<Animator>();
            transformObj = GameObject.Find("Player_Goblin").GetComponent<Transform>();
            Debug.Log("Avatar selected" + (PlayerPrefs.GetInt("AvatarSelected")));
        }
    }
    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

}
