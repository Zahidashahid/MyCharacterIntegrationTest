using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public Transform player;
    public Rigidbody2D rb;
    public Boss boss;
    public float speed = 2.5f;
     float attackRange = 15f;
    public Transform leftLimit;
    public Transform rightLimit;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        leftLimit = GameObject.FindGameObjectWithTag("Left Limit Boss").transform;
        rightLimit = GameObject.FindGameObjectWithTag("Right Limit Boss").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Boss is in the limits");
        boss.LookAtPlayer();
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetBool("Attack", true);

        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            animator.SetBool("Attack", false);
    }


}
