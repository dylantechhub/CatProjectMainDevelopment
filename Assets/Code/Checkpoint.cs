using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpawnPoint spawn;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Character hits");
        if (hit.gameObject.CompareTag("Player"))
        {
            // stop timer, keep hhigh scores
            spawn.StopTimer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(" THing hit");
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            // stop timer, keep hhigh scores
            spawn.StopTimer();
            Debug.Log(" Player hit");
        }
    }
}
