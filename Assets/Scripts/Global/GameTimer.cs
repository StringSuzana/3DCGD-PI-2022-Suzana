using System.Linq;
using Data;
using Global;
using MyGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private InventoryObject inventoryOfBags;

    [SerializeField] private float timeLeft = 300.0f; //Game time = 5 minutes

    private bool _startCountdown = false;

    void Update()
    {
        if (_startCountdown == false) return;

        timeLeft -= Time.deltaTime;
        countdownText.text = "Time left: " + Mathf.Round(timeLeft);

        if (timeLeft < 0)
        {
            if (IsMainBagCollected())
            {
                SaveBagInfo();
                gameManager.GetComponent<GameManager>().PlayLevelCompletedTimeline();
            }
            else
            {
                Debug.Log("Game over");
                gameManager.GetComponent<GameManager>().GameOver();
            }
        }

        else if (IsVictory())
        {
            SaveBagInfo();
            Debug.Log("Make a button for next level");
            gameManager.GetComponent<GameManager>().PlayLevelCompletedTimeline();
        }
    }

    private void SaveBagInfo()
    {
        PlayerPrefs.SetInt(PlayerPrefNames.MainBag,
            inventoryOfBags.Container.FirstOrDefault(slot => slot.item.itemType == ItemType.MainVaccineBag)!.amount);
        
        PlayerPrefs.SetInt(PlayerPrefNames.VaccineBags,
            inventoryOfBags.Container.FirstOrDefault(slot => slot.item.itemType == ItemType.VaccineBag)!.amount);
    }

    public void StartTimerTrigger()
    {
        _startCountdown = true;
    }

    public bool IsVictory()
    {
        int mainBagCount =
            inventoryOfBags.Container.FirstOrDefault(slot => slot.item.itemType == ItemType.MainVaccineBag)!.amount;
        int vaccineBagCount =
            inventoryOfBags.Container.FirstOrDefault(slot => slot.item.itemType == ItemType.VaccineBag)!.amount;

        return mainBagCount == GameData.MaxMainBag && vaccineBagCount == GameData.MaxVaccineBags;
    }

    public bool IsMainBagCollected()
    {
        int mainBagCount =
            inventoryOfBags.Container.FirstOrDefault(slot => slot.item.itemType == ItemType.MainVaccineBag)!.amount;
        return mainBagCount == GameData.MaxMainBag;
    }
}