using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private float Delay;
    private float gpms;
    private bool X;
    private bool Y;
    private bool Z;
    private bool stop;

    private float posX;
    private float posY;
    private float posZ;

    private static bool forward;

    private static bool reverse;

    private bool UsingTrigger;

    private bool GlobalDebugMode;

    [Space(10)]
    [SerializeField]
    public static string MovingPlatformID;
    public void SetMPID(string ID) {
        if (ID != "")
            MovingPlatformID = ID;
            else
            Debug.LogError($"[MPC] ERROR! Moving platform named {this.gameObject.name} does not have a set ID!");
    }

    private List<string> METHODS = new List<string>();


    private bool Direction;

    //the PlatformController calls this function to enable the platform.
    public void EnablePlatform(float d, float gpmsPC, string METHOD_X, string METHOD_Y, string METHOD_Z, bool DIRECTION, bool usingTrigger, bool GDM)
    {
        GlobalDebugMode = GDM;
        UsingTrigger = usingTrigger;
        Direction = DIRECTION;
            METHODS.Add(METHOD_X);
            METHODS.Add(METHOD_Y);
            METHODS.Add(METHOD_Z);

        if (GlobalDebugMode) {
            print("[MPC]: Global-Debug mode enabled!");
            print($"[MPC]: Platform {MovingPlatformID} Enabled/Updated! Using Methods: "+METHOD_Z+ METHOD_Y+ METHOD_X+$" TriggerMode?: "+usingTrigger); 
        }
        
        if(METHODS.Contains("X"))
        {
            X = true;
        } 
        if (METHODS.Contains("Y"))
        {
            Y = true;
        } 
        if (METHODS.Contains("Z"))
        {
            Z = true;
        }
        Delay = d;
        gpms = gpmsPC;
        forward = true;
    }

    private void Awake()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z;
    }

    private bool Align = true;

    private void Update()
    {


        if (Align)
            if (X && !Y && !Z)
            {
                transform.position = new Vector3(transform.position.x, posY, posZ);
            }
            else if (!X && Y && !Z)
            {
                transform.position = new Vector3(posX, transform.position.y, posZ);
            }
            else if (!X && !Y && Z)
            {
                transform.position = new Vector3(posX, posY, transform.position.z);
            }
            else if (X && Y && !Z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
            }
            else if (!X && Y && Z)
            {
                transform.position = new Vector3(posX, transform.position.y, transform.position.z);
            }
            else if (X && Y && Z)
            {
                if (GlobalDebugMode)
                    Debug.LogAssertion($"[MPC]: All Axis' For Platform: {MovingPlatformID} Have been enabled!\nPlease note that this could cause path issues.");
            }


        //FORWARD DIRECTION AXIS CONTROLLER

        //X Axis
        if (forward && X && !Y && !Z && !Direction)
            transform.Translate(Vector3.right * gpms);
        else if (reverse && X && !Y && !Z && !Direction)
            transform.Translate(Vector3.left * gpms);

        //Y Axis
        if (forward && Y && !X && !Z && !Direction)
            transform.Translate(Vector3.up * gpms);
        else if (reverse && Y && !X && !Z && !Direction)
            transform.Translate(Vector3.down * gpms);

        //Z Axis
        if (forward && Z && !Y && !X && !Direction)
            transform.Translate(Vector3.forward * gpms);
        else if (reverse && Z && !Y && !X && !Direction)
            transform.Translate(Vector3.back * gpms);






        //MULTI-AXIS-CONTROLLERS

        //X + Y Axis
        if (forward && Y && X && !Direction)
            transform.Translate(new Vector3(1, 1, 0) * gpms);
        else if (reverse && Y && X && !Direction)
            transform.Translate(new Vector3(-1, -1, 0) * gpms);

        //Z + Y Axis
        if (forward && Z && Y && !Direction)
            transform.Translate(new Vector3(0, 1, 1) * gpms);
        else if (reverse && Z && Y && !Direction)
            transform.Translate(new Vector3(0, -1, -1) * gpms);




        //REVERSE DIRECTION AXIS CONTROLLER

        //X Axis
        if (forward && X && !Y && !Z && Direction)
            transform.Translate(Vector3.left * gpms);
        else if (reverse && X && !Y && !Z && Direction)
            transform.Translate(Vector3.right * gpms);

        //Y Axis
        if (forward && Y && !X && !Z && Direction)
            transform.Translate(Vector3.down * gpms);
        else if (reverse && Y && !X && !Z && Direction)
            transform.Translate(Vector3.up * gpms);

        //Z Axis
        if (forward && Z && !Y && !X && Direction)
            transform.Translate(Vector3.back * gpms);
        else if (reverse && Z && !Y && !X && Direction)
            transform.Translate(Vector3.forward * gpms);






        //MULTI-AXIS-CONTROLLERS

        //X + Y Axis
        if (forward && Y && X && Direction)
            transform.Translate(new Vector3(-1, 1, 0) * gpms);
        else if (reverse && Y && X && Direction)
            transform.Translate(new Vector3(1, -1, 0) * gpms);

        //Z + Y Axis
        if (forward && Z && Y && Direction)
            transform.Translate(new Vector3(0, 1, -1) * gpms);
        else if (reverse && Z && Y && Direction)
            transform.Translate(new Vector3(0, -1, 1) * gpms);

        if (stop)
        {
            forward = false;
            reverse = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == MovingPlatformID+"_REVERSE" && UsingTrigger)
        {
            forward = false;
            reverse = false;
            if (GlobalDebugMode)
            print($"[MPC]: {MovingPlatformID} Reverse");
            StartCoroutine(Reverse());
        }
        else if (collision.gameObject.tag == MovingPlatformID + "_FORWARDS" && UsingTrigger)
        {
            forward = false;
            reverse = false;
            if (GlobalDebugMode)
            print($"[MPC]: {MovingPlatformID} Forwards");
            StartCoroutine(Forwards());
        }
        else if (!UsingTrigger)
        {
            if (GlobalDebugMode)
            print($"[MPC]: {MovingPlatformID} Hit Trigger in Collider mode, Disregarding");
        }
        else
        {
            if (GlobalDebugMode)
            Debug.LogWarning($"[MPC]: Warning! Platform ID: {MovingPlatformID} Hit GameObject Name {collision.gameObject.name} That is NOT a Platform Director!");
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == MovingPlatformID + "_REVERSE" && !UsingTrigger)
        {
            forward = false;
            reverse = false;
            if (GlobalDebugMode)
            print($"[MPC]: {MovingPlatformID}: Hit Reverse");
            StartCoroutine(Reverse());
        }
        else if (collision.gameObject.tag == MovingPlatformID + "_FORWARDS" && !UsingTrigger)
        {
            forward = false;
            reverse = false;
            if (GlobalDebugMode)
            print($"[MPC]: {MovingPlatformID} Hit Forwards");
            StartCoroutine(Forwards());
        } else if (UsingTrigger)
        {
            if (GlobalDebugMode)
            print($"[MPC]: {MovingPlatformID} Hit Collider in Trigger mode, Disregarding");
        }
        else
        {
            if (GlobalDebugMode)
            Debug.LogWarning($"[MPC]: Warning! Platform ID: {MovingPlatformID} Hit GameObject name {collision.gameObject.name} This is NOT a Platform Director!");
        }

    }

    private IEnumerator Forwards()
    {
        //adds delay
        yield return new WaitForSeconds(Delay);
        forward = true;
        reverse = false;
        stop = false;
        if (GlobalDebugMode)
        print("[MPC]: Forwards For ID: " + MovingPlatformID+ " (waited: " +Delay+"s)");
    }
    private IEnumerator Reverse()
    {
        //adds delay
        yield return new WaitForSeconds(Delay);
        forward = false;
        reverse = true;
        stop = false;
        gameObject.transform.Translate(Vector3.back * gpms);
        if (GlobalDebugMode)
        print("[MPC]: Reverse For ID: " + MovingPlatformID + " (waited: " + Delay + "s)");
    }
}
