using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Global
{
    public class GameIntroTriggers : MonoBehaviour
    {
        [SerializeField] private DialoguePlayer dialoguePlayer;

        public void StartIntroDialogue()
        {
            dialoguePlayer.TriggerDialogue();
        }
    }
}