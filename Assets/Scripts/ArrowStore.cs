/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
public class ArrowStore : MonoBehaviour
{
    public static int arrowPlayerHas;
    public int maxNumOfArrow = 100;
    public TMP_Text arrowStoreText;
   
    void Start()
    {
        
        arrowPlayerHas =   SaveSystem.instance.playerData.numOfArrows;
        Debug.Log("arrow player has " + arrowPlayerHas);

        /*SaveSystem.instance.playerData.arrow, maxNumOfArrow);
        arrowPlayerHas = SaveSystem.instance.playerData.arrow;*/
        arrowStoreText.text = "X " + arrowPlayerHas;
    }

    private void Update()
    {
        /*Debug.Log("arrowPlayerHas " + arrowPlayerHas);*/
       
    }
    public void ArrowUsed()
    {
        if (arrowPlayerHas > 0)
        {

            arrowPlayerHas -= 1;
            Debug.Log(" Arrow store in data ");
            PlayerPrefs.SetInt("PlayerHasNumOfArrows", arrowPlayerHas);
            Debug.Log("(ArrowPlayerHas) " + PlayerPrefs.GetInt("PlayerHasNumOfArrows"));
            //SaveSystem.instance.SavePlayer();
            UpdateArrowText();
        }
        else
            Debug.Log("!!!You dont have arrows!!! ");
        


    }
    public void UpdateArrowText()
    {
        arrowStoreText.text = "X " + arrowPlayerHas;
    }
}
