using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple script that moves and rotates the object its placed on
public class SimpleMover : MonoBehaviour {

    [Header("Enable or disable movement and rotation")]
    public bool moving = true;
    public bool rotating = true;

    [Header("Movement in Units per Second")]
    public Vector3 mover = Vector3.zero;

    [Header("Rotation in degrees per second")]
    public Vector3 rotator = Vector3.zero;
	
	// Update is called once per frame
	void Update () {
        // Move at units per second
        if( moving){
            transform.Translate(mover * Time.deltaTime);
        }

        // rotate the transform by rotator degrees per second
        // The Time.deltaTime converts from rotator degrees per frame to degrees per second
        // Technically it is the length of time it took the last frame to render
        if( rotating ){
            transform.Rotate(rotator * Time.deltaTime);
        }
    }

    // To use with events
    public void SetRotation(bool isRotating){
        rotating = isRotating;
    }
    // set from an event
    public void SetMovement(bool isMoving){
        moving = isMoving;
    }
}
