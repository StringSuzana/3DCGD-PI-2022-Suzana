using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float timeLeft = 30.0f;
    public Text countdownText;

    void Update()
    {
        timeLeft -= Time.deltaTime;
        countdownText.text = "Time left: " + Mathf.Round(timeLeft);
        if (timeLeft < 0)
        {
            Debug.Log("Game over");
            //GAME OVER (START AGAIN)
        }
    }
}