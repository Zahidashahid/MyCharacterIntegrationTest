
using UnityEngine;

public class Boss : MonoBehaviour
{

	public Transform player;

	public bool isFlipped = false;
    private void Start()
    {
		if ((SaveSystem.instance.playerData.avatarSelected == 2))
		{
			player = GameObject.Find("MushrromPlayer").transform;
		}
		else if ((SaveSystem.instance.playerData.avatarSelected == 1))
		{
			player = GameObject.Find("Player_Goblin").transform;
		}
		
	}
 
    public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}
	
}