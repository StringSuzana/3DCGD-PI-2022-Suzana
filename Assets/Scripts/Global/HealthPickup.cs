using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class HealthPickup : MonoBehaviour
    {
        [SerializeField] 
        private int healthAmount;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IPlayer player))
            {
                Debug.Log($@"Player will gain {healthAmount}");
                player.Heal(healthAmount);
                Destroy(gameObject);
            }
        }

    }
}