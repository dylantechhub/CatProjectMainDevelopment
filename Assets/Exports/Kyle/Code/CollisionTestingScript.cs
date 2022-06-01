using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTestingScript : MonoBehaviour
{
    public void OnTrigger(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Check the console to see if it works
            Debug.Log("It Collided!");

        }
    }
}
