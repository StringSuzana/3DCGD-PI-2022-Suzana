using Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{

    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject gameManager;

    [SerializeField] private float timeLeft = 300.0f;//Game time = 5 minutes

    private bool _startCountdown = false;

    void Update()
    {
        if (_startCountdown == false) return;

        timeLeft -= Time.deltaTime;
        countdownText.text = "Time left: " + Mathf.Round(timeLeft);
        Debug.Log(timeLeft);
        if (timeLeft < 0)
        {
            Debug.Log("Game over");
            gameManager.GetComponent<GameManager>().GameOver();
        }
    }

    public void StartTimerTrigger()
    {
        _startCountdown = true;
    }
}