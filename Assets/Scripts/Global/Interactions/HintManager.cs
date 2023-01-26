using System.Collections;
using System.Collections.Generic;
using Global;
using TMPro;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerTextField;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Canvas hintCanvas;

    public static HintManager Instance;

    private float letterTypingStops = 0.09f;

    public bool IsDone = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public bool IsHintActive()
    {
        return hintCanvas.isActiveAndEnabled;
    }

    public void StartHint(Hint hint)
    {
        IsDone = false;
        hintCanvas.gameObject.SetActive(true);
        headerTextField.SetText(hint.name);

        DisplaySentence(hint.sentence);
    }

    public void DisplaySentence(string sentence)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        textField.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textField.text += letter;
            yield return new WaitForSecondsRealtime(letterTypingStops);
        }

        IsDone = true;
    }

    public void EndDialogue()
    {
        hintCanvas.gameObject.SetActive(false);
    }
}