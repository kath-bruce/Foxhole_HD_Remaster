using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameEntryController : MonoBehaviour
{
    public AudioClip bleep;
    public AudioClip bloop;

    public void btnBackClick()
    {
        GlobalController.INSTANCE.PlaySound(bloop);
        gameObject.SetActive(false);
    }

    public void txtPlayerNameEnter(string input) //the input field triggers this method whenever unfocussed
    {
        GlobalController.INSTANCE.CurrentPlayerName = input;

        if (Input.GetKey(KeyCode.Return)) //this limits the game being loaded to when the player hits carriage return
        {
            GlobalController.INSTANCE.PlaySound(bleep);
            LevelLoader.INSTANCE.LoadLevel("_first_level");
        }
    }
}
