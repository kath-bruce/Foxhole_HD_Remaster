using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameEntryController : MonoBehaviour
{
    public AudioClip bleep;
    public AudioClip bloop;

    public TMPro.TMP_InputField playerName;

    public GameObject txtPleaseEnterName;

    public void btnBackClick()
    {
        GlobalController.INSTANCE.PlaySound(bloop);
        txtPleaseEnterName.SetActive(false);
        gameObject.SetActive(false);
    }

    public void txtPlayerNameEnter(string input) //the input field triggers this method whenever unfocussed
    {
        if (input == "" || input == null)
        {
            txtPleaseEnterName.SetActive(true);
            return;
        }
        else
        {
            txtPleaseEnterName.SetActive(false);
        }

        GlobalController.INSTANCE.CurrentPlayerName = input;

        //this limits the game being loaded to when the player hits carriage return/enter
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            GlobalController.INSTANCE.PlaySound(bleep);
            LevelLoader.INSTANCE.LoadLevel("_first_level");
        }
    }

    public void btnEnterClick()
    {
        if (playerName.text == "" || playerName.text == null)
        {
            txtPleaseEnterName.SetActive(true);
            return;
        }
        else
        {
            txtPleaseEnterName.SetActive(false);
        }

        GlobalController.INSTANCE.CurrentPlayerName = playerName.text;
        GlobalController.INSTANCE.PlaySound(bleep);
        LevelLoader.INSTANCE.LoadLevel("_first_level");
    }
}
