using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformsData;

namespace Platforms
{
    //DEV METHODS

    /// <summary>
    /// Retreives Lengths of all platforms
    /// </summary>
    public class RetreivePlatformsLength
    {

        private static PlatformController PC = PlatformControllerScript.Script;

        /// <summary>
        /// Retreives all the platforms in array format [MP, RP, ALL]
        /// </summary>
        public static int[] All()
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: RetreivePlatformsLength.All() Call Received!");
            if (PC.rotatingPlatforms == null)
            {
                return PlatformRemoteLength.GetMethodMPA(PC);
            }
            else if (PC.movingPlatforms == null)
            {
                return PlatformRemoteLength.GetMethodRPA(PC);
            }
            else
            {
                return PlatformRemoteLength.GetMethodAll(PC);
            }
        }

        /// <summary>
        /// Retreives all the Moving Platforms
        /// </summary>
        public static int MP()
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: RetreivePlatformsLength.MP() Call Received!");
            return PlatformRemoteLength.GetMethodMP(PC);
        }

        /// <summary>
        /// Retreives all the Rotating Platforms
        /// </summary>
        public static int RP()
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: RetreivePlatformsLength.RP() Call Received!");
            return PlatformRemoteLength.GetMethodRP(PC);
        }
    }

    /// <summary>
    /// Get platforms + tools
    /// </summary>
    public class GetPlatform {
        private static PlatformController PC = PlatformControllerScript.Script;

    /// <summary>
    /// Get a Moving Platforms gameObject via its ID
    /// </summary>
        public static GameObject MovingPlatformGameObject(string ID) {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: GetPlatform.GetMovingPlatformGameObject({ID}) Call Received!");
            foreach(var mp in PC.movingPlatforms) {
                if (mp.ID == ID) {
                    if (PC.GlobalDebugMode)
                    Debug.Log($"[PLATFORMS@CONTROLLER]: Call Complete!");    
                return mp.MovingPlatform.gameObject;
                }
            }
            return new GameObject();
        }

    /// <summary>
    /// Get a Rotating Platforms gameObject via its ID
    /// </summary>
        public static GameObject RotatingPlatformGameObject(string ID) {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: GetPlatform.GetRotatingPlatformGameObject({ID}) Call Received!");
            foreach(var rp in PC.rotatingPlatforms) {
                if (rp.ID == ID){
                    if (PC.GlobalDebugMode)
                    Debug.Log($"[PLATFORMS@CONTROLLER]: Call Complete!");
                return rp.rotationPlatform.gameObject;
                }
            }
            return new GameObject();
        }

    /// <summary>
    /// Check if a platform with a specified ID exists, method=1 RP, method=0 MP.
    /// </summary>
    public static bool Exists(string ID, int method) {
        if (method == 0) {
            foreach(var mp in PC.movingPlatforms) {
                if (mp.ID == ID) 
                return true;
                else
                return false;
            }
            return false;
        } else if (method == 1) {
            foreach(var rp in PC.rotatingPlatforms) {
                if (rp.ID == ID) 
                return true;
                else
                return false;
            } 
            return false;
        } else 
            return false;
    }

    }

    /// <summary>
    /// Retreive or set rotating platform speeds
    /// </summary>
    public class RotationSpeed
    {

        private static PlatformController PC = PlatformControllerScript.Script;

        /// <summary>
        /// Sets the global rotation speed for all rotating platforms
        /// </summary>
        public static void SetGlobal(float speed)
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: RotationSpeed.SetGlobal({speed}) Call Received!");
            SetRotationSpeedRemote.SetGlobal(PC, speed);
        }

        /// <summary>
        /// Sets the local rotation speed of a selected RP via ID
        /// </summary>
        public static void SetLocal(float speed, string ID)
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: RotationSpeed.SetLocal({speed}, {ID}) Call Received!");
            SetRotationSpeedRemote.SetLocal(PC, speed, ID);
        }

        /// <summary>
        /// Gets the local RP speed via ID
        /// </summary>
        public static float GetLocal(string ID)
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: RotationSpeed.GetLocal({ID}) Call Received!");
            return SetRotationSpeedRemote.GetLocal(PC, ID);
        }
    }

    /// <summary>
    /// Retreive or set moving platform speeds
    /// </summary>
    public class MovementSpeed
    {

        private static PlatformController PC = PlatformControllerScript.Script;

        /// <summary>
        /// Sets the global movement speed for all moving platforms
        /// </summary>
        public static void SetGlobal(float speed)
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: SetMovementSpeedRemote.SetGlobal({speed}) Call Received!");
            SetMovementSpeedRemote.SetGlobal(PC, speed);
        }

        /// <summary>
        /// Sets the local movement speed of a selected MP via ID
        /// </summary>
        public static void SetLocal(float speed, string ID)
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: SetMovementSpeedRemote.SetLocal({speed}, {ID}) Call Received!");
            SetMovementSpeedRemote.SetLocal(PC, speed, ID);
        }

        /// <summary>
        /// Gets the local MP speed via ID
        /// </summary>
        public static float GetLocal(string ID)
        {
            if (PC.GlobalDebugMode)
            Debug.Log($"[PLATFORMS@CONTROLLER]: SetMovementSpeedRemote.GetLocal({ID}) Call Received!");
            return SetMovementSpeedRemote.GetLocal(PC, ID);
        }
    }




}



//MAIN
public class PlatformController : MonoBehaviour
{





    public bool Update = false;
    [Space(20)]
    [Header("Debuggers")]
    [Tooltip("Use this if your having local problems. true by default.")]
    public bool LocalDebugMode = true;
    [Space(10)]
    [Tooltip("Use this if your reaching this script from another script and need to debug issues. very helpful!")]
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

    //private list that stores all the scripts on the Moving Platform.
    private static List<MovingPlatform> MPSCRIPTS = new List<MovingPlatform>();



    //create options class, organization 100
    [System.Serializable]
    public class MovingPlatformOptions
    {
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
            if (p.MovingPlatform != null) {
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
            } else {
                if (LocalDebugMode)
                Debug.LogError("[PCS]: ERROR! Invalid MP object(s)!");
            }
        }
    }

    void FixedUpdate()
    {
        //ROTATING PLATFORMS
        //start rotating the objects
        foreach (var p in rotatingPlatforms)
        {
            var platform = p.rotationPlatform;
            if (p.rotationPlatform != null) {

            
            var rotateSwitch = p.RotateSwitch;
            if (rotateSwitch)
            {
                platform.transform.Rotate(Vector3.up, 50 * p.LocalRotationSpeed);
            }
            else
            {
                platform.transform.Rotate(Vector3.up, -50 * p.LocalRotationSpeed);
            }
            } else {
                if (LocalDebugMode)
                Debug.LogError("[PCS]: ERROR! Invalid RP object(s)!");
            }
        }


        //update all platforms
        if (Update)
        {
            UpdatePlatformControllers();
            if (LocalDebugMode)
            Debug.LogWarning("[PCS]: Reset All Platforms!");
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
            if (p.MovingPlatform != null) {
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
            } else {
                if (LocalDebugMode)
                Debug.LogError("[PCS]: ERROR! Invalid platform object(s)!");
            }
        }
    }
}