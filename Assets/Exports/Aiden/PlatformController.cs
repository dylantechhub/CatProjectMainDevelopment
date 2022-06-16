using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformsData;

namespace Platforms
{
    using System.Collections;
    using UnityEngine;
    //DEV METHODS

    /// <summary>
    /// Retreives Lengths of all platforms
    /// </summary>
    public class RetreivePlatformsLength
    {

        private static MainPlatformController.PlatformController PC = PlatformControllerScript.Script;


        /// <summary>
        /// Retreives all the platforms in array format [MP, RP, ALL]
        /// </summary>
        public static int[] All()
        {
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@RetreivePlatformsLength.Dynamic()] Call Received!");
            if (PC.rotatingPlatforms == null)
            {
                if (PC.GlobalDebugMode)
                    Debug.Log($"[PLATFORMS@RetreivePlatformsLength.MP()] Returning: "+ PlatformRemoteLength.GetMethodMPA(PC));
                return PlatformRemoteLength.GetMethodMPA(PC);
            }
            else if (PC.movingPlatforms == null)
            {
                if (PC.GlobalDebugMode)
                    Debug.Log($"[PLATFORMS@RetreivePlatformsLength.RP()] Returning: " + PlatformRemoteLength.GetMethodRPA(PC));
                return PlatformRemoteLength.GetMethodRPA(PC);
            }
            else
            {
                if (PC.GlobalDebugMode)
                    Debug.Log($"[PLATFORMS@RetreivePlatformsLength.All()] Returning: " + PlatformRemoteLength.GetMethodAll(PC));
                return PlatformRemoteLength.GetMethodAll(PC);
            }
        }

