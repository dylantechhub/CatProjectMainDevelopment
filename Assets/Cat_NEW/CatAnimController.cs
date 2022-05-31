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
        //update animator with paramters that would be used by the old animator, makes life much more easier

        //sprinting
        if (TPC._input.sprint)
        {
            sprinting = true;
            //double the movement speed, therefore sprint
        Cat.SetFloat("motionSpeed", 0.6f);
            Cat.SetBool("running", true);
            Cat.SetBool("walking", false);
        }
        else
        {
            sprinting = false;
            Cat.SetBool("running", false);
            Cat.SetBool("walking", false);
            Cat.SetFloat("motionSpeed", 0);
            Idle();
        }


        //possible useful vars for later. default value for now.
        Cat.speed = TPC._speed;
        

        //walking
        if (TPC._input.move != Vector2.zero  && !sprinting)
        {
            Cat.SetFloat("motionSpeed", 0.5f);
            Cat.SetBool("running", false);
            Cat.SetBool("walking", true);
        } else if (!sprinting)
        {
            Cat.SetFloat("motionSpeed", 0);
            Cat.SetBool("walking", false);
            Cat.SetBool("running", false);
            Idle();
        }


    }


      void Idle()
    {
        //if idle animation is used, blend to it from here
        //for now, assume there is no idle animation, and blend the model to its static position
        
    }
}
