using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platforms;

public class Testing : MonoBehaviour
{
    public GameObject ChangeTo;
    public bool test = false;

    void Update()
    {
        if (test)
        {
            test = false;
            if ("this is my string".Contains("this"))
                Debug.Log("true");
            EditPlatforms.Rotating(GetPlatform.RotatingPlatform("Example_RP")).Change(ChangeTo);
        }
    }
}