        /// <summary>
        /// Retreives all the Moving Platforms
        /// </summary>
        public static int MP()
        {
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@RetreivePlatformsLength.MP()] Call Received!");
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@RetreivePlatformsLength.MP()] Returning: "+ PlatformRemoteLength.GetMethodMP(PC));
            return PlatformRemoteLength.GetMethodMP(PC);
        }

        /// <summary>
        /// Retreives all the Rotating Platforms
        /// </summary>
        public static int RP()
        {
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@RetreivePlatformsLength.RP()] Call Received!");
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@RetreivePlatformsLength.RP()] Returning: "+ PlatformRemoteLength.GetMethodRP(PC));
            return PlatformRemoteLength.GetMethodRP(PC);
        }
    }

    /// <summary>
    /// Get platforms + tools
    /// </summary>
    public class GetPlatform {
        private static MainPlatformController.PlatformController PC = PlatformControllerScript.Script;

        /// <summary>
        /// Get a Moving Platform via its ID
        /// </summary>
        public static GameObject MovingPlatform(string ID) {
            foreach (var mp in PC.movingPlatforms) {
                if (mp.ID == ID) {
                    return mp.MovingPlatform.gameObject;
                }
            }
            return new GameObject();
        }

        /// <summary>
        /// Get a Rotating Platform via its ID
        /// </summary>
        public static GameObject RotatingPlatform(string ID) {
            foreach (var rp in MainPlatformController.PlatformController.RPSCRIPTS) {
                MonoBehaviour.print(rp.name);
                if (rp.ID() == ID) {
                    return rp.gameObject;
                }
            }
            return new GameObject();
        }

        /// <summary>
        /// Check if a Moving Platform with a specified ID exists
        /// </summary>
        public static bool MPExists(string ID) {
            foreach (var mp in PC.movingPlatforms) {
                if (mp.ID == ID)
                    return true;
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Check if a Rotating Platform with a specified ID exists
        /// </summary>
        public static bool RPExists(string ID)
        {
            foreach (var rp in PC.rotatingPlatforms)
            {
                if (rp.ID == ID)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }

    /// <summary>
    /// Retreive or set rotating platform speeds
    /// </summary>
    public class RotationSpeed
    {

        private static MainPlatformController.PlatformController PC = PlatformControllerScript.Script;

        /// <summary>
        /// Sets the global rotation speed for all rotating platforms
        /// </summary>
        public static void SetGlobal(float speed)
        {
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@RotationSpeed.SetGlobal({speed})] Call Received!");
            SetRotationSpeedRemote.SetGlobal(PC, speed);
        }

        /// <summary>
        /// Sets the local rotation speed of a selected RP via ID
        /// </summary>
        public static void SetLocal(float speed, string ID)
        {
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@RotationSpeed.SetLocal({speed}, {ID})] Call Received!");
            SetRotationSpeedRemote.SetLocal(PC, speed, ID);
        }

        /// <summary>
        /// Gets the local RP speed via ID
        /// </summary>
        public static float GetLocal(string ID)
        {
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@RotationSpeed.GetLocal({ID})] Call Received!");
            return SetRotationSpeedRemote.GetLocal(PC, ID);
        }
    }


    /// <summary>
    /// Edit a platforms settings, and functions !![ADVANCED]!! Allows full control over the platforms.
    /// </summary>
    public class EditPlatforms : MonoBehaviour
    {

        private static MainPlatformController.PlatformController PC = PlatformControllerScript.Script;

        /// <summary>
        /// Edit a Moving Platform
        /// </summary>
        public static MovingPlatform Moving(GameObject PlatformToEdit)
        {
            MovingPlatform pre = PlatformToEdit.transform.gameObject.GetComponent<MovingPlatform>();
            UpdateInspector();
            return pre;
        }

        /// <summary>
        /// Edit a Rotating Platform
        /// </summary>
        public static RotatingPlatform Rotating(GameObject PlatformToEdit)
        {
            var pre = PlatformToEdit.GetComponent<RotatingPlatform>();
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@EditPlatforms.Rotating({PlatformToEdit.name})] Call Received!");
            if (PC.GlobalDebugMode)
                UpdateInspector();
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@EditPlatforms.Rotating({PlatformToEdit.name})] Returning: " + pre);
            return pre;
        }


        /// <summary>
        /// Update the inspector
        /// </summary>
        public static void UpdateInspector()
        {
           MainPlatformController.PlatformController PC = PlatformControllerScript.Script;
            if (PC.movingPlatforms != null)
           foreach (var p in PC.movingPlatforms)
            {
                var pOBJ = p.MovingPlatform;
                var mps = pOBJ.transform.gameObject.GetComponent<MovingPlatform>();
                //start updating the inspector with the remote settings
                p.DelayAmmount = mps.Delay;
                p.DirectionSwitch = mps.Direction;
                p.LocalPlatformMovingSpeed = mps.speed;
                p.usingTrigger = mps.UsingTrigger;
                p.X = mps.X;
                p.Y = mps.Y;
                p.Z = mps.Z;
                string x=""; string y=""; string z="";
                if (mps.METHODS.Contains("X")) { x = "X"; }; if (mps.METHODS.Contains("Y")) { y = "Y"; }; if (mps.METHODS.Contains("Z")) { z = "Z"; }
                p.mpAXIS_X = x;
                p.mpAXIS_Y = y;
                p.mpAXIS_Z = z;
                if (PC.GlobalDebugMode)
                print("[PCS@UpdateInspector] Remote Moving Platform Changes Saved");
            }
            else if (PC.GlobalDebugMode)
            {
                Debug.LogError("[PCS@UpdateInspector] No MP's Updated, as none are in the list. NULL");
            }
            //RP
            if (PC.rotatingPlatforms != null)
                foreach (var p in PC.rotatingPlatforms)
                {
                    var pOBJ = p.rotationPlatform;
                    var rps = pOBJ.GetComponent<RotatingPlatform>();
                    //start updating the inspector with the remote settings
                    p.LocalRotationSpeed = rps.LocalRotationSpeed;
                    bool cw;
                    if (rps.RotatingDirection() == "clockwise") cw = true; else cw = false;
                    p.RotateSwitch = cw;
                    p.rotationPlatform = rps.gameObject;

                    if (PC.GlobalDebugMode)
                        print("[PCS@UpdateInspector] Remote Rotating Platform Changes Saved");
                }
            else if (PC.GlobalDebugMode)
            {
                Debug.LogError("[PCS@UpdateInspector] No RP's Updated, as none are in the list. NULL");
            }
        }
    }

    /// <summary>
    /// Retreive or set moving platform speeds
    /// </summary>
    public class MovementSpeed
    {
        private static MainPlatformController.PlatformController PC = PlatformControllerScript.Script;

        /// <summary>
        /// Sets the global movement speed for all moving platforms
        /// </summary>
        public static void SetGlobal(float speed)
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@SetMovementSpeedRemote.SetGlobal({speed})] Call Received!");
            SetMovementSpeedRemote.SetGlobal(PC, speed);
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@SetMovementSpeedRemote.SetGlobal({speed})] Call Completed (void)");
        }

        /// <summary>
        /// Sets the local movement speed of a selected MP via ID
        /// </summary>
        public static void SetLocal(float speed, string ID)
        {
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@SetMovementSpeedRemote.SetLocal({speed}, {ID})] Call Received!");
            SetMovementSpeedRemote.SetLocal(PC, speed, ID);
            if (PC.GlobalDebugMode)
                Debug.Log($"[PLATFORMS@SetMovementSpeedRemote.SetLocal({speed}, {ID})] Call Completed (void)");
        }

        /// <summary>
        /// Gets the local MP speed via ID
        /// </summary>
        public static float GetLocal(string ID)
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@SetMovementSpeedRemote.GetLocal({ID})] Call Received!");
            return SetMovementSpeedRemote.GetLocal(PC, ID);
        }
    }




}


