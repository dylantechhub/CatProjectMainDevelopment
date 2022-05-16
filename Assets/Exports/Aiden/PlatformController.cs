using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Click this box to apply a live update to the platforms, if the game is running in unity.")]
    public bool Update = false;
    //stores all the rotating platforms
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

    }


    [Space(8)]


    [Header("Moving Platforms Settings")]
    //stores all the moving platforms
    public List<MovingPlatformOptions> movingPlatforms;

    //private list that stores all the scripts on the Moving Platform.
    private List<MovingPlatform> MPSCRIPTS = new List<MovingPlatform>();



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
        [Header("NOTE: You can combine the Y Axis with the Z or X Axis by clicking both!")]
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
    }

    private void Awake()
    {
        //MOVING PLATFORMS
        foreach (var p in movingPlatforms)
        {
            var gOBJ = p.MovingPlatform;
            var MPSCRIPT = gOBJ.GetComponent<MovingPlatform>();
            //add the moving platforms scripts to the global controllers MPSCRIPTS list, so if we need to edit all the moving platforms scripts at once, we can.
            MPSCRIPTS.Add(MPSCRIPT);

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
                Debug.LogError("[MPCS]: Please Choose An Axis For Moving Platform Name: " + p.MovingPlatform.name);
            }
            //tell the platform to start
            MPSCRIPT.EnablePlatform(p.DelayAmmount, p.LocalPlatformMovingSpeed, mpAXIS_X, mpAXIS_Y, mpAXIS_Z, p.DirectionSwitch, p.usingTrigger);
        }
    }

    void FixedUpdate()
    {
        //ROTATING PLATFORMS
        //start rotating the objects
        foreach (var p in rotatingPlatforms)
        {
            var platform = p.rotationPlatform;
            var rotateSwitch = p.RotateSwitch;
            if (rotateSwitch)
            {
                platform.transform.Rotate(transform.up, 50 * p.LocalRotationSpeed);
            }
            else
            {
                platform.transform.Rotate(transform.up, -50 * p.LocalRotationSpeed);
            }
        }


        //update all platforms
        if (Update)
        {
            UpdatePlatformControllers();
            Debug.LogWarning("[MPCS]: Reset All Platforms!");
            Update = false;
        }
    }

    public void UpdatePlatformControllers()
    {
        //MOVING PLATFORMS
        foreach (var p in movingPlatforms)
        {
            var gOBJ = p.MovingPlatform;
            var MPSCRIPT = gOBJ.GetComponent<MovingPlatform>();
            //add the moving platforms scripts to the global controllers MPSCRIPTS list, so if we need to edit all the moving platforms scripts at once, we can.
            MPSCRIPTS.Add(MPSCRIPT);

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
            if (mpAXIS_X == null && mpAXIS_Y == null && mpAXIS_Z == null)
            {
                Debug.LogError("[MPCS]: Please Choose An Axis For Moving Platform Name: " + p.MovingPlatform.name);
            }
            //tell the platform to start
            MPSCRIPT.EnablePlatform(p.DelayAmmount, p.LocalPlatformMovingSpeed, mpAXIS_X, mpAXIS_Y, mpAXIS_Z, p.DirectionSwitch, p.usingTrigger);
        }
    }
}