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
        [SerializeField]
        private TMP_InputField PlayerName;
        [SerializeField]
        private TextMeshProUGUI Message;

        private PlayerInfo PlayerInfo;

        private void Start()
        {
            Message.text = "";
        }
        public void OpenSettings()
        {
            SceneManager.LoadScene(LevelNames.Settings);
        }
        public void ShowMessage(String message, Color color)
        {
            Message.text = $"!! {message} !!";
            Message.color = color;
        }
        public void StartGame()
        {
            if (String.IsNullOrWhiteSpace(PlayerName.text))
            {
                ShowMessage("Enter your name", Color.red);
            }
            else
            {
                ShowMessage("Welcome", Color.green);
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
                //Create NEW data
                SaveSystem.SavePlayerInfoToJson(new PlayerInfo
                {
                    PlayerName = username,
                    HealthPoints = GameData.MaxPlayerHealth,
                    LevelName = LevelNames.FirstLevel,
                    MusicVolume = PlayerPrefs.GetFloat(PlayerPrefNames.MusicVolume),
                    SoundVolume = PlayerPrefs.GetFloat(PlayerPrefNames.SoundVolume)
                });

            }
            InitializePlayerInfoProperty(username);
            SetPlayerPrefs();
        }

        private void SetPlayerPrefs()
        {
            PlayerPrefs.SetFloat(PlayerPrefNames.Health, PlayerInfo.HealthPoints);
            PlayerPrefs.SetString(PlayerPrefNames.Username, PlayerInfo.PlayerName);
        }

        private void InitializePlayerInfoProperty(String username)
        {
            PlayerInfo = SaveSystem.LoadPlayerInfoFromJson(username);
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
