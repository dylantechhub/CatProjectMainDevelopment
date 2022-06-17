using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    public ParticleSystem waterParticles;
    public bool isSplashing = false;

    public Transform catTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSplashing = true;
            waterParticles.transform.position = catTransform.localPosition;
            waterParticles.Play();
        }
    }

    public void Update()
    {
        if (isSplashing)
        {
            waterParticles.transform.position = catTransform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSplashing = false;
            waterParticles.Stop();
        }
    }
}
