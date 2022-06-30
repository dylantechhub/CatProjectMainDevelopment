using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using Options;
using UnityEngine.Rendering.Universal;

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

    public bool LOADING_COMPLETE;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public Image LoadingScreenFader;
    public GameObject LoadingScreen;


    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3);
        float elapsedTime = 0.0f;
        Color c = LoadingScreenFader.color;
        while (elapsedTime < 2)
        {
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / 2);
            LoadingScreenFader.color = c;
            if (c.a < 0.2)
            {
                AudioMixer.SetFloat("MasterVol", OptionData.MSTRVOL);
                LoadingScreenFader.raycastTarget = false;
                LoadingScreenFader.maskable = false;
                LoadingScreen.SetActive(false);
            }
            yield return null;
        }
    }

    private UniversalAdditionalCameraData Cam;
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Cam = Camera.main.GetUniversalAdditionalCameraData();
        //load saved options

        StartCoroutine(FadeOut());
        LOADING_COMPLETE = true;

        StarterAssets.StarterAssetsInputs.Menuopen = false;
        LoadSavedSettings();

        UImenu.SetActive(true);
        StarterAssets.StarterAssetsInputs.Menuopen = true;
        Escenabled = true;


        UImenu.SetActive(false);
        StarterAssets.StarterAssetsInputs.Menuopen = false;
    }

    public GameObject DetailedWater;
    public GameObject nonDetailedWater;

    public TMP_Dropdown Quality;
    public TMP_Dropdown Fullscreen;

    public CloudsGen clouds;

    void LoadSavedSettings()
    {
        //load saved volume settings data
        CUISliders[0].value = OptionData.SFXVOL;
        CUISliders[2].value = OptionData.MSTRVOL;
        CUISliders[1].value = OptionData.MSICVOL;

        //load saved quality settings data
        if (OptionData.QUALITY_PRESET == 0)
            Quality.value = 3;
        else if (OptionData.QUALITY_PRESET == 1)
            Quality.value = 2;
        else if (OptionData.QUALITY_PRESET == 2)
            Quality.value = 1;
        else if (OptionData.QUALITY_PRESET == 3)
            Quality.value = 0;


        //load saved fullscreen settings data
            //i decided to remove this. lol.

        Screen.fullScreenMode = OptionData.FULLSCREEN_MODE;

        //load advanced options
        Cam.renderShadows = OptionData.SHADOWS;
        Cam.renderPostProcessing = OptionData.POST_PROCESSING;
        Cam.antialiasing = OptionData.ANTI_A_MODE;
        Cam.antialiasingQuality = OptionData.ANTI_A_QUALITY;

        if (OptionData.CLOUDS)
            clouds.enabled = true;
        else
            clouds.enabled = false;


        if (OptionData.DETAILED_WATER)
        {
            DetailedWater.SetActive(true);
            nonDetailedWater.SetActive(false);
        }
        else
        {
            DetailedWater.SetActive(false);
            nonDetailedWater.SetActive(true);
        }
    }

    private void Update()
    {
        if (ControllerMenu)
            Devices.text = $"Current Controls:\n Controller/Gamepad";
        else if (!ControllerMenu)
            Devices.text = $"Current Controls:\n Keyboard&Mouse";

        if (Keyboard.current.escapeKey.wasPressedThisFrame && !StarterAssets.StarterAssetsInputs.Menuopen)
        {
            UImenu.SetActive(true);
            StarterAssets.StarterAssetsInputs.Menuopen = true;
            Escenabled = true;
        } else if (Keyboard.current.escapeKey.wasPressedThisFrame && StarterAssets.StarterAssetsInputs.Menuopen)
        {
            Continue();
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

    private void FixedUpdate()
    {

            //set saved screen settings
            OptionData.FULLSCREEN_MODE = Screen.fullScreenMode;

            //set saved quality settings
            OptionData.QUALITY_PRESET = QualitySettings.GetQualityLevel();

            //set saved Anti-Aliasing settings
            OptionData.ANTI_A_MODE = Cam.antialiasing;
            OptionData.ANTI_A_QUALITY = Cam.antialiasingQuality;

            //set saved Post-Processing settings
            OptionData.POST_PROCESSING = Cam.renderPostProcessing;

            //set saved Clouds settings
            OptionData.CLOUDS = clouds.enabled;

            //set saved Shadows settings
            OptionData.SHADOWS = Cam.renderShadows;

            //set saved Detailed Water settings
            OptionData.DETAILED_WATER = DetailedWater.activeSelf;
        
        OptionData.CurrentSavedScene = SceneManager.GetActiveScene().name;
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
        //very low
        if (dd.value == 3)
        {
            QualitySettings.SetQualityLevel(0, true);
            Cam.antialiasing = AntialiasingMode.None;
        }
        //low
        if (dd.value == 2)
        {
            QualitySettings.SetQualityLevel(1, true);
            Cam.antialiasingQuality = AntialiasingQuality.Low;
        }
        //medium
        if (dd.value == 1)
        {
            Cam.antialiasingQuality = AntialiasingQuality.Medium;
            QualitySettings.SetQualityLevel(2, true);
        }
        //ultra
        if (dd.value == 0)
        {
            Cam.antialiasingQuality = AntialiasingQuality.High;
            QualitySettings.SetQualityLevel(3, true);
        }
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
    public ToScene TS;
    public void ToMainMenu()
    {
        TS.Auto();
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
