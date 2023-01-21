using System.Collections;
using System.Collections.Generic;
using Characters;
using TMPro;
using UnityEngine;

namespace Characters
{
    public class Npc : MonoBehaviour, INpc
    {
        [SerializeField] private Canvas dialogueCanvas;
        [SerializeField] private TMP_Text interactText;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip interactClip;


        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            dialogueCanvas.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IPlayer>() == null) return;
            Interact();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<IPlayer>() == null) return;
            EndInteraction();
        }
        public void Interact()
        {
            //audioSource.PlayOneShot(interactClip);
            PlayInteractSound();
            //interactText.text = "Hello, I am an NPC.";
            ShowDialogueCanvas();
        }

        public void EndInteraction()
        {
            HideDialogueCanvas();
        }
        private void PlayInteractSound()
        {
            AudioSource.PlayClipAtPoint(interactClip, transform.position);
        }

        private void ShowDialogueCanvas()
        {
            dialogueCanvas.enabled = true;
        }

        private void HideDialogueCanvas()
        {
            dialogueCanvas.enabled = false;
        }


    }
}