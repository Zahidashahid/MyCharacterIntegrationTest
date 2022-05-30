using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandMovement : MonoBehaviour
{
    /*--------Weapon movement Script----------*/
    /*--------Weapon/Gun  of the Player----------*/
    PlayerController controls;
    Vector3 rotateBow;

     PlayerMovement playerMovement;
    PauseGame pauseGameScript;


    private void Awake()
    {
        controls = new PlayerController();
        controls.Gameplay.RangeAttackGP.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Gameplay.RangeAttackGP.canceled += ctx => rotateBow = Vector2.zero;

        controls.Gameplay.MouseDirection.performed += ctx => BowRotate(ctx.ReadValue<Vector2>());
        controls.Gameplay.MouseDirection.canceled += ctx => rotateBow = Vector2.zero;
    }
    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        pauseGameScript = GameObject.FindGameObjectWithTag("PauseCanvas").GetComponent<PauseGame>();

    }
    void Update()
    {
       
    }
    private void Move(Vector2 vector)
    {
        transform.Rotate(vector.x * Vector3.forward + vector.y * Vector3.forward);
    }
    void BowRotate(Vector2 vector)
    {
        /*-----------Left hand i.e spreat object rotation with mouse position --------*/
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(vector) - transform.position;

        if (!pauseGameScript.isGamePaused)
        {
            float rotZ = Mathf.Atan2(mousePoint.y, mousePoint.x) * Mathf.Rad2Deg;
            if (playerMovement.direction == 2)
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
            else
                transform.rotation = Quaternion.Euler(0f, 180f, 180 - rotZ);

           
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
