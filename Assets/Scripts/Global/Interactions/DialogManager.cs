using System.Collections;
using System.Collections.Generic;
using MyGame;
using TMPro;
using UnityEngine;

namespace Global
{
    public class DialogManager : MonoBehaviour
    {
        public Queue<string> Sentences; //FIFO
        public TextMeshProUGUI headerTextField;
        public TextMeshProUGUI textField;
        public Canvas dialogCanvas;
        public bool isOpened = false;
        public bool stopTime;

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

            Sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            dialogCanvas.gameObject.SetActive(true);
            headerTextField.SetText(dialogue.name);
            Sentences.Clear();

            foreach (var sentence in dialogue.sentences)
            {
                Sentences.Enqueue(sentence);
            }

            DisplayNextSentece();
        }

        public void Update()
        {
            if (dialogCanvas.isActiveAndEnabled)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = stopTime == true ? 0 : 1;
            }
        }

        public void DisplayNextSentece()
        {
            if (Sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = Sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        IEnumerator TypeSentence(string sentence)
        {
            textField.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                textField.text += letter;
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