namespace MainPlatformController
{
    //MAIN
    public class PlatformController : MonoBehaviour
    {






        public bool Update = false;
        [Space(20)]
        [Header("Debuggers")]
        [Tooltip("Use this if your having local problems. true by default.")]
        public bool LocalDebugMode = true;
        [Space(10)]
        [Tooltip("Advanced Debugger, Logs all remote changes, and platform events.")]
        public bool GlobalDebugMode = false;
        [Space(30)]
        //stores all the rotating platforms
        [SerializeField]
        public List<RotatingPlatformsOptions> rotatingPlatforms;


        //create options class, [organization 100]
        [System.Serializable]
        public class RotatingPlatformsOptions
        {
            //the platforms
            public GameObject rotationPlatform;
            //which way to rotate said platform
            public bool RotateSwitch;
            [Range(0.0001f, 0.5f)]
            public float LocalRotationSpeed;

            [Tooltip("Mainly used to contact this RP from a script, set if needed")]
            public string ID;

        }


        [Space(8)]

        //stores all the moving platforms
        public List<MovingPlatformOptions> movingPlatforms;

        //private list that stores all the scripts on the Moving Platform scripts.
        [HideInInspector]
        public static List<MovingPlatform> MPSCRIPTS = new List<MovingPlatform>();
        //private list that stores all the scripts on the Rotating Platform scripts.
        [HideInInspector]
        public static List<RotatingPlatform> RPSCRIPTS = new List<RotatingPlatform>();

        //create options class, organization 100
        [System.Serializable]
        public class MovingPlatformOptions
        {
            [HideInInspector]
            public string mpAXIS_X;
            [HideInInspector]
            public string mpAXIS_Y;
            [HideInInspector]
            public string mpAXIS_Z;

            public GameObject MovingPlatform;
            //Delay for when the platform reaches its end, and switches directions
            [Header("Delay in seconds to wait, before switching directions")]
            public float DelayAmmount;
            [Space(5)]
            [Range(0.001f, 0.3f)]
            public float LocalPlatformMovingSpeed;

            [Space(5)]
            [Header("If the Director object is a trigger, click this. if not it defaults to collider.")]
            public bool usingTrigger;
            [Space(3)]
            [Header("Move on X Axis?")]
            public bool X;
            [Header("Move on Y Axis?")]
            public bool Y;
            [Header("Move on Z Axis?")]
            public bool Z = true;
            [Space(5)]
            [Header("Switch Directions?")]
            public bool DirectionSwitch;
            [Space(5)]
            public string ID;
        }



