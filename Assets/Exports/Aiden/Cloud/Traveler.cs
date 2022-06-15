using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platforms;

public class Traveler : MonoBehaviour
{
    public bool X;
    public bool DirectionSwitch;
    public bool Y;
    public bool Z;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<MovingPlatform>())
        {
            if (collision.gameObject.GetComponent<MovingPlatform>().MovingPlatformID == "Traveler")
            {
                EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).posX = collision.transform.position.x;
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).posY = collision.transform.position.y;
                EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).posZ = collision.transform.position.z;

                if (DirectionSwitch)
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).Direction = true;
             if (X)
                {
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).X = true;
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).Y = false;
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).Z = false;
                } else if (Y)
                {
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).X = false;
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).Y = true;
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).Z = false;
                } else if (Z)
                {
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).X = false;
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).Y = false;
                    EditPlatforms.Moving(GetPlatform.MovingPlatform("Traveler")).Z = true;
                }
            }
        }
    }
}
