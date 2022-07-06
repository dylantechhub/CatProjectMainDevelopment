using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;

public class ButtonEvent : MonoBehaviour
{
    public ThirdPersonController TPC;

    private void Awake()
    {
        TPC = GameObject.Find("cat").GetComponent<ThirdPersonController>();
        TPC.SprintSpeed = 1;
        TPC.MoveSpeed = 0.2f;
        StarterAssetsInputs.Menuopen = true;
    }

    public bool sprinting;

    public float Wtaps = 0;

    public bool JUMP;
    [Header("The position of the latest touch")]
    public Vector2 Tpos;

    public Vector3 TD;

    private void Update()
    {
        float inputMagnitude = 1f;
        float currentHorizontalSpeed = new Vector3(TPC._controller.velocity.x, 0.0f, TPC._controller.velocity.z).magnitude;

        //IMPORTANT!!!
        if (!sprinting)
            TPC._speed = Mathf.Lerp(currentHorizontalSpeed, 1.5f * inputMagnitude, Time.deltaTime * TPC.SpeedChangeRate);
        else
            TPC._speed = Mathf.Lerp(currentHorizontalSpeed, 3 * inputMagnitude, Time.deltaTime * TPC.SpeedChangeRate);

        if (Touchscreen.current.press.isPressed)
        {

            Tpos = Touchscreen.current.position.ReadValue();

            

            // W 
            if (Tpos.x > 220 && Tpos.x < 420 && Tpos.y > 364 && Tpos.y < 571)
            {

                float _targetRotation;

                if (!TPC.canclimb)
                {
                    _targetRotation = Mathf.Atan2(TPC.transform.position.x, TPC.transform.position.y) * Mathf.Rad2Deg + TPC._mainCamera.transform.eulerAngles.y;
                    var targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;


                    float rotation = Mathf.SmoothDampAngle(TPC.transform.eulerAngles.y, _targetRotation, ref TPC._rotationVelocity, 2f);


                    // rotate to face input direction relative to camera position
                    TPC.gameObject.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);





                    // round speed to 3 decimal places
                    TPC._speed = Mathf.Round(TPC._speed * 100000f) / 100000f;

                    print(TPC.targetDirection);

                    TPC.moving = true;
                    TPC._controller.Move(targetDirection * (TPC._speed * Time.deltaTime) + new Vector3(0.0f, TPC._verticalVelocity, 0.0f) * Time.deltaTime);
                }

            }
            //A KEY (LEFT)
            else if (Tpos.x > 33 && Tpos.x < 220 && Tpos.y > 171 && Tpos.y < 364)
            {
                TPC._cinemachineTargetYaw -= 15 * Time.deltaTime;

                //This controls the way the cat moves, by rotating on the Y axis
                TPC._targetRotation += TPC._cinemachineTargetYaw;

                float rotation = Mathf.SmoothDampAngle(TPC.transform.eulerAngles.y, TPC._targetRotation, ref TPC._rotationVelocity, 2f);


                // rotate to face input direction relative to camera position
                TPC.gameObject.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            } 
            //D KEY (RIGHT)
            else if (Tpos.x > 423 && Tpos.x < 616 && Tpos.y > 171 && Tpos.y < 364)
            {
                TPC._cinemachineTargetYaw += 15 * Time.deltaTime;
                //This controls the way the cat moves, by rotating on the Y axis
                TPC._targetRotation += TPC._cinemachineTargetYaw;

                float rotation = Mathf.SmoothDampAngle(TPC.transform.eulerAngles.y, TPC._targetRotation, ref TPC._rotationVelocity, 2f);


                // rotate to face input direction relative to camera position
                TPC.gameObject.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            else if (Tpos.x > 1956 && Tpos.x < 2156 && Tpos.y > 254 && Tpos.y < 444)
            {
                JUMP = true;
            }
        }




        if (Touchscreen.current.press.wasPressedThisFrame)
        {
            Tpos = Touchscreen.current.position.ReadValue();

            //WKEY
            if (Tpos.x > 220 && Tpos.x < 420 && Tpos.y > 364 && Tpos.y < 571)
            {

                //sprint controller Wtaps = times W was pressed. (press W 2 times in 1.3 seconds to sprint!)
                Wtaps++;
                if (Wtaps == 1 || Wtaps == 2)
                    StartCoroutine(CheckForSprinting());
            }

            //jump controller

            if (JUMP)
            {
                JUMP = false;
                TPC.jump = true;
            }
        }


        if (Touchscreen.current.press.wasReleasedThisFrame)
        {
            Tpos = Touchscreen.current.position.ReadValue();

            JUMP = false;

            TPC.moving = false;
            TPC.sprinting = false;
            sprinting = false;
        }
    }

    public IEnumerator CheckForSprinting()
    {
        if (Wtaps == 2)
        {
            sprinting = true;
            TPC.sprinting = true;
        }
        else
            yield return new WaitForSecondsRealtime(4f);
        Wtaps = 0;

    }

}
