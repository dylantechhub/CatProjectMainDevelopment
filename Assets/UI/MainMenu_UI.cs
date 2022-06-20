using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Options;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem.XR;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class MainMenu_UI : MonoBehaviour
{

    public Button ContinueBtn;
    public GameObject SettingsMenu;
    public GameObject MainMenu;
    public AudioMixer Audio;
    public TextMeshProUGUI Inputs;
    public static bool LOADING_COMPLETE;
    public TMP_Dropdown Fullscreen;
    public TMP_Dropdown Quality;

    public Toggle AA;
    public Toggle PP;
    public Toggle SD;
    public Toggle DW;
    public Toggle CL;

    private void Start()
    {
        //OptionData.CurrentSavedScene = SceneManager.GetActiveScene();

        Cam = Camera.main.GetUniversalAdditionalCameraData();

        if (File.Exists(Application.dataPath+"/GameSave/SavedGame.save"))
        {
            ContinueBtn.gameObject.SetActive(true);
        } else
        {
            ContinueBtn.gameObject.SetActive(false);
        }

        LoadSavedSettings();
        Music.interactable = true;
        Master.interactable = true;
        SFX.interactable = true;
    }

    public void LoadSavedSettings()
    {
        //load saved volume settings data
        SFX.value = OptionData.SFXVOL;
        Master.value = OptionData.MSTRVOL;
        Music.value = OptionData.MSICVOL;

        //load saved quality settings data
        if (OptionData.QUALITY_PRESET == 0)
            Quality.value = 3;
        else if (OptionData.QUALITY_PRESET == 1)
            Quality.value = 2;
        else if (OptionData.QUALITY_PRESET == 2)
            Quality.value = 1;
        else if (OptionData.QUALITY_PRESET == 3)
            Quality.value = 0;


        //load adnvaced saved options
        if (OptionData.SHADOWS)
            SD.isOn = true;
        else
            SD.isOn = false;

        if (OptionData.POST_PROCESSING)
            PP.isOn = true;
        else
            PP.isOn = false;

        if (OptionData.ANTI_A_MODE != AntialiasingMode.None)
            AA.isOn = true;
        else
            AA.isOn = false;

        if (OptionData.DETAILED_WATER)
            DW.isOn = true;
        else
            DW.isOn = false;

        if (OptionData.CLOUDS)
            CL.isOn = true;
        else
            CL.isOn = false;


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


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        MainMenu.gameObject.SetActive(true);
        SettingsMenu.gameObject.SetActive(false);
        if (!Directory.Exists(Application.dataPath + "/GameSave"))
            Directory.CreateDirectory(Application.dataPath + "/GameSave");
        LOADING_COMPLETE = true;
    }


    public void GetInputDevices()
    {
        var g = "NON";
        var k = "NON";
        var j = "NON";
        var m = "NON";
        var t = "NON";
        var xrl = "NON";
        var xrr = "NON";
        if (Gamepad.current != null)
            g = Gamepad.current.name;
        if (Keyboard.current != null)
            k = Keyboard.current.name;
        if (Joystick.current != null)
            j = Joystick.current.name;
        if (Mouse.current != null)
            m = Mouse.current.name;
        if (Touchscreen.current != null)
            t = Touchscreen.current.name;
        if (XRController.leftHand != null)
            xrl = XRController.leftHand.name;
        if (XRController.rightHand != null)
            xrr = XRController.rightHand.name;
        Inputs.text = $"Keyboard: {k}, Controller: {g}, Joystick: {j}, Mouse: {m}, Touch: {t}, XR-L: {xrl}, XR-R: {xrr}";
    }

    public bool DEBUGMODE = true;

    private UniversalAdditionalCameraData Cam;
    public void AntiAliasing(Toggle tog)
    {
        // if antialiasing is off, turn it on, and if the current quality level/preset is very-low, then keep it off.
        if (Cam.antialiasing == AntialiasingMode.None && QualitySettings.GetQualityLevel() != 0)
        {
            if (DEBUGMODE)
                Debug.Log("Anti-Aliasing On");
            tog.isOn = true;
            Cam.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        }
        else if (QualitySettings.GetQualityLevel() != 0)
        {
            if (DEBUGMODE)
                Debug.Log("Anti-Aliasing Off");
            tog.isOn = false;
            Cam.antialiasing = AntialiasingMode.None;
        } else if (QualitySettings.GetQualityLevel() == 0)
        {
            if (DEBUGMODE)
                Debug.LogWarning("Anti-Aliasing is unable to be turned on in Quality Preset \"Very Low\"!");
            //force toggle to off
            tog.isOn = false;
        }
    }

    public void Shadows(Toggle tog)
    {
        if (!Cam.renderShadows && QualitySettings.GetQualityLevel() != 0)
        {
            if (DEBUGMODE)
                Debug.Log("Shadows On");
            tog.isOn = true;
            Cam.renderShadows = true;
        }
        else if (QualitySettings.GetQualityLevel() != 0)
        {
            if (DEBUGMODE)
                Debug.Log("Shadows Off");
            tog.isOn = false;
            Cam.renderShadows = false;
        }
        else if (QualitySettings.GetQualityLevel() == 0)
        {
            if (DEBUGMODE)
                Debug.LogWarning("Shadows are unable to be turned on in Quality Preset \"Very Low\"!");
            //force toggle to off
            tog.isOn = false;
        }
    }
    public CloudsGen clouds;
    public void Clouds()
    {
        if (clouds.enabled)
            clouds.enabled = false;
        else
            clouds.enabled = true;
    }

    public GameObject DetailedWater;
    public GameObject nonDetailedWater;
    public void SetDetailedWater(Toggle tog)
    {
        if (!DetailedWater.activeSelf)
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

    public void PostProcessing(Toggle tog)
    {
        if (!Cam.renderPostProcessing && QualitySettings.GetQualityLevel() != 0)
        {
            if (DEBUGMODE)
                Debug.Log("Enabled Post-Processing");
            Cam.renderPostProcessing = true;
            tog.isOn = true;
        }
        else if (QualitySettings.GetQualityLevel() != 0)
        {
            if (DEBUGMODE)
                Debug.Log("Disabled Post-Processing");
            Cam.renderPostProcessing = false;
            tog.isOn = false;
        } else if (QualitySettings.GetQualityLevel() == 0)
        {
            if (DEBUGMODE)
                Debug.LogWarning("Post-Processing is unable to be turned on in Quality Preset \"Very Low\"!");
            //force toggle to off
            tog.isOn = false;
        }
    }

    public void ChangeFullscreenMode(TMP_Dropdown dd)
    {
        if (dd.value == 0)
        {
            if (DEBUGMODE)
                Debug.Log("Windowed-Fullscreen Mode");
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        if (dd.value == 1)
        {
            if (DEBUGMODE)
                Debug.Log("Fullscreen Mode");
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        if (dd.value == 2)
        {
            if (DEBUGMODE)
                Debug.Log("Windowed");
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

        public void ChangeQualityPreset(TMP_Dropdown dd)
    {
        //very low
        if (dd.value == 3)
        {
            if (DEBUGMODE)
                Debug.Log("Anti-Aliasing DISABLED, Quality Preset Set To VERY LOW");
            QualitySettings.SetQualityLevel(0, true);
            Cam.antialiasing = AntialiasingMode.None;
        }
        //low
        if (dd.value == 2)
        {
            if (DEBUGMODE)
                Debug.Log("Anti-Aliasing-Quality LOW, Quality Preset Set To LOW");
            QualitySettings.SetQualityLevel(1, true);
            Cam.antialiasingQuality = AntialiasingQuality.Low;
        }
        //medium
        if (dd.value == 1)
        {
            if (DEBUGMODE)
                Debug.Log("Anti-Aliasing-Quality MEDIUM, Quality Preset Set To MEDIUM");
            Cam.antialiasingQuality = AntialiasingQuality.Medium;
            QualitySettings.SetQualityLevel(2, true);
        }
        //ultra
        if (dd.value == 0)
        {
            if (DEBUGMODE)
                Debug.Log("Anti-Aliasing-Quality HIGH, Quality Preset Set To ULTRA");
            Cam.antialiasingQuality = AntialiasingQuality.High;
            QualitySettings.SetQualityLevel(3, true);
        }
    }

    //save all option data to Data.cs (non monobehaviour script)
    private void FixedUpdate()
    {
        //set saved audio settings
        OptionData.SFXVOL = SFX.value;
        OptionData.MSICVOL = Music.value;
        OptionData.MSTRVOL = Master.value;

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
    }

    public void Continue(Button button)
    {
        if (DEBUGMODE)
            Debug.Log("Checking For Save File...");

        if (File.Exists(Application.dataPath + "/GameSave/SavedGame.save"))
        {
            if (DEBUGMODE)
                Debug.Log("Save File Located, Initializing File Save Load...");

            //Initializes the load game script to start loading in all saved data to the users current session.
            LOADSAVEGAME.LOADGAME = true;

        } else
        {
            if (DEBUGMODE)
                Debug.Log("No Saved Game Detected! (LOCAL_PATH: \"Assets/GameSave/SavedGame.save\"");
        }
    }
    public Button StartBtn;
    public Image LoadingScreenFader;
    public GameObject LoadingScreen;
    public GameObject LoadingSaveModal;

    public void Play(Button button)
    {
        StartCoroutine(FadeIn());
        StartCoroutine(Loading());
    }

    public IEnumerator FadeIn()
    {
        var targetAlpha = 1.0f;
        Color curColor = LoadingScreenFader.color;
        while (Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)
        {
            var lerp = Mathf.Lerp(curColor.a, targetAlpha, 1 * Time.deltaTime);
            Audio.SetFloat("MasterVol", 80/-lerp );
            curColor.a = lerp;
            LoadingScreenFader.color = curColor;
            if (curColor.a > 0.9)
            {
                LoadingScreenFader.raycastTarget = true;
                LoadingScreenFader.maskable = true;
                LoadingScreen.SetActive(true);
                LoadingScreenFader.gameObject.transform.SetParent(transform);
            }
            yield return null;
        }
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(4);

        SceneManager.LoadScene("Level 1 Scene");

        yield return null;
    }


    public IEnumerator CloseModal()
    {
        yield return new WaitForSeconds(2);
        LoadingSaveModal.SetActive(false);
        yield return null;
    }

    public IEnumerator LoadIntoSavedScene(string LoadInto)
    {
        yield return new WaitForSeconds(4);

        SceneManager.LoadScene(LoadInto);

        yield return null;
    }

    public Button Tut;
    public GameObject TutorialScene;

    public void Tutorial(Button btn)
    {
        if (!TutorialScene.activeSelf)
        {
            TutorialScene.SetActive(true);
            SettingsMenu.SetActive(false);
            MainMenu.SetActive(false);
            Tut.Select();
        }
    }
    public GameObject Tutorial1;
    public GameObject Tutorial2;
    public GameObject Tutorial3;
    public GameObject Tutorial4;
    private bool tutEnd;

    public void TutorialOkay(Button btn)
    {
        if (Tutorial1.activeSelf)
            Tutorial1.SetActive(false);
        else if (Tutorial2.activeSelf)
            Tutorial2.SetActive(false);
        else if (Tutorial3.activeSelf)
            Tutorial3.SetActive(false);
        else if (Tutorial4.activeSelf)
        {
            Tutorial4.SetActive(false);
            tutEnd = true;
        }
        else if (tutEnd)
        {
            MainMenu.SetActive(true);
            TutorialScene.SetActive(false);
            Tutorial1.SetActive(true);
            Tutorial2.SetActive(true);
            Tutorial3.SetActive(true);
            Tutorial4.SetActive(true);
            StartBtn.Select();
        }

    }

    public void LoadSavedData()
    {
        if (File.Exists(Application.dataPath + "/GameSave/SavedGame.save"))
        {
            LOADSAVEGAME.LOADGAME = true;

            //the OptionDataHandeler will set this Modal to false once the Data Loading is completed.
            LoadingSaveModal.SetActive(true);
        }
        else
            Debug.Log("No save data was found to load.");
    }
    

    public void Settings(Button button)
    {
        if (SettingsMenu.activeSelf)
            SettingsMenu.SetActive(false);
        else
            SettingsMenu.SetActive(true);
    }

    public void Quit(Button button)
    {
        if (DEBUGMODE)
            Debug.Log("Quit");
        Application.Quit();
    }



    //volume setters
    public Slider Music; public Slider Master; public Slider SFX;
    public void SetSFXVolume(Slider slider)
    {
        if (Gamepad.current != null)
        {
            if (DEBUGMODE)
                Debug.Log("SFX Volume Set To "+slider.value+" Via Gamepad");
            slider.wholeNumbers = true;
            Audio.SetFloat("SFXVol", slider.value);
            slider.wholeNumbers = false;
        }
        else
        {
            if (DEBUGMODE)
                Debug.Log("SFX Volume Set To " + slider.value);
            Audio.SetFloat("SFXVol", slider.value);
        }
    }

    public void SetMasterVolume(Slider slider)
    {
        if (DEBUGMODE)
            Debug.Log("Master Volume Set To " + slider.value + " Via Gamepad");
        if (Gamepad.current != null)
        {
            slider.wholeNumbers = true;
            Audio.SetFloat("MasterVol", slider.value);
            slider.wholeNumbers = false;
        }
        else
        {
            if(DEBUGMODE)
            Debug.Log("Master Volume Set To " + slider.value);
            Audio.SetFloat("MasterVol", slider.value);
        }
    }

    public void SetMusicVolume(Slider slider)
    {
        if (DEBUGMODE)
            Debug.Log("Music Volume Set To " + slider.value + " Via Gamepad");
        if (Gamepad.current != null)
        {
            slider.wholeNumbers = true;
            Audio.SetFloat("MusicVol", slider.value);
            slider.wholeNumbers = false;
        }
        else
        {
            if (DEBUGMODE)
                Debug.Log("Music Volume Set To " + slider.value);
            Audio.SetFloat("MusicVol", slider.value);
        }
    }
}
