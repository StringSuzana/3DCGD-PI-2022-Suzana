using UnityEngine;

namespace Global
{

    public class DialoguePlayer : MonoBehaviour
    {
        [SerializeField]
        private Dialogue dialogue;
        public void TriggerDialogue()
        {
            foreach (var s in dialogue.sentences)
            {
               // Debug.Log(s);
            }
            DialogManager.Instance.StartDialogue(dialogue);
        }

        void Start()
        {
           // TriggerDialogue();
        }

    }

    [System.Serializable]
    public class Dialogue
    {
        public string name;
        [TextArea(3, 10)]
        public string[] sentences;
    }
}
