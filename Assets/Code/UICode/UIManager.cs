using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using Options;

public class UIManager : MonoBehaviour
{
    [Header("Options menu")]
    public AudioMixer AudioMixer;
    public bool inOptions;

    [Header("UI stuff")]
    public GameObject UImenu;
    public GameObject OptionsMenu;
    public TextMeshProUGUI Devices;

    [Header("Controller UI")]
    public List<Button> CUIbuttons = new List<Button>();
    public List<Slider> CUISliders = new List<Slider>();
    //lock controller controls for uses for other options
    public bool ControllerLock;

    //if the controller is controlling a slider of some sort.
    public bool ControllerSliding;


    //Keybinding stuff
    private bool Escenabled = false;
    private bool ControllerMenu = false;

    
    private void Update()
    {
        if (ControllerMenu)
            Devices.text = $"Current Controls:\n Controller/Gamepad";
        else if (!ControllerMenu)
            Devices.text = $"Current Controls:\n Keyboard&Mouse";

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UImenu.SetActive(true);
            StarterAssets.StarterAssetsInputs.Menuopen = true;
            Escenabled = true;
        }

        //controller menu open
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            UImenu.SetActive(true);
            //changed the variable Menuopen to static, allowing this. no refrence needs to be made to change this variable.
            StarterAssets.StarterAssetsInputs.Menuopen = true;
            Escenabled = true;
            ControllerMenu = true;
            CUIbuttons[0].Select();
        }

        //if menu opened
        if (Escenabled)
        {

            //if controller went right to menu from options, then allow menu selection
            if (Gamepad.current != null && Gamepad.current.leftStick.right.wasPressedThisFrame && !ControllerLock && inOptions)
            {
                inOptions = false;
            }


            //back button
            if (Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame)
            {
                //if controller is locked when b is pressed, unlock it. if pressed again close menu. if pressed within JUST the menu, it will close the menu.
                if (ControllerLock)
                {
                    ControllerLock = false;
                    inOptions = false;
                }
                else
                    Continue();
            }

            //detect if slider is being changed, then go into slider changing mode
            if (Gamepad.current != null && Gamepad.current.leftStick.left.wasPressedThisFrame && inOptions)
            {
                ControllerSliding = true;
                ControllerLock = true;
            } else
            {
                ControllerSliding = false;
            }

        }
    }

    private void Start()
    {
        CUISliders[0].value = OptionData.SFXVOL;
        CUISliders[2].value = OptionData.MSTRVOL;
        CUISliders[1].value = OptionData.MSICVOL;
    }

    private void FixedUpdate()
    {
        OptionData.SFXVOL = CUISliders[0].value;
        OptionData.MSTRVOL = CUISliders[2].value;
        OptionData.MSICVOL = CUISliders[1].value;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    


    public void OpenOptions()
    {
            if(!OptionsMenu.activeSelf)
            OptionsMenu.SetActive(true);
            else
            OptionsMenu.SetActive(false);
    }

    public void ChangeQualityPreset(TMP_Dropdown dd)
    {
        if (dd.value == 3)
            QualitySettings.SetQualityLevel(0, true);
        if (dd.value == 2)
            QualitySettings.SetQualityLevel(1, true);
        if (dd.value == 1)
            QualitySettings.SetQualityLevel(2, true);
        if (dd.value == 0)
            QualitySettings.SetQualityLevel(3, true);
    }

    public void ChangeFullscreenMode(TMP_Dropdown dd)
    {
        if (dd.value == 0)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        if (dd.value == 1)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        if (dd.value == 2)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }


    public void ChangeSFXVolume(Slider slider)
    {
        if (ControllerMenu)
        {
            slider.wholeNumbers = true;
            AudioMixer.SetFloat("SFXVol", slider.value);
            slider.wholeNumbers = false;
        }    
        else
        {
            AudioMixer.SetFloat("SFXVol", slider.value);
        }
    }

    public void ChangeMusicVolume(Slider slider)
    {
        if (ControllerMenu)
        {
            slider.wholeNumbers = true;
            AudioMixer.SetFloat("MusicVol", slider.value);
            slider.wholeNumbers = false;
        }
        else
        {
            AudioMixer.SetFloat("MusicVol", slider.value);
        }
    }

    public void ChangeMasterVolume(Slider slider)
    {
        if (ControllerMenu)
        {
            slider.wholeNumbers = true;
            AudioMixer.SetFloat("MasterVol", slider.value);
            slider.wholeNumbers = false;
        }
        else
        {
            AudioMixer.SetFloat("MasterVol", slider.value);
        }
    }


    public void Continue()
    {
        UImenu.SetActive(false);
        OptionsMenu.SetActive(false);
        inOptions = false;
        ControllerLock = false;
        StarterAssets.StarterAssetsInputs.Menuopen = false;
        Escenabled = false;
        ControllerMenu = false;
    }

    public void ApplicationQuit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

   
}
