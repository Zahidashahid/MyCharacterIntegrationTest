using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.SceneManagement;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.InputSystem;

using TMPro;
public class PlayerMovement : MonoBehaviour
{
    
    PlayerController controls;
    Vector3 move;
    bool isShieldBtnPressed;
    bool isJumpBtnPressed;
    // Vector3 m;

    public CharacterController2D controller;
    [SerializeField] public LayerMask m_WhatIsGround;
    public Rigidbody2D rb;
    private BoxCollider2D boxCollider2d;

    public Animator animator;
    public Transform transform;
    //public Animator eagle_animator;
    //public Animator skeleton_animator;
    /*public AudioSource jumpSound;
    public AudioSource DeathSound;
    public AudioSource meleeAttackSound;*/
    public AudioSource bgSound;

    //bool crouch = false;
    //bool grounded;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [Range(1, 10)]
    public float jumpVelocity;
    public int jumpCount = 0;
    public int direction = 2;
    [Range(1, 100)]
    public static int currentHealth;
    public int maxHealth = 100;
    public static int lives ;
    public int numberOfDamgeTake ;

    public bool isHurt;
    public bool activeShield;
    public bool isWalking;

    //private float dashTime = 40f;
    public float attackRange = 0.5f;
    public float attackRate = 1f; //one attack per second
    public float nextAttackTime = 0f;
    public float distToGroundCapusleCollider = 1.6f;

    float runSpeed = 8f;
    float dashMoveSpeed = 16f;
    //float jumpHight = 10f;

    public Transform attackPoint;
    public Transform weaponAttackPoint;
    public LayerMask enemyLayers;

    //private Shield shield; //Player Shield
    private GameObject bodyParts;
    private GameObject weapon;
    //private GameMaster gm;
    public TMP_Text livesText;

    /*Scripts Refrences*/
    public HealthBar healthBar;
    PauseGame pauseGameScript;
    public ArrowStore arrowStoreScript;
    //GameUIScript gameUIScript;
    
