using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPoints : MonoBehaviour
{
	public GameObject weakPoint1;
	public GameObject weakPoint2;
	public GameObject weakPoint3;
	// Start is called before the first frame update
	void Start()
	{
		string diffcultylevelSelected = PlayerPrefs.GetString("difficultyLevel");
		switch (diffcultylevelSelected)
		{
			case "Easy":
				weakPoint1.SetActive(true);
				weakPoint2.SetActive(true);
				weakPoint3.SetActive(true);

				break;
			case "Medium":
				weakPoint1.SetActive(true);
				weakPoint2.SetActive(true);
				weakPoint3.SetActive(true);
				break;
			case "Hard":
				weakPoint1.SetActive(true);
				weakPoint2.SetActive(false);
				weakPoint3.SetActive(true);
				GameObject.Find("Boss").GetComponent<PolygonCollider2D>().enabled = true;
				break;

			default:
				break;
		}
	}
}
