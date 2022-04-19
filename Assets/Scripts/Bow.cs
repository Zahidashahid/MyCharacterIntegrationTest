/*using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;*/
using UnityEngine;

public class Bow : MonoBehaviour
{
    //Bow of the Player
    PlayerController controls;
    Vector3 rotateBow;
    public Transform shootPoint;
    public GameObject projectile;

    public float timeBtwShots;
   // public float startTimeBtwShots;
    public float nextAttackTime;
    bool canAttack;
    int arrowLeft;
    public ArrowStore arrowStore;
    public PlayerMovement playerMovement;
    Vector2 BowRotateValue;


    private void Awake()
    {
        controls = new PlayerController();
        controls.Gameplay.ArowHit.performed += ctx => ArrowShoot();
        controls.Gameplay.RangeAttackGP.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Gameplay.RangeAttackGP.canceled += ctx => rotateBow = Vector2.zero;

        controls.Gameplay.MouseDirection.performed += ctx => BowRotate(ctx.ReadValue<Vector2>());
        controls.Gameplay.MouseDirection.performed += ctx => BowRotateValue = ctx.ReadValue<Vector2>();
        controls.Gameplay.MouseDirection.canceled += ctx => rotateBow = Vector2.zero;
        


    }
    private void Start()
    {
        nextAttackTime = -1;
        canAttack = true;
        playerMovement = GetComponentInParent<PlayerMovement>();
       
    }
    void Update()
    {
        /* if (playerMovement.direction == 1)
         {
             mousePoint.x = -mousePoint.x;
             Debug.Log("Heading towadrs left " + mousePoint.x);
         }*/

       
        if (nextAttackTime <= -1)
        {
            canAttack = true;
        }
        else
        {
            nextAttackTime -= Time.deltaTime;
            canAttack = false;
        }

        //Debug.Log("PauseGame.isGamePaused ==  " + PauseGame.isGamePaused);
           
        /* Vector3 r = new Vector3(rotateBow.x, rotateBow.y, rotateBow.z) * 100f * Time.deltaTime;
         Debug.Log("rotateBow.z"+ rotateBow.z);
         Debug.Log("rotateBow.y"+ rotateBow.y);
         transform.Rotate(r ,Space.World);*/
        //transform.rotation = Quaternion.Euler(0f, 0f, r.z );
        /*Vector2 difference = Camera.main.ScreenToWorldPoint(m) - transform.position;
        float rotZ = Mathf.Atan2(m.y, m.x) * Mathf.Rad2Deg;
        *//*
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        Debug.Log("" + rotZ + offset);*/
        /*Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
         float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
         transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
         
         arrowLeft = PlayerPrefs.GetInt("ArrowPlayerHas");

         if (arrowLeft > 0)
         {
             if (Input.GetMouseButtonDown(0))
             {
                 arrowStore.ArrowUsed();
                 Instantiate(projectile, shootPoint.position, transform.rotation);

             }
         }*/

    }
    private void Move(Vector2 vector)
    {
        transform.Rotate(vector.x * Vector3.forward + vector.y * Vector3.forward);
    }
    void BowRotate(Vector2 vector)
    {
        /*-----------Weapon i.e spreat object rotation with mouse position --------*/
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(vector) - transform.position;

        if (PauseGame.isGamePaused == false )
        {

            float rotZ = Mathf.Atan2(mousePoint.y, mousePoint.x) * Mathf.Rad2Deg;
            if(playerMovement.direction == 2)
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ );
            else
                transform.rotation = Quaternion.Euler(0f, 180f, 180 - rotZ);
            Debug.Log("rotZ " + rotZ );
         


            if (mousePoint.x < 0 && playerMovement.direction == 2)
             {
                 Debug.Log(" gun change" + mousePoint.x + playerMovement.name );
                transform.Rotate(180f, 180f, 180f);
                playerMovement.PlayerChangeDirection();
             }
             else if (mousePoint.x > 0 && playerMovement.direction == 1)
             {
                  Debug.Log("" + mousePoint.x);
                transform.Rotate(0f, 180f, 0f);
                playerMovement.PlayerChangeDirection();
             }
            arrowLeft = PlayerPrefs.GetInt("ArrowPlayerHas");
        }
    }
    void ArrowShoot()
    {
        if (PauseGame.isGamePaused == false)
        {
            arrowLeft = PlayerPrefs.GetInt("ArrowPlayerHas");

            if (arrowLeft > 0 && canAttack)
            {
                arrowStore.ArrowUsed();
                Instantiate(projectile, shootPoint.position, transform.rotation);
                nextAttackTime = 0.01f;
            }
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
