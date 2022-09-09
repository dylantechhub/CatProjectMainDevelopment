using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;

public class LoadNextLevel : MonoBehaviour
{
    private string curLvl;
    public GameObject cat;
    public GameObject nextLvlNotice;
    public AudioMixer Audio;

    private void Awake()
    {
        curLvl = SceneManager.GetActiveScene().name;
    }

    public IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(1.5f);
        if (curLvl.ToLower() == "level one")
        SceneManager.LoadScene("Level Two");
        else if (curLvl.ToLower() == "level two")
        SceneManager.LoadScene("Level Three");
        if (curLvl.ToLower() == "level three")
        SceneManager.LoadScene("Ending");
    }

    public bool close;

    private void Update()
    {
        //if player is near end pipe
        if ((cat.transform.position - transform.position).sqrMagnitude < 17 * 17)
        {
            close = true;
            nextLvlNotice.SetActive(true);
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                //load next level
                StarterAssets.StarterAssetsInputs.Menuopen = true;
                StartCoroutine(FadeIn());
            }
        }
        else if (!close)
        {
            close = false;
            nextLvlNotice.SetActive(false);
        } else
            close = false;
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
                StartCoroutine(GoToNextLevel());
            }
            yield return null;
        }
    }


}
