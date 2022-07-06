using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MobileController : MonoBehaviour
{


    public GameObject MobileControls;

    public List<GameObject> Buttons = new List<GameObject>();



    private void Start()
    {
        IsMobile();

        foreach (var b in Buttons)
        {
            b.gameObject.AddComponent<ButtonEvent>();
        }

    }


    void IsMobile() {
        if (Touchscreen.current != null)
        {
            //touchscreen exists
            MobileControls.SetActive(true);

        }
    }
    }