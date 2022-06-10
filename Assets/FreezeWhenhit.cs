using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeWhenhit : MonoBehaviour
{
    public Rigidbody BridgeRig;

    private void OnTriggerEnter(Collider other)
    {

        BridgeRig.freezeRotation = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
