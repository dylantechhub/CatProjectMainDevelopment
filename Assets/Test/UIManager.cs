using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using StarterAssets;

public class UIManager : MonoBehaviour
{
    [Header("Options menu")]
    public Slider VolumeSlider;
    public AudioMixer AudioMixer;
    public float CurrentVolume;

    [Header("UI stuff")]
    public Image UImenu;
    public Image OptionsMenu;
    

    [Header("KeyBinding")]
    public bool Escenabled = false;

    // Start is called before the first frame update
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UImenu.gameObject.SetActive(true);
            GameObject.Find("cat").GetComponent<StarterAssetsInputs>().Menuopen = true;
        }
    }

    public void OnRestart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void OnPlay()
    {
        
    }

    public void OnContinue()
    {
        UImenu.gameObject.SetActive(false);
        GameObject.Find("cat").GetComponent<StarterAssetsInputs>().Menuopen = false;
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

   
}
