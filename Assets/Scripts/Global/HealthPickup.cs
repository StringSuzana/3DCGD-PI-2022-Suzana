using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class HealthPickup : MonoBehaviour
    {
        [SerializeField] 
        private int heathAmount;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("ON TRIGGER ENTER HEALTH PICKUP");

            if (other.gameObject.TryGetComponent(out IPlayer player))
            {
                Debug.Log("PLAYER DETECTED");
                player.Heal(heathAmount);
            }
        }

    }
}