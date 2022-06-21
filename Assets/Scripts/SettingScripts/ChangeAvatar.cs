
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
        if ( SaveSystem.instance.playerData.avatarSelected == 1)
        {
            SaveSystem.instance.playerData.avatarSelected = 1;
        }

        else
        {
            SaveSystem.instance.playerData.avatarSelected = 2;
        }
       SaveSystem.instance.SavePlayer();
    }
    public void SelectAvatarOne()
    {
        SaveSystem.instance.playerData.avatarSelected = avatarSelected = 1;
        SaveSystem.instance.SavePlayer();
    }
    public void SelectAvatarTwo()
    {
        SaveSystem.instance.playerData.avatarSelected = avatarSelected = 2;
       SaveSystem.instance.SavePlayer();
    }
   

  /*  public void ChangeAvatar(Image img)
    {
        audioSrcMusic.Stop();
        audioSrcMusic.clip = music;
        audioSrcMusic.Play();

    }*/

}
