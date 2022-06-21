
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager obj;
    public TMP_Text gemText;
    public TMP_Text cherryText;
   // public TextMeshPro cherryText;
    int gemCollected;
    int cherryCollected;
    private void Start()
    {

        gemCollected = SaveSystem.instance.playerData.gemPlayerHas;
        cherryCollected = SaveSystem.instance.playerData.cherryPlayerHas;
        gemText.text = "X" + gemCollected;
        cherryText.text = "X" + cherryCollected;
    }
    public void GemCollect()
    {
        gemCollected += 1;
        SaveSystem.instance.playerData.gemPlayerHas = gemCollected;
        SaveSystem.instance.SavePlayer();
        Debug.Log("----------Gem saved in file ------------ "  );
        UpdateGemText(gemCollected);
    }
    public void CherryCollect()
    {
        cherryCollected += 1;
        SaveSystem.instance.playerData.cherryPlayerHas = cherryCollected;
        SaveSystem.instance.SavePlayer();
        Debug.Log("----------Gcherry saved in file ------------ ");
        UpdateCherryText(cherryCollected);
    }
    public void UpdateGemText(int count)
    {
        gemText.text = "X" + count;
        Debug.Log("Gem = " + count);
    }
    public void UpdateCherryText(int count)
    {
        cherryText.text = "X" + count;
        Debug.Log("Cherry = " + count);
    }
}
