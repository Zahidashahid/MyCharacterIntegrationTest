using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
	//public HealthBar healthBar;
	public int maxHealth = 500;
	public int health = 500;
	public static int numOfHits = 0;
	public GameObject deathEffect;
	int damage;
	public bool isInvulnerable = false;
    private void Start()
    {
		GetComponentInChildren<HealthBar>().SetHealth(health);
		string diffcultylevelSelected = PlayerPrefs.GetString("difficultyLevel");
		switch (diffcultylevelSelected)
		{
			case "Easy":
				damage = maxHealth / 3;
				break;
			case "Medium":
				damage = maxHealth / 5;
				break;
			case "Hard":
				damage = maxHealth / 10;
				break;

			default:
				break;
		}
	}
    public void TakeDamage()
	{
		if (isInvulnerable)
			return;

		health -= damage;
		numOfHits++;
		if ( numOfHits % 2 == 0)
		{
			/*-------------- Jump Attack Every second hit on boss-----------*/
			GetComponent<BossWeapon>().BossJumpAttack();
			//BossWeapon.BossJumpAttack();
		}
		GetComponentInChildren<HealthBar>().SetHealth(health);
		if (health <= 200)
		{
			GetComponent<Animator>().SetBool("IsEnraged", true);
			Debug.Log("enrage");
			Debug.Log("GameObject.Find(Spawn Points) " + GameObject.Find("Spawn Points"));
			if(GameObject.Find("Spawn Points"))
				GameObject.Find("Spawn Points").SetActive(false);
			
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