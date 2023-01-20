using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Characters
{
    public class Npc : MonoBehaviour, INpc
    {
        public void Interact()
        {
            
            Debug.Log("interact with player!!!!");
        }
    }
}