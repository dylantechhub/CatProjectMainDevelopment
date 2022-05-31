using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpawnPoint : MonoBehaviour
{
    public Transform cat;
    public CharacterController controller;

    public float timer = 0;
    public float fastestTime  = 10000000f;

    public Text timerUI;

    public bool isTimerRunning = false;

    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("Resetting Level");
            
            controller.enabled = false;
            cat.position = this.transform.position;
            controller.enabled = true;

            StartTimer();
        }

        timer += Time.deltaTime;
        timerUI.text = timer.ToString("n2");

        if(Keyboard.current.escapeKey.IsPressed())
        {
            Application.Quit(0);
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        timer = 0;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        if (timer < fastestTime)
        {
            fastestTime = timer;
        }
    }
}
