using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformDetector : MonoBehaviour
{
    private bool off;
    public GameObject Cat;

    private void Start()
    {
        Cat = transform.parent.gameObject;
    }

    //detect if the player i
    //
    //s standing on a platform type
    //this script is global to all platforms, with no hassle. Attach to the playerarmature.
    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        //GLOBAL PLATFORMS
        if (collision.gameObject.tag == "Platform")
        {
            Cat.transform.SetParent(collision.transform);
            off = true;
        }
    }

    Ray ray;

    void Update()
    {
        if (off)
        {
            ray = new Ray(transform.position, -transform.up);
            Debug.DrawRay(ray.origin, ray.direction * 5);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 5))
            {
                if (!hitData.transform.gameObject.GetComponent<MovingPlatform>())
                {
                    //end
                    off = false;
                    Cat.transform.SetParent(null);
                }
            }
        }
    }
}
