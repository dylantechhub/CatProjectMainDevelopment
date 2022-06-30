using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using  UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class Debugging : MonoBehaviour
{

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        if (Gamepad.current.name == "DualShock4GamepadHID" || Gamepad.current.name == "DualShock4GamepadHID2" || Gamepad.current.name == "DualShock4GamepadHID3")
        {
            InputSystem.RemoveDevice(Gamepad.current);
        }
    }
}
