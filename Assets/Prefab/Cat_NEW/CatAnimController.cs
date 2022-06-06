using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class CatAnimController : MonoBehaviour
{
    ThirdPersonController TPC;
    Animator Cat;

    void Start()
    {
        //get the cats gameobject, then get the TPC
        TPC = GameObject.Find("cat").GetComponent<ThirdPersonController>();

        //get the animator
        Cat = GetComponent<Animator>();
    }

    private bool sprinting;

    // Update is called once per frame
    void Update()
    {
        Main();
    }


      void Idle()
    {
        Cat.SetBool("idle", true);
        //if idle animation is used, blend to it from here
        
    }
    float t;
    void Main()
    {

        //update animator with paramters that would be used by the old animator, makes life much more easier

        //sprinting
        if (TPC._input.sprint)
        {
            Cat.speed = 3f;
            sprinting = true;
            //double the movement speed, therefore sprint
            Cat.SetFloat("motionSpeed", 0.6f);

            Cat.SetFloat("running", Mathf.Lerp(0, 1, t+=0.01f));

            Cat.SetBool("idle", false);
        }
        else
        {
            t = 0;
            Cat.speed = 1;
            sprinting = false;
            //set movment speed to 0, therefore stopping the animation
            Cat.SetFloat("motionSpeed", 0);
            Cat.SetFloat("running", Mathf.Lerp(1, 0, t += 1));


            Idle();
        }

        Cat.SetFloat("speed", TPC._speed);

        //walking
        if (TPC._input.move != Vector2.zero && !sprinting)
        {
            Cat.SetFloat("motionSpeed", 0.5f);
            Cat.SetFloat("running", Mathf.Lerp(1, 0, 1));
            Cat.SetBool("idle", false);
        }
        else if (!sprinting)
        {
            Cat.SetFloat("motionSpeed", 0);
            Cat.SetFloat("running", Mathf.Lerp(1, 0, 1));
            Idle();
        }

    }
}
