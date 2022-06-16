using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlatformController;
using PlatformsData;

public class RotatingPlatform : MonoBehaviour
{
    private static string RotatingPlatformID;

    private bool RotateSwitch;

    /// <summary>
    /// Change the local speed of the platform
    /// </summary>
    [HideInInspector]
    public float LocalRotationSpeed { get; set; }

    private bool GlobalDebugMode;

    private bool stop;

    private bool SLEEP_MODE;

    public void SetRPID(string ID, PlatformController PC)
    {
        if (!SLEEP_MODE)
        {
        if (ID != "")
            RotatingPlatformID = ID;
        else if (PC.GlobalDebugMode)
            Debug.LogError($"[RPC] ERROR! Rotating platform named {this.gameObject.name} does not have a set ID!");
        }
        else
            Debug.LogError($"[{name}@SLEEPMODE] ERROR! The platform you are trying to remotely edit has been changed. This platform is no longer taking remote requests.\n Try refreshing the platform-to-edit's gameobject.");

    }

    public void EnablePlatform(bool rotateSwitch, float localRotationSpeed, bool globalDebugMode)
    {
        if (!SLEEP_MODE)
        {
        //enable

        GlobalDebugMode = globalDebugMode;

        if (GlobalDebugMode)
        {
            print("[RPC]: Global-Debug mode enabled!");
            print($"[RPC]: Platform {RotatingPlatformID} Enabled/Updated!");
        }

        LocalRotationSpeed = localRotationSpeed;
        RotateSwitch = rotateSwitch;


        if (RotateSwitch)
            GoCounterClockWise();
        else
            GoClockwise();
        }
        else
            Debug.LogError($"[{name}@SLEEPMODE] ERROR! The platform you are trying to remotely edit has been changed. This platform is no longer taking remote requests.\n Try refreshing the platform-to-edit's gameobject.");
    }

    /// <summary>
    /// Rotates the rotating platform clockwise
    /// </summary>
    public void GoClockwise()
    {
        if (!SLEEP_MODE)
        {
        cw = true;
        }
        else
            Debug.LogError($"[{name}@SLEEPMODE] ERROR! The platform you are trying to remotely edit has been changed. This platform is no longer taking remote requests.\n Try refreshing the platform-to-edit's gameobject.");

    }

    /// <summary>
    /// Retrieves the platforms ID
    /// </summary>
    public string ID()
    {
        if (!SLEEP_MODE)
        {
            return RotatingPlatformID;
        }
        else
        {
            Debug.LogError($"[{name}@SLEEPMODE] ERROR! The platform you are trying to remotely edit has been changed. This platform is no longer taking remote requests.\n Try refreshing the platform-to-edit's gameobject.");
            return null;
        }
    }

    /// <summary>
    /// Rotates the rotating platform counter-clickwise
    /// </summary>
    public void GoCounterClockWise()
    {
        if (!SLEEP_MODE)
        {
            cw = false;
        }
        else
            Debug.LogError($"[{name}@SLEEPMODE] ERROR! The platform you are trying to remotely edit has been changed. This platform is no longer taking remote requests.\n Try refreshing the platform-to-edit's gameobject.");
    }

    private bool cw;
    public void FixedUpdate()
    {
        //rotates the platform n 
        if (cw && !stop && !SLEEP_MODE)
            transform.Rotate(Vector3.up, 50 * LocalRotationSpeed);
        else if (!stop && !SLEEP_MODE)
            transform.Rotate(Vector3.up, -50 * LocalRotationSpeed);

        if (SLEEP_MODE)
        {
            //bedtime!
            stop = true;
        }
    }

    /// <summary>
    /// Pauses the Rotating Platform
    /// </summary>
    public void Pause()
    {
        if (!SLEEP_MODE)
            stop = true;
    }

