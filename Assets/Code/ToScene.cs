using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ToScene : MonoBehaviour
{
    public bool LEVEL_2;
    public bool LEVEL_3;
    public bool MENU;
    public AudioMixer Audio;

    //not incorperated/created yet
    public bool END;

    void OnTriggerEnter()
    {
        StarterAssets.StarterAssetsInputs.Menuopen = true;
        //start fade in loading screen
        StartCoroutine(FadeIn());
    }

    public void Auto()
    {
        StartCoroutine(FadeIn());
    }


    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2.5f);
        if (LEVEL_2)
        {
            SceneManager.LoadScene("Level 2 Scene");
        }
        if (LEVEL_3)
        {
            SceneManager.LoadScene("Level 3 Scene");
        }
        if (MENU)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public Image LoadingScreenFader;
    public GameObject LoadingScreen;

    public IEnumerator FadeIn()
    {
        var targetAlpha = 1.0f;
        Color curColor = LoadingScreenFader.color;
        while (Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)
        {
            var lerp = Mathf.Lerp(curColor.a, targetAlpha, 1 * Time.deltaTime);
            Audio.SetFloat("MasterVol", 80 / -lerp);
            curColor.a = lerp;
            LoadingScreenFader.color = curColor;
            if (curColor.a > 0.9)
            {
                LoadingScreenFader.raycastTarget = true;
                LoadingScreenFader.maskable = true;
                LoadingScreen.SetActive(true);
                LoadingScreenFader.gameObject.transform.SetParent(transform);
                StartCoroutine(LoadScene());
            }
            yield return null;
        }
    }

}
