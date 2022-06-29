using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public Transform player;
    public Rigidbody2D rb;
    public Boss boss;
   public float speed = 2.5f;
    public float attackRange = 3;
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
        float distanceToLeft = Vector2.Distance(rb.transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(rb.transform.position, rightLimit.position);
        if (rb.transform.position.x >= leftLimit.position.x && rb.transform.position.x <= rightLimit.position.x)
        {
            boss.LookAtPlayer();
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Vector2.Distance(player.position, rb.position) <= attackRange)
            {
                animator.SetTrigger("Attack");
            }
        }
        else
        {

            if(distanceToLeft > distanceToRight)
            {
                Vector2 target = new Vector2(leftLimit.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
            else
            {
                Vector2 target = new Vector2(rightLimit.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
        }
    }

           

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
            animator.ResetTrigger("Attack");
        
    }


}
