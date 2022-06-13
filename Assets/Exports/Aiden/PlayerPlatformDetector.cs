using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformDetector : MonoBehaviour
{
    private bool off;
    public GameObject Cat;
    //detect if the player is standing on a platform type
    //this script is global to all platforms, with no hassle. Attach to the playerarmature.
    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        //GLOBAL PLATFORMS
            if (collision.gameObject.tag == "Platform")
            {
            Cat.transform.SetParent(collision.transform);
            off = true;
        } else if (off) {
            //end player rotation
            off = false;
                print(collision.gameObject.tag);
            Cat.transform.SetParent(null);
            Cat.transform.position += new Vector3(0.0f, 1.0f, 0.0f);
            }
        }
}
