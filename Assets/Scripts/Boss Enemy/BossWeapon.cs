
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
	public CameraShake cameraShake;
	//public int attackDamage = 0;
	 int attackDamage = 20;
	 int enragedAttackDamage = 40;

	public Vector3 attackOffset;
	public float attackRange ;
	float jumpVelocity = 7f;
	public LayerMask attackMask;
	Animator animator;
	Rigidbody2D rb;
	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		Debug.Log("Awake boss weapon");
    }
    private void Start()
    {
		attackRange = 19f;
	}

    public void Attack()
	{
	
		Vector3 pos = transform.position;
		//Vector2 size= new Vector2( 10f,9f);
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;
		Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
		//Collider2D colInfo = Physics2D.OverlapBox(pos,  size, attackRange, attackMask);
		if (colInfo != null)
		{
			colInfo.GetComponent<PlayerMovement>().TakeDamage(attackDamage);
		}
	}
    
    public  void BossJumpAttack()
    {
		//play jump animation 
		rb.velocity = Vector2.up * jumpVelocity;
		animator.SetTrigger("Jump");
		Debug.Log("Boss jump attack");
		/*--------Shake Enviorement-----------*/
		StartCoroutine(cameraShake.Shake(0.5f, 0.6f));

		//if player is on the ground , it will take demage
		PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		if (player.IsGrounded())
        {
			int jumpDamage = 10;
			player.GetComponent<PlayerMovement>().TakeDamage(jumpDamage);
		}
		// if player is in the air , it will not take damage 

	}
	public void BossJumpAttackStop()
    {
		animator.ResetTrigger("Jump");
		Debug.Log("Boss jump stop");
	}
	public void EnragedAttack()
	{
		Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;
		//GetComponent<PlayerMovement>().TakeDamage(attackDamage);
		Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
		if (colInfo != null)
		{
			colInfo.GetComponent<PlayerMovement>().TakeDamage(enragedAttackDamage);
		}
	}
	public void WeakPointExpose()
    {
		/*Collider2D weakPointColliders = GameObject.Find("WeakPoint1").GetComponent<PolygonCollider2D>();
		weakPointColliders.enabled = true;*/
	}
	public void WeakPointHide()
    {
		/*Collider2D weakPointColliders = GameObject.Find("WeakPoint1").GetComponent<PolygonCollider2D>();
		weakPointColliders.enabled = false;*/
	}
    
    void OnDrawGizmosSelected()
	{
		Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;
		Gizmos.color = new Color(1, 0, 0, 0.3f);
		//Gizmos.DrawCube(pos, new Vector3(5, 1.5f, 1));
		//Gizmos.DrawWireSphere(pos, attackRange);
	}
	
}