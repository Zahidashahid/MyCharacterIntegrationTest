/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
public class ArrowStore : MonoBehaviour
{
    public int arrowPlayerHas;
    public int maxNumOfArrow = 100;
    public TMP_Text arrowStoreText;
   
    void Start()
    {
        
        arrowPlayerHas = PlayerPrefs.GetInt("ArrowPlayerHas");
        Debug.Log("arrow player has " + arrowPlayerHas);
        /*PlayerPrefs.SetInt("ArrowPlayerHas", maxNumOfArrow);
        arrowPlayerHas = PlayerPrefs.GetInt("ArrowPlayerHas");*/
        arrowStoreText.text = "X " + arrowPlayerHas;
    }

    private void Update()
    {
        /*Debug.Log("arrowPlayerHas " + arrowPlayerHas);*/
       
    }
    public void ArrowUsed()
    {
        arrowPlayerHas -= 1;
        PlayerPrefs.SetInt("ArrowPlayerHas", arrowPlayerHas);
        Debug.Log("PlayerPrefs.GetInt(ArrowPlayerHas) " + PlayerPrefs.GetInt("ArrowPlayerHas"));
        UpdateArrowText();


    }
    public void UpdateArrowText()
    {
        arrowStoreText.text = "X " + arrowPlayerHas;
    }
}
