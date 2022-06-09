using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class CatAnimController : MonoBehaviour
{
    public ThirdPersonController TPC;
    public Animator Cat;

    private bool sprinting;

    // Update is called once per frame
    void Update()
    {
        Main();

        //blend
        if (Cat.GetFloat("runningBlend") > 1)
            t_running = 1;
        if (Cat.GetFloat("runningBlend") < 0)
            t_running = 0;
    }

    public void Start()
    {
        Idle();
    }

    void ClimbIdle()
    {
        Cat.SetBool("climbIdle", true);
    }

    void Idle()
    {
        Cat.SetBool("idle", true);
        
    }

    //running blend is walking/running blend
    float t_running;


    void Main()
    {

        //update animator with paramters that would be used by the old animator, makes life much more easier

        Cat.SetBool("grounded", TPC.Grounded);

        Cat.SetFloat("mainBlend", TPC._speed / 1);

        Cat.SetBool("isClimbing", TPC.canclimb);

        //sprinting
        if (TPC._input.sprint)
        {
            Cat.speed = 3f;
            sprinting = true;
            t_running += 0.9f * Time.deltaTime;

            Cat.SetFloat("runningBlend", t_running);

            Cat.SetBool("idle", false);
            Cat.SetBool("climbIdle", false);
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

                if (!TPC.canclimb)
                    Idle();
                else
                    ClimbIdle();
            }
        }

       

        //walking
        if (TPC._input.move != Vector2.zero && !sprinting)
        {
            Cat.SetBool("climbIdle", false);

            Cat.SetBool("idle", false);
        }
        else if (!sprinting)
        {
            if (!TPC.canclimb)
                Idle();
            else
                ClimbIdle();
        }

    }
}
