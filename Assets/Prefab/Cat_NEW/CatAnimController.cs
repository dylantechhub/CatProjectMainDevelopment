using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class CatAnimController : MonoBehaviour
{
    public ThirdPersonController TPC;
    public Animator Cat;

    void Start()
    {

        //get the cats gameobject, then get the TPC
        //TPC = GameObject.Find("cat").GetComponent<ThirdPersonController>();

        //get the animator
        //Cat = GetComponent<Animator>();
    }

    private bool sprinting;

    // Update is called once per frame
    void Update()
    {
        Main();

        //blends
        if (Cat.GetFloat("runningBlend") > 1)
            t_running = 1;
        if (Cat.GetFloat("runningBlend") < 0)
            t_running = 0;

        if (Cat.GetFloat("mainBlend") > 1)
            t_main = 1;
        if (Cat.GetFloat("mainBlend") < 0.5 && !jumping)
            t_main = 0.5f;
        if (Cat.GetFloat("mainBlend") < 0 && jumping)
            t_main = 0;
    }


    void Idle()
    {
        Cat.SetBool("idle", true);
        
    }
    bool jumping = false;

    float t_running;
    float t_main;
    float t_jumpIP;

    void Main()
    {

        //update animator with paramters that would be used by the old animator, makes life much more easier

        //sprinting
        if (TPC._input.sprint)
        {
            Cat.speed = 3f;
            sprinting = true;
            t_running += 0.9f * Time.deltaTime;

            //double the movement speed, therefore sprint


            Cat.SetFloat("runningBlend", t_running);

            Cat.SetBool("idle", false);
        }
        else
        {
            //dont blend past 0
            if (Cat.GetFloat("runningBlend") > 0)
                t_running -= 2.2f * Time.deltaTime;

            Cat.SetFloat("runningBlend", t_running);

            //only stop the animation if blending is completed
            if (Cat.GetFloat("runningBlend") <= 0)
            {
                Cat.speed = 1;
                sprinting = false;
                //set movment speed to 0, therefore stopping the animation
                
                Idle();
            }
        }

       

        //walking
        if (TPC._input.move != Vector2.zero && !sprinting)
        {
            t_main += 0.9f * Time.deltaTime;
            Cat.SetFloat("mainBlend", t_main);
            Cat.SetBool("idle", false);
        }
        else if (!sprinting)
        {
            //dont blend past 0.5, as thats the Idle Animation blend. 0 is IP_Jump blend
            if (Cat.GetFloat("mainBlend") > 0.5)
                t_main -= 2.2f * Time.deltaTime;
            Cat.SetFloat("mainBlend", t_main);
            Idle();
        }



        //jumping, IN PLACE
        if (TPC._input.jump && !sprinting && TPC._input.move == Vector2.zero)
        {
            jumping = true;

            //set main blend to Blend to jump blend
            t_main -= 2.2f * Time.deltaTime;
            Cat.SetFloat("mainBlend", t_main);



        } else if (jumping && TPC.Grounded)
        {
            jumping = false;


        }

    }
}
