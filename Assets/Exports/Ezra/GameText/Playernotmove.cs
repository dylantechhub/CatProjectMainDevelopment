using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace StarterAssets
{
    public class Playernotmove : MonoBehaviour
    {
        [Header("Goal Event Triggered")]
        public UnityEvent[] triggeredEvent;

        [Header("Player Move Timer")]
        public float timer = 12;
        [Header("Amount of storybeats")]
        public int Saywhat;
        private StarterAssetsInputs AssetsInput;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (AssetsInput.move.x == 0 || AssetsInput.move.y == 0)
            {
                timer -= Time.deltaTime;
            }
            if(timer < 0)
            {
                Random.Range(1, 3);
            }
            
        }
        public void Triggered()
        {

            // call our goal event
            triggeredEvent[Saywhat].Invoke();

        }
    }
}
