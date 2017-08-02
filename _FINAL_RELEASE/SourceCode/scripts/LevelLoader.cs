using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader INSTANCE { get; protected set; }

    public GameObject loadingScreen;
    public Slider loadingBar;

    public GameObject PlayerNameEntry;

    public AudioClip bleep;
    public AudioClip bloop;

    void Start()
    {
        if (INSTANCE != null)
        {
            Debug.LogError("MORE THAN ONE LEVEL LOADER!!!!");
        }
        else
        {
            INSTANCE = this;
        }
    }

    //these buttons generally relate to level loading so the handlers are here
    #region buttons

    public void btnQuitClick()
    {
        GlobalController.INSTANCE.PlaySound(bleep);

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void btnPlayClick()
    {
        if (PlayerNameEntry != null)
        {
            GlobalController.INSTANCE.PlaySound(bleep);
            PlayerNameEntry.SetActive(true);
        }
    }

    public void btnScoresClick()
    {
        GlobalController.INSTANCE.PlaySound(bleep);
        LoadLevel("_scores");
    }

    public void btnBackClick()
    {
        GlobalController.INSTANCE.PlaySound(bloop);
        LoadLevel("_main_menu");
    }

    public void btnCreditsClick()
    {
        GlobalController.INSTANCE.PlaySound(bleep);
        LoadLevel("_credits");
    }

    #endregion

    #region level loading

    public void LoadLevel(string scene)
    {
        GlobalController.INSTANCE.LastLevelName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadAsynchronously(scene));
    }

    IEnumerator LoadAsynchronously(string scene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);

        loadingScreen.SetActive(true);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            loadingBar.value = progress;

            yield return null;
        }
    }

    #endregion
}
