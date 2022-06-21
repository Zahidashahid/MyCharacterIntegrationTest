using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
public class GiftData : MonoBehaviour
{
    //Display data on setting
    public static int gemCount;//Amount of gems in prefrences
    public static int cherryCount;//Amount of cherry in prefrences
    public TMP_Text gemText;
    public TMP_Text cherryText;
    void Start()
    {
        gemCount = SaveSystem.instance.playerData.gemPlayerHas ;
        cherryCount = SaveSystem.instance.playerData.cherryPlayerHas;
    }

    // Update is called once per frame
    public void UpdateGiftsData()
    {
        gemText.text = "------" + gemCount;
        cherryText.text = "---------" + cherryCount;
    }
}