    /// <summary>
    /// Resumes the Rotating Platform
    /// </summary>
    public void Resume()
    {
        if (!SLEEP_MODE)
            stop = false;
        else
            Debug.LogError($"[{name}@SLEEPMODE] ERROR! The platform you are trying to remotely edit has been changed. This platform is no longer taking remote requests.\n Try refreshing the platform-to-edit's gameobject.");
    }

    /// <summary>
    /// Returns a "clockwise" or "counter-clockwise" string, depending on what direction the platforms rotating in
    /// </summary>
    public string RotatingDirection()
    {
        if (!SLEEP_MODE)
        {
            string rotating;
            if (cw) rotating = "clockwise"; else rotating = "counter-clockwise";
            return rotating;
        }
        else
        {
            Debug.LogError($"[{name}@SLEEPMODE] ERROR! The platform you are trying to remotely edit has been changed. This platform is no longer taking remote requests.\n Try refreshing the platform-to-edit's gameobject.");
            return null;
        }
    }

    /// <summary>
    /// Returns true if the platform is stopped
    /// </summary>
    public bool isPaused()
    {
        if (!SLEEP_MODE)
            return stop;
        else
        {
            Debug.LogError($"[{name}@SLEEPMODE] ERROR! The platform you are trying to remotely edit has been changed. This platform is no longer taking remote requests.\n Try refreshing the platform-to-edit's gameobject.");
            return false;
        }
    }

    /// <summary>
    /// Removes this platform from the list, and scene
    /// </summary>
    public void Remove()
    {
        if (!SLEEP_MODE)
        {
            foreach (var p in PlatformControllerScript.Script.rotatingPlatforms)
            {
                if (p.ID == RotatingPlatformID)
                {
                    gameObject.SetActive(false);
                    if (GlobalDebugMode)
                        Debug.LogWarning("[RPC] WARNING: Removing RP " + RotatingPlatformID + "\n...Ending \"foreach\", as collection has been modified");
                    PlatformControllerScript.Script.rotatingPlatforms.Remove(p);
                }
            }
        }
        else
            Debug.LogError($"[{name}@SLEEPMODE] ERROR! The platform you are trying to remotely edit has been changed. This platform is no longer taking remote requests.\n Try refreshing the platform-to-edit's gameobject.");
    }

    /// <summary>
    /// Changes the rotating platforms GameObject
    /// </summary>
    public void Change(GameObject switchTo)
    {
        if (!SLEEP_MODE)
        {
            foreach (var p in PlatformControllerScript.Script.rotatingPlatforms)
            {
                if (p.ID == RotatingPlatformID)
                {
                    if (GlobalDebugMode)
                        Debug.LogWarning("[RPC] Changed RP GameObject For RPID: " + RotatingPlatformID + " To: " + switchTo.name);
                    //set local gameobject in options
                    p.rotationPlatform = switchTo;
                    //remove script from list, as its now in SleepMode
                    PlatformController.RPSCRIPTS.Remove(this);
                    //enable globally, and transfer all current settings and vars, to the new GameObject.

                    //if it already has a rotating platform script (could be in SLEEP_MODE) then destroy it
                    if (switchTo.GetComponent<RotatingPlatform>())
                        Destroy(switchTo.GetComponent<RotatingPlatform>());

                        switchTo.AddComponent<RotatingPlatform>().EnablePlatform(RotateSwitch, LocalRotationSpeed, GlobalDebugMode);
                    switchTo.GetComponent<RotatingPlatform>().SetRPID(RotatingPlatformID, PlatformControllerScript.Script);
                    PlatformController.RPSCRIPTS.Add(switchTo.GetComponent<RotatingPlatform>());
                    //Signify to this script that its now in Sleep Mode, which means its only job is when a non-refreshed
                    //game object attempts a remote change, send an error saying this rotating platform was changed.
                    SLEEP_MODE = true;
                }
            }
        }
        else
            Debug.LogError($"[{name}@SLEEPMODE] ERROR! The platform you are trying to remotely edit has been changed. This platform is no longer taking remote requests.\n Try refreshing the platform-to-edit's gameobject.");

    }
}
