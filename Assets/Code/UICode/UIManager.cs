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
    public AudioMixer AudioMixer;
    public bool inOptions;

    [Header("UI stuff")]
    public GameObject UImenu;
    public GameObject OptionsMenu;

    [Header("Controller UI")]
    public List<Button> CUIbuttons = new List<Button>();
    public List<Slider> CUISliders = new List<Slider>();
    public int currentbutton = 0;
    public int currentOption = 44444;
    //lock controller controls for uses for other options
    public bool ControllerLock;

    //if the controller is controlling a slider of some sort.
    public bool ControllerSliding;


    //Keybinding stuff
    private bool Escenabled = false;
    private bool ControllerMenu = false;

    
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UImenu.SetActive(true);
            StarterAssets.StarterAssetsInputs.Menuopen = true;
            Escenabled = true;
        }

        //controller menu open
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame && !ControllerLock)
        {
            UImenu.SetActive(true);
            //changed the variable Menuopen to static, allowing this. no refrence needs to be made to change this variable.
            StarterAssets.StarterAssetsInputs.Menuopen = true;
            Escenabled = true;
            ControllerMenu = true;
        }

        //if menu opened
        if (Escenabled)
        {

            //sort through menu up
            if (Gamepad.current != null && Gamepad.current.leftStick.up.wasPressedThisFrame && !ControllerLock && !inOptions)
                currentbutton--;
                if (currentbutton < 0)
                    currentbutton = 3;

            //sort through menu down
            if (Gamepad.current != null && Gamepad.current.leftStick.down.wasPressedThisFrame && !ControllerLock && !inOptions)
                currentbutton++;
                if (currentbutton > 3)
                    currentbutton = 0;

            //if controller went left to options menu, then allow changing of the options
            if (Gamepad.current != null && Gamepad.current.leftStick.left.wasPressedThisFrame && !ControllerLock && !inOptions)
            {
                currentbutton = 44444;
                inOptions = true;
                //options: 0: SFXVol, 1: MSTRVol, 2: MSICVol,
                currentOption = 0;
            }

            //if controller went right to menu from options, then allow menu selection
            if (Gamepad.current != null && Gamepad.current.leftStick.right.wasPressedThisFrame && !ControllerLock && inOptions)
            {
                //options: 0: continue, 1: restart, 2: options, 3: quit
                currentbutton = 3;
                inOptions = false;
                currentOption = 44444;
            }


            //back button
            if (Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame)
            {
                //if controller is locked when b is pressed, unlock it. if pressed again close menu. if pressed within JUST the menu, it will close the menu.
                if (ControllerLock)
                {
                    ControllerLock = false;
                    currentbutton = 3;
                    inOptions = false;
                    currentOption = 44444;
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


            if (!inOptions && !ControllerLock)
            if (currentbutton==0)CUIbuttons[currentbutton].Select();if(currentbutton==1)CUIbuttons[currentbutton].Select();if(currentbutton==2)CUIbuttons[currentbutton].Select();if(currentbutton==3)CUIbuttons[currentbutton].Select();

            if (inOptions)
            {
                //sort through options up
                if (Gamepad.current != null && Gamepad.current.leftStick.up.wasPressedThisFrame && !ControllerLock)
                    currentOption++;
                    if (currentOption > 2)
                        currentOption = 0;

                //sort through options down
                if (Gamepad.current != null && Gamepad.current.leftStick.down.wasPressedThisFrame && !ControllerLock)
                    currentOption--;
                if (currentOption < 0)
                    currentOption = 2;



            }
        }
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
