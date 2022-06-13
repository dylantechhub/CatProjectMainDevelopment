using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeOnCollision : MonoBehaviour
{
   public Rigidbody m_Rigidbody;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            m_Rigidbody.freezeRotation = false;
            Debug.Log("Trigger");
        }

       if (collision.gameObject.tag == "Untagged")
        {
            m_Rigidbody.freezeRotation = true;
            Debug.Log("Trigger");
        }
    }
}
