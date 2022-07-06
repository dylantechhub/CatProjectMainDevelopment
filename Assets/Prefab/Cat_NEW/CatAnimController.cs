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

        if (Cat.GetFloat("mainBlend") > 1)
            t_walking = 1;
        if (Cat.GetFloat("mainBlend") < 0)
            t_walking = 0;
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
    bool sprintLock;
    float t_walking;

    void Main()
    {
        if (TPC != null)
        {
            if (TPC.canclimb && sprinting)
            {
                sprintLock = true;
            }
            else
                sprintLock = false;

            //update animator with paramters that would be used by the old animator, makes life much more easier

            Cat.SetBool("grounded", TPC.Grounded);

            Cat.SetBool("isClimbing", TPC.canclimb);


            //sprinting
            if (TPC._input.sprint || TPC.sprinting)
            {
                sprinting = true;
                if (!sprintLock)
                {
                    Cat.speed = 3f;
                    t_running += 0.9f * Time.deltaTime;

                    Cat.SetFloat("runningBlend", t_running);

                    Cat.SetBool("idle", false);
                    Cat.SetBool("climbIdle", false);
                }
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
                    sprinting = false;
                    if (!sprintLock)
                    {
                        Cat.speed = 1;
                        if (!TPC.canclimb)
                            Idle();
                        else
                            ClimbIdle();
                    }
                }
            }


            //walking
            if (TPC._input.move != Vector2.zero || TPC.moving && !sprinting)
            {

                t_walking += 3f * Time.deltaTime;

                Cat.SetFloat("mainBlend", t_walking);

                Cat.SetBool("climbIdle", false);

                Cat.SetBool("idle", false);
            }
            else if (!sprinting)
            {

                if (Cat.GetFloat("mainBlend") > 0)
                    t_walking -= 2.2f * Time.deltaTime;

                Cat.SetFloat("mainBlend", t_walking);

                //only stop the animation if blending is completed
                if (Cat.GetFloat("mainBlend") <= 0)
                {
                        Cat.speed = 1;
                        if (!TPC.canclimb)
                            Idle();
                        else
                            ClimbIdle();
                }


            }
        }
    }
}
