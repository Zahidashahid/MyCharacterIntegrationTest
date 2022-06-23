using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
	HealthBar healthBar;
	public int health = 500;
	
	public GameObject deathEffect;

	public bool isInvulnerable = false;
    private void Start()
    {
		GetComponentInChildren<HealthBar>().SetHealth(health);
	}
    public void TakeDamage(int damage)
	{
		if (isInvulnerable)
			return;

		health -= damage;

		GetComponentInChildren<HealthBar>().SetHealth(health);
		if (health <= 200)
		{
			GetComponent<Animator>().SetBool("IsEnraged", true);
		}

		if (health <= 0)
		{

			StartCoroutine(Die());
			
		}
	}

	IEnumerator Die()
	{

		GetComponent<Animator>().SetTrigger("Death");

		yield return new WaitForSeconds(0.8f);
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

}