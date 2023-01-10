using System;
using System.Collections;
using Assets.Scripts.Constants;
using Data;
using MyGame;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Global
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
                    playerName = username,
                    healthPoints = GameData.MaxPlayerHealth,
                    levelName = LevelNames.FirstLevel,
                    musicVolume = 1,
                    soundVolume = 1
                };
                SaveSystem.SavePlayerInfoToJson(playerInfo);
                PlayerInfo = playerInfo;
            }
            else
            {
                ShowMessage($"Welcome back, {username}", Color.green);
                PlayerInfo = SaveSystem.LoadPlayerInfoFromJson(username);
                if (PlayerInfo.healthPoints == 0)
                {
                    ShowMessage("You will start from first level", Color.yellow);
                    PlayerInfo.levelName = LevelNames.FirstLevel;
                    PlayerInfo.healthPoints = GameData.MaxPlayerHealth;
                }
            }

            SetPlayerPrefs();
        }

        private void SetPlayerPrefs()
        {
            PlayerPrefs.SetFloat(PlayerPrefNames.Health, PlayerInfo.healthPoints);
            PlayerPrefs.SetString(PlayerPrefNames.Username,        PlayerInfo.playerName);
            PlayerPrefs.SetString(PlayerPrefNames.LastPlayedLevel, PlayerInfo.levelName);
            PlayerPrefs.SetFloat(PlayerPrefNames.MusicVolume, PlayerInfo.musicVolume);
            PlayerPrefs.SetFloat(PlayerPrefNames.SoundVolume, PlayerInfo.soundVolume);
        }

        private IEnumerator LoadGameForPlayer(String username)
        {
            yield return new WaitForSecondsRealtime(6);

            SceneManager.LoadScene(PlayerInfo.levelName);
        }

        private IEnumerator PlaySoundStart(int waitForSeconds)
        {
            AudioManager.Instance.PlaySoundOneTime(SoundNames.GameStart);
            yield return new WaitForSecondsRealtime(waitForSeconds);

            LevelSongs.dict.TryGetValue(PlayerInfo.levelName, out string levelSongName);
            AudioManager.Instance.PlayMusic(levelSongName);
        }
    }
}