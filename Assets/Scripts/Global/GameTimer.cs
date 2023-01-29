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
                Debug.Log("Main bag collected");
                SaveBagInfo();
                gameManager.GetComponent<GameManager>().PlayLevelCompletedTimeline();
            }
            else
            {
                Debug.Log("Main bag not collected");
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
        InventorySlot mainContainer =
            inventoryOfBags.Container.FirstOrDefault(slot => slot.item.itemType == ItemType.MainVaccineBag);
        int mainBagCount = mainContainer?.amount ?? 0;

        InventorySlot vaccineContainer =
            inventoryOfBags.Container.FirstOrDefault(slot => slot.item.itemType == ItemType.VaccineBag);
        int vaccineBagCount = vaccineContainer?.amount ?? 0;

        return mainBagCount == GameData.MaxMainBag && vaccineBagCount == GameData.MaxVaccineBags;
    }

    public bool IsMainBagCollected()
    {
        InventorySlot mainContainer =
            inventoryOfBags.Container.FirstOrDefault(slot => slot.item.itemType == ItemType.MainVaccineBag);
        int mainBagCount = mainContainer?.amount ?? 0;

        return mainBagCount == GameData.MaxMainBag;
    }
}