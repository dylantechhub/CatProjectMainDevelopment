using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public bool DEBUG_MODE = true;

    public GameObject[] allCameras;


    // Start is called before the first frame update
    void Start()
    {
        if(allCameras.Length == 0)
        {
            Debug.Log("No Cameras Found");
        }
        else
        {
            allCameras[0].SetActive(true);
            for(int i = 1; i < allCameras.Length; i++)
            {
                allCameras[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (DEBUG_MODE)
        {
            if (Keyboard.current.digit6Key.IsPressed())
            {
                ToggleToCamera(0);
            }

            if (Keyboard.current.digit7Key.IsPressed())
            {
                ToggleToCamera(1);
            }

            if (Keyboard.current.digit8Key.IsPressed())
            {
                ToggleToCamera(2);
            }

        }
    }

    public void ToggleToCamera(int camID)
    {
        if (camID >= allCameras.Length)
        {
            Debug.LogWarning("Can''t switch to camera " + camID);
        }
        else
        {
            for(int i =0; i < allCameras.Length; i++)
            {
                allCameras[i].SetActive(false);
            }
            allCameras[camID].SetActive(true);
        }
    }
}
