using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneLoader : MonoBehaviour
{

    public int targetScene = 0;

    public bool DEBUG_MODE = true;


    public void Update()
    {
        if (DEBUG_MODE)
        {
            if (Keyboard.current.digit1Key.IsPressed())
            {
                SceneManager.LoadScene(0);
            }

            if(Keyboard.current.digit2Key.IsPressed())
            {
                SceneManager.LoadScene(1);
            }

            if (Keyboard.current.digit3Key.IsPressed())
            {
                SceneManager.LoadScene(2);
            }

            if (Keyboard.current.digit0Key.IsPressed())
            {
                SceneManager.LoadScene(3);
            }
        } 
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(targetScene);
        }
    }


}
