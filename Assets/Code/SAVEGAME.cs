using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Options;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class SAVEGAME : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        SaveFile();
    }
    private void Start()
    {
        SaveFile();
    }

    public bool CreateManualSave;
    private void Update()
    {
        if (CreateManualSave)
        {
            CreateManualSave = false;
            SaveFile();
        }
    }

    public void SaveFile()
    {
        Debug.Log("Attempting Game Save before Shutdown...");
        string destination = Application.dataPath + "/GameSave/SavedGame.save";
        FileStream file;

        if (File.Exists(destination)) 
            File.Delete(destination);

        file = File.Create(destination);

        Debug.Log("Writing SaveFile...");
        OPTIONDATAHANDLERS Data = new OPTIONDATAHANDLERS(OptionData.SFXVOL, OptionData.MSTRVOL, OptionData.MSICVOL, OptionData.CurrentSavedScene, OptionData.ANTI_A_MODE, OptionData.ANTI_A_QUALITY, OptionData.POST_PROCESSING, OptionData.FULLSCREEN_MODE, OptionData.QUALITY_PRESET, OptionData.CLOUDS, OptionData.DETAILED_WATER, OptionData.SHADOWS);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, Data);
        file.Close();
        Debug.Log("SaveFile Written & Serialized!");
    }

}