        private void Awake()
        {

            PlatformControllerScript.Script = this;


            //MOVING PLATFORMS
            foreach (var p in movingPlatforms)
            {
                var gOBJ = p.MovingPlatform;
                MovingPlatform MPSCRIPT;
                if (p.MovingPlatform != null)
                {
                    MPSCRIPT = gOBJ.GetComponent<MovingPlatform>();
                    MPSCRIPTS.Add(MPSCRIPT);
                    MPSCRIPT.SetMPID(p.ID);

                    //add the moving platforms scripts to the global controllers MPSCRIPTS list, so if we need to edit all the moving platforms scripts at once, or seperatly, we can.



                    //Change Axis Option for MOVING PLATFORMS
                    var x = p.X;
                    var y = p.Y;
                    var z = p.Z;

                     p.mpAXIS_Y = "";
                     p.mpAXIS_Z = "";
                     p.mpAXIS_X = "";

                    if (x)
                    {
                        //set axis
                        p.mpAXIS_X = "X";
                    }
                    if (y)
                    {
                        //set axis
                        p.mpAXIS_Y = "Y";
                    }
                    if (z)
                    {
                        //set axis
                        p.mpAXIS_Z = "Z";
                    }
                    if (p.mpAXIS_X == "" && p.mpAXIS_Y == "" && p.mpAXIS_Z == "")
                    {
                        if (LocalDebugMode)
                            Debug.LogError("[PCS]: Please Choose An Axis For Moving Platform Name: " + p.MovingPlatform.name);
                    }
                    //tell the platform to start
                    MPSCRIPT.EnablePlatform(p.DelayAmmount, p.LocalPlatformMovingSpeed, p.mpAXIS_X, p.mpAXIS_Y, p.mpAXIS_Z, p.DirectionSwitch, p.usingTrigger, GlobalDebugMode);
                }
                else
                {
                    if (LocalDebugMode)
                        Debug.LogError("[PCS]: ERROR! Invalid MP object(s)!");
                }
            }

            //RP 

            foreach (var p in rotatingPlatforms)
            {
                var gOBJ = p.rotationPlatform;
                RotatingPlatform RPSCRIPT;
                if (p.rotationPlatform != null)
                {
                    RPSCRIPT = gOBJ.GetComponent<RotatingPlatform>();
                    RPSCRIPTS.Add(RPSCRIPT);
                    RPSCRIPT.SetRPID(p.ID, this);

                    //add the rotating platforms scripts to the global controllers RPSCRIPTS list, so if we need to edit all the rotating platforms scripts at once, or seperatly, we can.

                    RPSCRIPT.EnablePlatform(p.RotateSwitch, p.LocalRotationSpeed, GlobalDebugMode);
                }
                else
                {
                    if (LocalDebugMode)
                        Debug.LogError("[PCS]: ERROR! Invalid RP object(s)!");
                }
            }
        }

        void FixedUpdate()
        {
            //update all platforms
            if (Update)
            {
                UpdatePlatformControllers();
                if (LocalDebugMode)
                    Debug.Log("[PCS]: Updated Platforms!");
                Update = false;
            }
        }

        public void UpdatePlatformControllers()
        {
            //MOVING PLATFORMS
            foreach (var p in movingPlatforms)
            {
                var gOBJ = p.MovingPlatform;
                MovingPlatform MPSCRIPT;
                if (p.MovingPlatform != null)
                {
                    MPSCRIPT = gOBJ.GetComponent<MovingPlatform>();
                    MPSCRIPTS.Add(MPSCRIPT);
                    MPSCRIPT.SetMPID(p.ID);

                    //add the moving platforms scripts to the global controllers MPSCRIPTS list, so if we need to edit all the moving platforms scripts at once, we can.



                    //Change Axis Option for MOVING PLATFORMS
                    var x = p.X;
                    var y = p.Y;
                    var z = p.Z;

                    var mpAXIS_Y = "";
                    var mpAXIS_Z = "";
                    var mpAXIS_X = "";

                    if (x)
                    {
                        //set axis
                        mpAXIS_X = "X";
                    }
                    if (y)
                    {
                        //set axis
                        mpAXIS_Y = "Y";
                    }
                    if (z)
                    {
                        //set axis
                        mpAXIS_Z = "Z";
                    }
                    if (mpAXIS_X == "" && mpAXIS_Y == "" && mpAXIS_Z == "")
                    {
                        if (LocalDebugMode)
                            Debug.LogError("[PCS]: Please Choose An Axis For Moving Platform Name: " + p.MovingPlatform.name);
                    }
                    //tell the platform to start
                    MPSCRIPT.EnablePlatform(p.DelayAmmount, p.LocalPlatformMovingSpeed, mpAXIS_X, mpAXIS_Y, mpAXIS_Z, p.DirectionSwitch, p.usingTrigger, GlobalDebugMode);
                }
                else
                {
                    if (LocalDebugMode)
                        Debug.LogError("[PCS]: ERROR! Invalid platform object(s)!");
                }
            }

            //RP 

            foreach (var p in rotatingPlatforms)
            {
                var gOBJ = p.rotationPlatform;
                RotatingPlatform RPSCRIPT;
                if (p.rotationPlatform != null)
                {
                    RPSCRIPT = gOBJ.GetComponent<RotatingPlatform>();
                    RPSCRIPTS.Add(RPSCRIPT);
                    RPSCRIPT.SetRPID(p.ID, this);

                    //add the rotating platforms scripts to the global controllers RPSCRIPTS list, so if we need to edit all the rotating platforms scripts at once, or seperatly, we can.

                    RPSCRIPT.EnablePlatform(p.RotateSwitch, p.LocalRotationSpeed, GlobalDebugMode);
                }
                else
                {
                    if (LocalDebugMode)
                        Debug.LogError("[PCS]: ERROR! Invalid RP object(s)!");
                }
            }

        }
    }
}