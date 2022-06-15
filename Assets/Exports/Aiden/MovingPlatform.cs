using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class MovingPlatform : MonoBehaviour
    {
    
    /// <summary>
    /// Delay for when the platform switches its directions (seconds)
    /// </summary>
    [HideInInspector]
    public float Delay { get; set; }


    /// <summary>
    /// Local speed
    /// </summary>
    [HideInInspector]
    public float speed { set; get; }




    /// <summary>
    /// Enable/get platform movement on the X axis
    /// </summary>
    [HideInInspector]
    public bool X { get; set; }

    /// <summary>
    /// Enable/get platform movement on the Y axis
    /// </summary>
    [HideInInspector]
    public bool Y { get; set; }

    /// <summary>
    /// Enable/get platform movement on the Z axis
    /// </summary>
    [HideInInspector]
    public bool Z { get; set; }



    private bool stop;

    /// <summary>
    /// Stops the platform in its path
    /// </summary>
    [HideInInspector]
    public void Pause()
    {
        stop = true;
    }
    /// <summary>
    /// Resumes the platforms movement path if stopped
    /// </summary>
    [HideInInspector]
    public void Resume()
    {
        stop = false;
    }
    /// <summary>
    /// true if the platform is paused
    /// </summary>
    [HideInInspector]
    public bool isPaused()
    {
        return stop;
    }

    /// <summary>
    /// Retrieves the platforms ID
    /// </summary>
    public string ID()
    {
        return MovingPlatformID;
    }

    public float posX;
    public float posY;
    public float posZ;

    /// <summary>
    /// Moves the platform in the forwards direction
    /// </summary>
    [HideInInspector]
    public void GoForwards()
    {
        reverse = false;
        forward = true;
    }
    /// <summary>
    /// Moves the platform in reverse
    /// </summary>
    [HideInInspector]
    public void GoReverse()
    {
        reverse = true;
        forward = false;
    }
    /// <summary>
    /// Returns a "forwards" or "reverse" string, depending on what direction the platforms moving in
    /// </summary>
    [HideInInspector]
    public string MovingWhere()
    {
        var moving = "";
        if (forward) moving = "forwards"; else if (reverse) moving = "reverse";
        return moving;
    }

    private bool forward;
    private bool reverse;


    /// <summary>
    /// Change the Collision Detecting mode for this platform
    /// </summary>
    [HideInInspector]
    public bool UsingTrigger { get; set; }

        private bool GlobalDebugMode;

        [Space(10)]
        public string MovingPlatformID;

    public void SetMPID(string ID)
        {
            if (ID != "")
                MovingPlatformID = ID;
            else
                Debug.LogError($"[MPC] ERROR! Moving platform named {this.gameObject.name} does not have a set ID!");
        }

    /// <summary>
    /// Change/get the Axis Methods
    /// </summary>
    [HideInInspector]
    public List<string> METHODS { get; set; } = new List<string>();


    /// <summary>
    /// Chnage the direction of the platform
    /// </summary>
    [HideInInspector]
    public bool Direction { get; set; }


    //the PlatformController calls this function to enable the platform.
    /// <summary>
    /// manually enable a platform from another script. read docs for more information.
    /// </summary>
    [HideInInspector]
    public void EnablePlatform(float delay, float speedFPC, string METHOD_X, string METHOD_Y, string METHOD_Z, bool DIRECTION, bool usingTrigger, bool globalDebugMode)
        {
            GlobalDebugMode = globalDebugMode;
            UsingTrigger = usingTrigger;
            Direction = DIRECTION;
            METHODS.Add(METHOD_X);
            METHODS.Add(METHOD_Y);
            METHODS.Add(METHOD_Z);

            if (GlobalDebugMode)
            {
                print("[MPC]: Global-Debug mode enabled!");
                print($"[MPC]: Platform {MovingPlatformID} Enabled/Updated! Using Methods: " + METHOD_Z + METHOD_Y + METHOD_X + $" TriggerMode?: " + usingTrigger);
            }

            if (METHODS.Contains("X"))
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
            Delay = delay;
            speed = speedFPC;

        //start! \/
            forward = true;
        }

        private void Start()
        {
            posX = transform.position.x;
            posY = transform.position.y;
            posZ = transform.position.z;
        print(posX+" "+posY+" "+posZ);
        }
    private bool Align = true;
        private void FixedUpdate()
        {
            //Alingners, keep the platform on path.
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
                transform.Translate(Vector3.right * speed);
            else if (reverse && X && !Y && !Z && !Direction)
                transform.Translate(Vector3.left * speed);

            //Y Axis
            if (forward && Y && !X && !Z && !Direction)
                transform.Translate(Vector3.up * speed);
            else if (reverse && Y && !X && !Z && !Direction)
                transform.Translate(Vector3.down * speed);

            //Z Axis
            if (forward && Z && !Y && !X && !Direction)
                transform.Translate(Vector3.forward * speed);
            else if (reverse && Z && !Y && !X && !Direction)
                transform.Translate(Vector3.back * speed);






            //MULTI-AXIS-CONTROLLERS

            //X + Y Axis
            if (forward && Y && X && !Direction)
                transform.Translate(new Vector3(1, 1, 0) * speed);
            else if (reverse && Y && X && !Direction)
                transform.Translate(new Vector3(-1, -1, 0) * speed);

            //Z + Y Axis
            if (forward && Z && Y && !Direction)
                transform.Translate(new Vector3(0, 1, 1) * speed);
            else if (reverse && Z && Y && !Direction)
                transform.Translate(new Vector3(0, -1, -1) * speed);




            //REVERSE DIRECTION AXIS CONTROLLER

            //X Axis
            if (forward && X && !Y && !Z && Direction)
                transform.Translate(Vector3.left * speed);
            else if (reverse && X && !Y && !Z && Direction)
                transform.Translate(Vector3.right * speed);

            //Y Axis
            if (forward && Y && !X && !Z && Direction)
                transform.Translate(Vector3.down * speed);
            else if (reverse && Y && !X && !Z && Direction)
                transform.Translate(Vector3.up * speed);

            //Z Axis
            if (forward && Z && !Y && !X && Direction)
                transform.Translate(Vector3.back * speed);
            else if (reverse && Z && !Y && !X && Direction)
                transform.Translate(Vector3.forward * speed);






            //MULTI-AXIS-CONTROLLERS

            //X + Y Axis
            if (forward && Y && X && Direction)
                transform.Translate(new Vector3(-1, 1, 0) * speed);
            else if (reverse && Y && X && Direction)
                transform.Translate(new Vector3(1, -1, 0) * speed);

            //Z + Y Axis
            if (forward && Z && Y && Direction)
                transform.Translate(new Vector3(0, 1, -1) * speed);
            else if (reverse && Z && Y && Direction)
                transform.Translate(new Vector3(0, -1, 1) * speed);

            if (stop)
            {
                forward = false;
                reverse = false;
            }
        }

        void OnTriggerEnter(Collider collision)
        { 
        StartCoroutine(Reverse());
            if (collision.gameObject.tag == MovingPlatformID + "_REVERSE" && UsingTrigger)
            {
                stop = true;
                if (GlobalDebugMode)
                    print($"[MPC]: {MovingPlatformID} Reverse");
                StartCoroutine(Reverse());
            }
            else if (collision.gameObject.tag == MovingPlatformID + "_FORWARDS" && UsingTrigger)
            {
                stop = true;
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
                stop = true;
                if (GlobalDebugMode)
                    print($"[MPC]: {MovingPlatformID}: Hit Reverse");
                StartCoroutine(Reverse());
            }
            else if (collision.gameObject.tag == MovingPlatformID + "_FORWARDS" && !UsingTrigger)
            {
                stop = true;
                if (GlobalDebugMode)
                    print($"[MPC]: {MovingPlatformID} Hit Forwards");
                StartCoroutine(Forwards());
            }
            else if (UsingTrigger)
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
                print("[MPC]: Forwards For ID: " + MovingPlatformID + " (waited: " + Delay + "s)");
        }


    private IEnumerator Reverse()
        {
            //adds delay
            yield return new WaitForSeconds(Delay);
            forward = false;
            reverse = true;
            stop = false;
            gameObject.transform.Translate(Vector3.back * speed);
            if (GlobalDebugMode)
                print("[MPC]: Reverse For ID: " + MovingPlatformID + " (waited: " + Delay + "s)");
        }
    }
