using System.Collections;
using System.Collections.Generic;
using MyGame;
using TMPro;
using UnityEngine;

namespace Global
{
    public class DialogManager : MonoBehaviour
    {
        public Queue<string> sentences; //FIFO
        public TextMeshProUGUI characterNameTextField;
        public TextMeshProUGUI dialogTextField;
        public Canvas dialogCanvas;
        public bool isOpened = false;

        public static DialogManager Instance;

        private float letterTypingSpeed = 0.05f;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            dialogCanvas.gameObject.SetActive(true);
            characterNameTextField.SetText(dialogue.name);
            sentences.Clear();

            foreach (var sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentece();
        }

        public void Update()
        {
            if (dialogCanvas.isActiveAndEnabled)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
        }

        public void DisplayNextSentece()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        IEnumerator TypeSentence(string sentence)
        {
            dialogTextField.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogTextField.text += letter;
                yield return new WaitForSecondsRealtime(letterTypingSpeed);
            }
        }

        public void EndDialogue()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            Time.timeScale = 1;
            dialogCanvas.gameObject.SetActive(false);
        }
    }
}