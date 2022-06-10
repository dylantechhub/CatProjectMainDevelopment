using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeOnCollision : MonoBehaviour
{
   public Rigidbody m_Rigidbody;
    bool isTriggerd = false;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && !isTriggerd)
        {
            m_Rigidbody.freezeRotation = false;
            isTriggerd = true;
            Debug.Log("Trigger");
        }
    }
}
