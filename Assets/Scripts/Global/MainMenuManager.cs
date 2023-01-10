using Assets.Scripts.Constants;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField PlayerName;
        [SerializeField] private TextMeshProUGUI Message;

        private PlayerInfo PlayerInfo;

        private void Start()
        {
            Message.text = "";
        }

        public void OpenSettings()
        {
            SceneManager.LoadScene(LevelNames.Settings);
        }

        public void ShowMessage(string message, Color color)
        {
            Message.text = $"!! {message} !!";
            Message.color = color;
        }

        public void StartGame()
        {
            if (string.IsNullOrWhiteSpace(PlayerName.text))
            {
                ShowMessage("Enter your name", Color.red);
            }
            else
            {
                SavePlayerInfo(PlayerName.text);

                StartCoroutine(PlaySoundStart(6));
                StartCoroutine(LoadGameForPlayer(PlayerName.text));
            }
        }

        private void SavePlayerInfo(string username)
        {
            PlayerPrefs.SetString("username", username);
            if (SaveSystem.LoadPlayerInfoFromJson(username) == null)
            {
                ShowMessage("Welcome", Color.green);
                //Create NEW data
                var playerInfo = new PlayerInfo
                {
                    PlayerName = username,
                    HealthPoints = GameData.MaxPlayerHealth,
                    LevelName = LevelNames.FirstLevel,
                    MusicVolume = 1,
                    SoundVolume = 1
                };
                SaveSystem.SavePlayerInfoToJson(playerInfo);
                PlayerInfo = playerInfo;
            }
            else
            {
                ShowMessage($"Welcome back, {username}", Color.green);
                PlayerInfo = SaveSystem.LoadPlayerInfoFromJson(username);
                if (PlayerInfo.HealthPoints == 0)
                {
                    ShowMessage("You will start from first level", Color.yellow);
                    PlayerInfo.LevelName = LevelNames.FirstLevel;
                    PlayerInfo.HealthPoints = GameData.MaxPlayerHealth;
                }
            }

            SetPlayerPrefs();
        }

        private void SetPlayerPrefs()
        {
            PlayerPrefs.SetFloat(PlayerPrefNames.Health, PlayerInfo.HealthPoints);
            PlayerPrefs.SetString(PlayerPrefNames.Username,        PlayerInfo.PlayerName);
            PlayerPrefs.SetString(PlayerPrefNames.LastPlayedLevel, PlayerInfo.LevelName);
            PlayerPrefs.SetFloat(PlayerPrefNames.MusicVolume, PlayerInfo.MusicVolume);
            PlayerPrefs.SetFloat(PlayerPrefNames.SoundVolume, PlayerInfo.SoundVolume);
        }

        private IEnumerator LoadGameForPlayer(String username)
        {
            yield return new WaitForSecondsRealtime(6);

            SceneManager.LoadScene(PlayerInfo.LevelName);
        }

        private IEnumerator PlaySoundStart(int waitForSeconds)
        {
            AudioManager.Instance.PlaySoundOneTime(SoundNames.GameStart);
            yield return new WaitForSecondsRealtime(waitForSeconds);

            LevelSongs.dict.TryGetValue(PlayerInfo.LevelName, out string levelSongName);
            AudioManager.Instance.PlayMusic(levelSongName);
        }
    }
}