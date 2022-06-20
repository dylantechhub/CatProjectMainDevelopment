using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Options;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using DataHandler;

public class LOADSAVEGAME : MonoBehaviour
{
    public MainMenu_UI Menu;
    public static bool LOADGAME;

    private void Update()
    {
        if (LOADGAME)
            LoadFile();
        LOADGAME = false;
    }


    public void LoadFile()
    {
        Debug.Log("Attempting to load last Game Save File...");
        string destination = Application.dataPath + "/GameSave/SavedGame.save";
        FileStream file;

        if (File.Exists(destination)) 
            file = File.OpenRead(destination);
        else
        {
            Debug.LogError("Game Save not found (PATH: "+ Application.dataPath + "/GameSave/[SavedGame.save]");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        OPTIONDATAHANDLERS data = (OPTIONDATAHANDLERS)bf.Deserialize(file);
        Debug.Log("Save Data Received & Deserialized");
        file.Close();
       
        //load the options from save file, and update the system to use them, then do a refresh.
        Debug.Log("Refreshing options with the new data");

        Data.RefreshOptionsWithDataHandlerUpdate(data, Menu);

    }
    
}
