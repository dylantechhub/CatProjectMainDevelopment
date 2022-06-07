using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [Header("Options menu")]
    public Slider VolumeSlider;
    public AudioMixer AudioMixer;
    public float CurrentVolume;

    [Header("UI stuff")]
    public Image UImenu;
    public Image OptionsMenu;
    public Button Continue;
    public Button Restart;
    public Button Play;
    public Button Options;
    public Button Quit;

    [Header("KeyBinding")]
    public bool Escenabled = false;

    // Start is called before the first frame update
    private void Update()
    {
        PlayerPrefs.SetFloat("VolumePreference", CurrentVolume);
    }
    void OnContinue()
    {
        UImenu.gameObject.SetActive(false);
        
    }

    void OnMenuBack()
    {

    }

    void OnRestart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void OnPlay()
    {
        
    }

    void OnOptionsMenu()
    {
        OptionsMenu.gameObject.SetActive(true);
        PlayerPrefs.SetFloat("VolumePreference", CurrentVolume);
    }

    private void OnApplicationQuit()
    {
        Application.Quit();
    }

    void Isfullscreen(bool Fullscreen)
    {
        Screen.fullScreen = Fullscreen;
    }

    public void SetVolume(float Volumefloat)
    {
        AudioMixer.SetFloat("Master", Volumefloat);
        CurrentVolume = Volumefloat;
        

    }
}
