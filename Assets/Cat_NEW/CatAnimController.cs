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

    void Main()
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
            Cat.SetBool("idle", false);
        }
        else
        {
            sprinting = false;
            //set movment speed to 0, therefore stopping the animation
            Cat.SetFloat("motionSpeed", 0);
            Cat.SetBool("running", false);
            Cat.SetBool("walking", false);
            Idle();
        }


        Cat.speed = TPC._speed;
        Cat.SetFloat("speed", TPC._speed);

        //walking
        if (TPC._input.move != Vector2.zero && !sprinting)
        {
            Cat.SetFloat("motionSpeed", 0.5f);
            Cat.SetBool("running", false);
            Cat.SetBool("walking", true);
            Cat.SetBool("idle", false);
        }
        else if (!sprinting)
        {
            Cat.SetFloat("motionSpeed", 0);
            Cat.SetBool("walking", false);
            Cat.SetBool("running", false);
            Idle();
        }

        if (Cat.GetBool("idle"))
        {
           
        }

    }
}
