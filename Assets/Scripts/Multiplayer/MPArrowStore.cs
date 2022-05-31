using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MPArrowStore : MonoBehaviour
{
    int maxNumArrow = 100;
    public int arrowPlayer1Has ;
    public int arrowPlayer2Has;
    public TMP_Text arrowStoreP1Text;
    public TMP_Text arrowStoreP2Text;
    private void Awake()
    {
        
    }
    void Start()
    {

        arrowPlayer1Has = maxNumArrow;
        arrowPlayer2Has = maxNumArrow;
        /*PlayerPrefs.SetInt("ArrowPlayerHas", maxNumArrow );
        arrowPlayerHas = PlayerPrefs.GetInt("ArrowPlayerHas");*/
        arrowStoreP1Text.text = "X " + arrowPlayer1Has;
        arrowStoreP2Text.text = "X " + arrowPlayer2Has;
    }

    // Update is called once per frame
    public void ArrowUsed()
    {
        if(this.tag == "ArrowStoreP1")
            arrowPlayer1Has -= 1;
        else if (this.tag == "ArrowStoreP2")
            arrowPlayer2Has -= 1;
        UpdateArrowText();


    }
    public void UpdateArrowText()
    {
        if (this.tag == "ArrowStoreP1")
            arrowStoreP1Text.text = "X " + arrowPlayer1Has;
        else if (this.tag == "ArrowStoreP2")
            arrowStoreP2Text.text = "X " + arrowPlayer2Has;


    }
}
