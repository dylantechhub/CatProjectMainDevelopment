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

        //blends
        if (Cat.GetFloat("runningBlend") > 1)
            t = 1;
        if (Cat.GetFloat("runningBlend") < 0)
            t = 0;
    }


    void Idle()
    {
        Cat.SetBool("idle", true);
        
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
            t += 0.9f * Time.deltaTime;

            //double the movement speed, therefore sprint
            Cat.SetFloat("motionSpeed", 0.6f);

            Cat.SetFloat("runningBlend", t);

            Cat.SetBool("idle", false);
        }
        else
        {
            //dont blend past 0
            if (Cat.GetFloat("runningBlend") > 0)
                t -= 2.2f * Time.deltaTime;

            Cat.SetFloat("runningBlend", t);

            //only stop the animation if blending is completed
            if (Cat.GetFloat("runningBlend") <= 0)
            {
                Cat.speed = 1;
                sprinting = false;
                //set movment speed to 0, therefore stopping the animation
                Cat.SetFloat("motionSpeed", 0);
                Idle();
            }
        }

        Cat.SetFloat("speed", TPC._speed);

        //walking
        if (TPC._input.move != Vector2.zero && !sprinting)
        {
            Cat.SetFloat("motionSpeed", 0.8f);
            Cat.SetBool("idle", false);
        }
        else if (!sprinting)
        {
            Cat.SetFloat("motionSpeed", 0);
            Idle();
        }

    }
}
