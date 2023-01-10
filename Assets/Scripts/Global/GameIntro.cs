using UnityEngine;

namespace Global
{

    public class GameIntro : MonoBehaviour
    {
        [SerializeField]
        private Dialogue dialogue;
        public void TriggerDialogue(Dialogue gameIntro)
        {
            foreach (var s in gameIntro.sentences)
            {
               // Debug.Log(s);
            }
            DialogManager.Instance.StartDialogue(gameIntro);
        }

        void Start()
        {
            TriggerDialogue(dialogue);
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