    private void Awake()
    {
        //SaveSystem.instance.LoadPlayer();
        boxCollider2d = GetComponent<BoxCollider2D>();
        lives = 3;
        controls = new PlayerController();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.performed += ctx => isWalking = true;
        controls.Gameplay.Move.canceled += ctx => isWalking = false;
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Move.canceled += ctx => StopMoving();
      /*  controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;*/
        controls.Gameplay.Jump.performed += ctx => JumpPlayer();
        controls.Gameplay.Jump.performed += ctx => isJumpBtnPressed = true;
        controls.Gameplay.Jump.canceled += ctx => isJumpBtnPressed = false;
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
        jumpVelocity = 10f;
        numberOfDamgeTake = 0;
        isHurt = false;
        CheckForAwatarSelected();
        bgSound = GameObject.FindGameObjectWithTag("BGmusicGameObject").GetComponent<AudioSource>();
        pauseGameScript = GameObject.FindGameObjectWithTag("PauseCanvas").GetComponent<PauseGame>();
        arrowStoreScript = GameObject.FindGameObjectWithTag("ArrowStore").GetComponent<ArrowStore>();
        bodyParts = GameObject.FindGameObjectWithTag("BodyParts");
        weapon = GameObject.FindGameObjectWithTag("WeaponSprite");
        //gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        animator = GetComponent<Animator>();
        //Eagle_animator = GameObject.FindGameObjectWithTag("Enemy").transform<Animator>();
        // grounded = true;
        // bgSound.Play();

        
        if (MainMenu.isNewGame || GameUIScript.isNewGame || (SaveSystem.instance.playerData.level == "Level 1")) 
        {
            Debug.Log("-----New Game Started--------");
            Debug.Log("MainMenu.isNewGame" + MainMenu.isNewGame);
            Debug.Log("GameUIScript.isNewGame  " + GameUIScript.isNewGame);
            Debug.Log("SaveSystem.instance.playerData.level == Level 1  " + SaveSystem.instance.playerData.level == "Level 1");
            /*------------Reset Gift collected---------------------*/
            SaveSystem.instance.playerData.gemPlayerHas = 0; 
            SaveSystem.instance.playerData.cherryPlayerHas = 0;

            /*-------------Reset arrow Store----------------------*/
            Debug.Log(" Arrow store in data "); 
            SaveSystem.instance.playerData.numOfArrows  = arrowStoreScript.maxNumOfArrow;
            SaveSystem.instance.SavePlayer();
            /* -------- Set last check point zero when game restarted-----------*/
            GameMaster.lastCheckPointPos = transform.position;
            Debug.Log("Saving to file");
            SaveSystem.instance.playerData.lastCheckPointPos[0] = transform.position.x;
            SaveSystem.instance.playerData.lastCheckPointPos[1] = transform.position.y;
            /*
            SaveSystem.instance.playerData.lastCheckPointPos[0] = transform.position.x;
            SaveSystem.instance.playerData.lastCheckPointPos[1] = transform.position.y;*/
            currentHealth = maxHealth;
            SaveSystem.instance.playerData.health = currentHealth;
            SaveSystem.instance.playerData.level = MainMenu.currentLevel;
            lives = CheckDifficultylevel();
            livesText.text = "X " + lives;
            SaveSystem.instance.playerData.lives = lives; 
            SaveSystem.instance.SavePlayer();
           // SaveSystem.instance.LoadPlayer();
        }
        else
        {
            Debug.Log(" Game Continue");
            lives = SaveSystem.instance.playerData.lives;
            livesText.text = "X " + lives;
            currentHealth = SaveSystem.instance.playerData.health;

            GameMaster.lastCheckPointPos.x =   SaveSystem.instance.playerData.lastCheckPointPos[0];
            GameMaster.lastCheckPointPos.y =   SaveSystem.instance.playerData.lastCheckPointPos[1];
            transform.position = GameMaster.lastCheckPointPos;
            /*Debug.Log(" SaveSystem.instance.playerData.lastCheckPointPos[0] " + SaveSystem.instance.playerData.lastCheckPointPos[0] +
                "\t SaveSystem.instance.playerData.lastCheckPointPos[1]" + SaveSystem.instance.playerData.lastCheckPointPos[1]);
              Debug.Log("\n transform.position " + transform.position 
                + " gm.lastCheckPointPos " + GameMaster.lastCheckPointPos
                + "\t gm.lastCheckPointPos.x" + GameMaster.lastCheckPointPos.y);*/
            ArrowStore.arrowPlayerHas =  SaveSystem.instance.playerData.numOfArrows;
            lives = SaveSystem.instance.playerData.lives;
            currentHealth = SaveSystem.instance.playerData.health;
            livesText.text = "X " + lives;
           
        }
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        /* if (SaveSystem.instance.hasLoaded)
         {
             gm.lastCheckPointPos = SaveSystem.instance.playerData.lastCheckPointPos;
             lives = SaveSystem.instance.playerData.lives;
             currentHealth = SaveSystem.instance.playerData.health;
             *//* SaveSystem.instance.playerData.gemCollected;
             SaveSystem.instance.playerData.cherryCollected;
             SaveSystem.instance.playerData.difficultyLevel;
               =SaveSystem.instance.playerData.level;*//*
         }
         else
         {
             SaveData();
         }*/
    }
    private void Update()
    {
        string currentLevel = SaveSystem.instance.playerData.level;
        /*SaveSystem.instance.playerData.numOfArrows = 150;
        SaveSystem.instance.SavePlayer();*/
        if (currentLevel == "Level 1")
        {
            SaveSystem.instance.playerData.lastCheckPointPos[0] = transform.position.x;
            SaveSystem.instance.playerData.lastCheckPointPos[1] = transform.position.y;
          SaveSystem.instance.SavePlayer();
        }
        if (!isJumpBtnPressed)
        {
            animator.SetBool("IsJumping", false);
        }
        /*------For better jump---------*/
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime; ;
        }
        else if (rb.velocity.y > 0 && !isJumpBtnPressed)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; 
        }
       

        if (IsGrounded())
        {
            jumpCount = 1;
        }
        
    }
    private void FixedUpdate()
    {
        // Move Player back
        //CheckGamePaused();
        // m = new Vector3(move.x, move.y)  * 10f *  Time.deltaTime;
        /*//Debug.Log(" move.x " + move.x);
          Debug.Log(" move.y " + move.y);
        Debug.Log(" move.z " + move.z);*/

        //Debug.Log("IsGrounded " + IsGrounded());
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
   
        /*animator.SetFloat("Speed", Mathf.Abs(40));
        transform.Translate(m, Space.World);*/
            // MovePlayer();
            // MelleAttack();
    }
   void StopMoving()
   {
        if (!isWalking )
        {
            animator.SetFloat("Speed", Mathf.Abs(0));
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
   }
   void MovePlayerRight()
   {
        direction = 2;
        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
        Flip();
        if (IsGrounded())
        {
            animator.SetFloat("Speed", Mathf.Abs(40));
            animator.SetBool("IsJumping", false);
        }
        else
            animator.SetFloat("Speed", Mathf.Abs(0));
    }
    void MoveplayerLeft()
    {
        direction = 1;
        rb.velocity = new Vector2(-runSpeed, rb.velocity.y);
        Flip();
        if (IsGrounded())
        {
            animator.SetFloat("Speed", Mathf.Abs(40));
            animator.SetBool("IsJumping", false);
        }
        else
            animator.SetFloat("Speed", Mathf.Abs(0));
    }
    private void Flip()
    {
        // Rotate the player
        if ( !pauseGameScript.isGamePaused)
       {
            if (transform.localEulerAngles.y != 180 && direction == 1)
                transform.Rotate(0f, 180f, 0f);
            else if (transform.localEulerAngles.y != 0 && direction == 2)
                transform.Rotate(0f, -180f, 0f);
       }
        // player flip point of attck also flip is direction
        //transform.Rotate(0f, 180f, 0f);
    }
    public void PlayerChangeDirection()
    {
        if(isWalking == false)
        {
            if (direction == 1)
            {
                direction = 2;
                Flip();
            }
            else if (direction == 2)
            {
                direction = 1;
                Flip();
            }
        }
    }
    void JumpPlayer()
    {

        if (jumpCount < 2 && !isHurt )
        {
            jumpCount++;
             rb.velocity = Vector2.up * jumpVelocity;
            animator.SetBool("IsJumping", true);
           /* Debug.Log(" jump count is " + jumpCount);
            Debug.Log(" IsGrounded() is " + IsGrounded());*/

            /*
             * SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.jumpSound);
            */
            //grounded = false;
           /* if (direction == 1)
            {
                rb.velocity = new Vector2(-0, rb.velocity.y);
            }
            else if (direction == 2)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }*/
           
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }
    }
    void DashMovePlayer()
    {
        
        Debug.Log("DashMovePlayer() Grounded " + IsGrounded());
        if (IsGrounded())
        {
            //rb.velocity = new Vector2(5, rb.velocity.y);
            //animator.SetBool("IsJumping", true);

            Debug.Log("direction " + direction);
            if (direction == 1)
            {
               //rb.velocity =  Vector2.left * 8;
                rb.velocity = new Vector2(-dashMoveSpeed, rb.velocity.y);
                //transform.localScale = new Vector2(-1, 1);
                
            }
            else if (direction == 2)
            {
               // rb.velocity =  Vector2.right * 8;
                rb.velocity = new Vector2(dashMoveSpeed, rb.velocity.y);
                //transform.localScale = new Vector2(1, 1);
                //animator.SetFloat("Speed", Mathf.Abs(40));
            }

        }
        
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
            SetActiveBodyParts();
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
    
    private bool IsGrounded()
    {   
        //RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0.1f, Vector2.down, 0.1f, m_WhatIsGround);
        RaycastHit2D raycastHit2d = Physics2D.Raycast(transform.position, Vector2.down, distToGroundCapusleCollider + 0.1f, m_WhatIsGround);
        
        return raycastHit2d.collider != null;
    }
    /* <summary>
       ------------Attack on skeleton enemy--------------------------------------
    </summary> */
    IEnumerator Attack() //Melle Attack by player
    {
        if(!pauseGameScript.isGamePaused)
        {
            animator.SetBool("Attack 2", true);
            Debug.Log("Attacking ");
            yield return new WaitForSeconds(0.7f);

            SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.meleeAttackSound);
            animator.SetBool("Attack 2", false);
            string difficultyLevel = SaveSystem.instance.playerData.difficultyLevel;
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
        bodyParts.SetActive(false);
        weapon.SetActive(false);
    }
    public void TakeDamage(int damage)
    {
        if(lives >= 1)
        {
            if (numberOfDamgeTake > 3)
                StartCoroutine(SheildTimer());
            if (!(animator.GetBool("Sheild")))
            {
                currentHealth -= damage;
                SaveSystem.instance.playerData.health = currentHealth;
                SaveSystem.instance.SavePlayer();
                healthBar.SetHealth(currentHealth);
                // DisableBodyParts();

                if (currentHealth > 0.01)
                    StartCoroutine(Hurt());
                //SetActiveBodyParts();
                // play hurt animation
                // StartCoroutine(HurtAnimation());

                if (currentHealth <= 0 && lives <= 1)
                {
                    // bgSound.Stop();
                   
                    SaveSystem.instance.playerData.health = maxHealth;
                    SaveSystem.instance.playerData.lives = 3;
                   SaveSystem.instance.SavePlayer();
                    // Reset gifts collected and last check point
                    ResetData();
                    SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.deathSound);
                    FindObjectOfType<GameUIScript>().ResetDataOfLastGame();
                    FindObjectOfType<GameUIScript>().RestLastCheckPoint();
                    StartCoroutine(Die());
                    this.enabled = false;
                }
                else if (currentHealth <= 0 && lives > 1)
                {
                    StartCoroutine(OnOneDeath());
             
                }

            }
            else
                numberOfDamgeTake += 1;

        }
        else
        {
            Debug.Log("Out of lives");
        }
        
    }
    IEnumerator SheildTimer()
    {
        activeShield = false;
        animator.SetBool("Sheild", false);
        yield return new WaitForSeconds(0.8f);
        activeShield = true;
        animator.SetBool("Sheild", true);
        numberOfDamgeTake = 0;
    }
    IEnumerator Hurt()
    {
        DisableBodyParts();
        isHurt = true;
        animator.SetBool("IsHurt", true);
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("IsHurt", false);
        SetActiveBodyParts();
        isHurt = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
   public void SaveData()
   {
        Debug.Log(" ----  Data Saved in file --- ");
        SaveSystem.instance.playerData.lives = lives;
        SaveSystem.instance.playerData.lastCheckPointPos[0] = GameMaster.lastCheckPointPos.x;
        SaveSystem.instance.playerData.lastCheckPointPos[1] = GameMaster.lastCheckPointPos.y;
        SaveSystem.instance.playerData.health = currentHealth;
       SaveSystem.instance.SavePlayer();


    }
    public void ResetData()
    {
        Debug.Log(" ---- Reset Data --- ");  
        SaveSystem.instance.playerData.lives = CheckDifficultylevel();
        SaveSystem.instance.playerData.lastCheckPointPos[0] = 0;
        SaveSystem.instance.playerData.lastCheckPointPos[1] = 4;
        SaveSystem.instance.playerData.health = maxHealth;
        SaveSystem.instance.playerData.numOfArrows = arrowStoreScript.maxNumOfArrow;
       /* SaveSystem.instance.playerData.gemPlayerHas = 550;
        SaveSystem.instance.playerData.cherryPlayerHas = 550;*/
        SaveSystem.instance.SavePlayer();

    }

    int CheckDifficultylevel()
    {
        if (SaveSystem.instance.playerData.difficultyLevel == "Hard")
        {
            lives = 1;
        }
        else if (SaveSystem.instance.playerData.difficultyLevel == "Medium")
        {
            lives = 2;

        }
        else if (SaveSystem.instance.playerData.difficultyLevel == "Easy")
        {
            lives = 3;

        }
        return lives;
    }
    public IEnumerator Die()
    {
        // Die Animation
        DisableBodyParts();
        animator.SetBool("IsDied", true);
        Debug.Log("Player died!");
        ResetData();
        Debug.Log("SaveSystem.instance.playerData.health " + SaveSystem.instance.playerData.health);
        bgSound.Stop();
        yield return new WaitForSeconds(0.4f);
       // animator.SetBool("IsDied", false);
        // Disable the player
        FindObjectOfType<GameUIScript>().GameOver();
        SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.deathSound);
        
    }
    
    public IEnumerator OnOneDeath()
    {
        DisableBodyParts();
        currentHealth = maxHealth;
        SaveSystem.instance.playerData.health = currentHealth;
        SaveSystem.instance.SavePlayer();
        healthBar.SetHealth(currentHealth);
        lives = lives -  1;
        Debug.Log("lives left " + lives);
       
        animator = GetComponent<Animator>(); ;
        rb.bodyType = RigidbodyType2D.Static;
       
        // Die Animation
        CheckForAwatarSelected();
        animator.SetBool("IsDied", true);
        Debug.Log("Player died!"); 
        Debug.Log(" died!" + animator.GetBool("IsDied"));

        SoundEffect.sfInstance.audioS.PlayOneShot(SoundEffect.sfInstance.deathSound);
        // bgSound.Stop();
        yield return new WaitForSeconds(0.8f);
       
        // Set the player on check point position
        animator.SetBool("IsDied", false);
        livesText.text = "X " + lives;
        Debug.Log("Player Reactive!");
        if ((SaveSystem.instance.playerData.level == "Level 1"))
            transform.position = transform.position + new Vector3(0,10f,0);
        else
        {
            GameMaster.lastCheckPointPos.y = GameMaster.lastCheckPointPos.y +  3f; 
            transform.position = GameMaster.lastCheckPointPos ;
            Debug.Log("lastCheckPointPos pistion ! " + GameMaster.lastCheckPointPos);
            Debug.Log("Player position transform ! " + transform.name);
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
      
        SetActiveBodyParts();
        SaveData();
    }
   
/*    void CheckGamePaused()
    {
        if (PauseGame.isGamePaused)
        {
            //bgSound.pitch *= .5f;
        }
       *//* else
            bgSound.pitch = 1f;*//*
    }*/

    public void Reset()
    {
        CheckForAwatarSelected();
        lives = 3;
        currentHealth = 100;
        Time.timeScale = 1f;

        Debug.Log(" Arrow store in data "); 
        SaveSystem.instance.playerData.numOfArrows = arrowStoreScript.maxNumOfArrow;
        SaveSystem.instance.playerData.avatarSelected = ChangeAvatar.avatarSelected;
        SaveSystem.instance.SavePlayer();
       // SaveSystem.instance.LoadPlayer();
        SaveData();
    }

   
    void CheckForAwatarSelected()
    {
        if ((SaveSystem.instance.playerData.avatarSelected == 2))
        {
            animator = GameObject.Find("MushrromPlayer").GetComponent<Animator>();
            transform = GameObject.Find("MushrromPlayer").GetComponent<Transform>();
            Debug.Log("Avatar selected" + (SaveSystem.instance.playerData.avatarSelected));
        }
        else if ((SaveSystem.instance.playerData.avatarSelected == 1))
        {
            animator = GameObject.Find("Player_Goblin").GetComponent<Animator>();
            transform = GameObject.Find("Player_Goblin").GetComponent<Transform>();
            Debug.Log("Avatar selected" + (SaveSystem.instance.playerData.avatarSelected));
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
