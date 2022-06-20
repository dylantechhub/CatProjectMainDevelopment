using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_CameraRotate : MonoBehaviour
{
    public GameObject Cat;

    private Vector3 point;

    void Start()
    {
        point = Cat.transform.position; 
        transform.LookAt(point); 
    }

    private void Update()
    {
        transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 20 * 0.005f);
    }


}
