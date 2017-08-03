using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour {

    public GameObject modelsAnimation;
    public GameObject textures;
    public GameObject sounds;

    public AudioClip bleep;

    public void btnModelsAnimation()
    {
        GlobalController.INSTANCE.PlaySound(bleep);
        modelsAnimation.SetActive(true);
        textures.SetActive(false);
        sounds.SetActive(false);
    }

    public void btnTextures()
    {
        GlobalController.INSTANCE.PlaySound(bleep);
        modelsAnimation.SetActive(false);
        textures.SetActive(true);
        sounds.SetActive(false);
    }

    public void btnSounds()
    {
        GlobalController.INSTANCE.PlaySound(bleep);
        modelsAnimation.SetActive(false);
        textures.SetActive(false);
        sounds.SetActive(true);
    }
}
