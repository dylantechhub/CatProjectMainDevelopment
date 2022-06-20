using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader_Spinner : MonoBehaviour
{
    void Update()
    {
        //spinny spin spin spin
        transform.Rotate(Vector3.one * 90 * Time.deltaTime);
        transform.Rotate(Vector3.up * 90 * Time.deltaTime);
    }
}
