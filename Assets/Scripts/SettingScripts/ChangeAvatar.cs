
using UnityEngine;

public class ChangeAvatar : MonoBehaviour
{
   public static int avatarSelected;
    private void Start()
    {
        //CheckAvatar();
        avatarSelected = SaveSystem.instance.playerData.avatarSelected;
    }
    void CheckAvatar()
    {
        /*--Not any avatar Selected-----*/
        Debug.Log("Not any avatar Selected-");
        if ( SaveSystem.instance.playerData.avatarSelected == 1)
        {
            PlayerPrefs.SetInt("AvatarSelected", 1);
        }

        else
        {
            PlayerPrefs.SetInt("AvatarSelected", 2);
        }
       //SaveSystem.instance.SavePlayer();
    }
    public void SelectAvatarOne()
    {
        avatarSelected = 1;
        PlayerPrefs.SetInt("AvatarSelected", avatarSelected );
        //SaveSystem.instance.SavePlayer();
    }
    public void SelectAvatarTwo()
    {
        avatarSelected = 2;
        PlayerPrefs.SetInt("AvatarSelected", avatarSelected);
        //SaveSystem.instance.SavePlayer();
    }
   

  /*  public void ChangeAvatar(Image img)
    {
        audioSrcMusic.Stop();
        audioSrcMusic.clip = music;
        audioSrcMusic.Play();

    }*/

}
