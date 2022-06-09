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

    [Header("Controller UI")]
    public Button[] CUIbuttons;
    public MonoBehaviour[] CallFunction;
    public int currentbutton;
    

    //Keybinding stuff
    private bool Escenabled = false;
    private bool ControllerMenu = false;

    
    private void Update()
    {
        if (ControllerMenu)
        {
            CUIbuttons[currentbutton].interactable = false;
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UImenu.gameObject.SetActive(true);
            GameObject.Find("cat").GetComponent<StarterAssetsInputs>().Menuopen = true;
            Escenabled = true;
        }
        if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            UImenu.gameObject.SetActive(true);
            GameObject.Find("cat").GetComponent<StarterAssetsInputs>().Menuopen = true;
            Escenabled = true;
            ControllerMenu = true;
        }
        if (Escenabled)
        {
            if (Gamepad.current.leftStick.up.wasPressedThisFrame)
            {
                currentbutton--;
                if (currentbutton < -0)
                {
                    currentbutton = 2;
                }
                
            }
            if (Gamepad.current.leftStick.down.wasPressedThisFrame)
            {
                currentbutton++;
                if (currentbutton > 2)
                {
                    currentbutton = 0;
                }
            }
            if (Gamepad.current.aButton.wasPressedThisFrame)
            {
                CUIbuttons[currentbutton].onClick.Invoke();
            }
        }
    }

    public void LateUpdate()
    {
        if (ControllerMenu)
        {
            CUIbuttons[0].interactable = true;
            CUIbuttons[1].interactable = true;
            CUIbuttons[2].interactable = true;
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
        Escenabled = false;
        ControllerMenu = false;
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

   
}
