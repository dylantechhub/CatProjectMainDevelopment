//data file to transfer options between scenes & save game files.
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using System;
using Options;
using System.Collections.Generic;
using System.Collections;

namespace DataHandler
{
    class Data : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        static public void RefreshOptionsWithDataHandlerUpdate(OPTIONDATAHANDLERS data, MainMenu_UI Menu)
        {
            OptionData.SFXVOL = data.SFXVOL;
            OptionData.MSICVOL = data.MSICVOL;
            OptionData.MSTRVOL = data.MSTRVOL;
            OptionData.CurrentSavedScene = data.CurrentSavedScene;
            OptionData.ANTI_A_MODE = data.ANTI_A_MODE;
            OptionData.ANTI_A_QUALITY = data.ANTI_A_QUALITY;
            OptionData.POST_PROCESSING = data.POST_PROCESSING;
            OptionData.FULLSCREEN_MODE = data.FULLSCREEN_MODE;
            OptionData.QUALITY_PRESET = data.QUALITY_PRESET;
            OptionData.CLOUDS = data.CLOUDS;
            OptionData.DETAILED_WATER = data.DETAILED_WATER;
            OptionData.SHADOWS = data.SHADOWS;
            //refresh the cache for all settings & options
            Menu.LoadSavedSettings();
            Debug.Log("GameSave Loaded. Data Transfered!");
            if (Menu.LoadingSaveModal.activeSelf)
                Menu.StartCoroutine(Menu.CloseModal());
            else
            {
                Menu.StartCoroutine(Menu.FadeIn());
                Menu.StartCoroutine(Menu.LoadIntoSavedScene(OptionData.CurrentSavedScene));
            }
        }
    }
}

namespace Options
{
    [System.Serializable]
    public class OPTIONDATAHANDLERS
    {

        public OPTIONDATAHANDLERS(float sfx, float master, float music, string cs, AntialiasingMode am, AntialiasingQuality aq, bool p_prc, FullScreenMode fm, int qp, bool clouds, bool dt_water, bool shadows)
        {
            SFXVOL = sfx;
            MSTRVOL = master;
            MSICVOL = music;
            CurrentSavedScene = cs;
            ANTI_A_MODE = am;
            ANTI_A_QUALITY = aq;
            POST_PROCESSING = p_prc;
            FULLSCREEN_MODE = fm;
            QUALITY_PRESET = qp;
            CLOUDS = clouds;
            DETAILED_WATER = dt_water;
            SHADOWS = shadows;
        }

        public float SFXVOL { get; set; } = -23;

        public float MSTRVOL { get; set; } = 0;

        public float MSICVOL { get; set; } = 0;

        public string CurrentSavedScene { get; set; }

        //quality-graphics savers
        public AntialiasingMode ANTI_A_MODE { get; set; }

        public AntialiasingQuality ANTI_A_QUALITY { get; set; }

        public bool POST_PROCESSING { get; set; } = true;

        //option savers

        public FullScreenMode FULLSCREEN_MODE { get; set; }

        public int QUALITY_PRESET { get; set; } = 2;

        public bool CLOUDS { get; set; } = true;

        public bool DETAILED_WATER { get; set; } = true;

        public bool SHADOWS { get; set; } = true;
        
    }

    [System.Serializable]
    public class OptionData
    {

        public static float SFXVOL { get; set; } = -23;

        public static float MSTRVOL { get; set; } = 0;

        public static float MSICVOL { get; set; } = 0;


        public static string CurrentSavedScene { get; set; }

        //quality-graphics savers
        public static AntialiasingMode ANTI_A_MODE { get; set; }

        public static AntialiasingQuality ANTI_A_QUALITY { get; set; }

        public static bool POST_PROCESSING { get; set; } = true;

        //option savers

        public static FullScreenMode FULLSCREEN_MODE { get; set; }

        public static int QUALITY_PRESET { get; set; } = 2;

        public static bool CLOUDS { get; set; } = true;

        public static bool DETAILED_WATER { get; set; } = true;

        public static bool SHADOWS { get; set; } = true;

    }
}