using System;
using Data;
using MyGame;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Global
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private PlayableDirector gameOverTimeline;
        [SerializeField] private PlayableDirector levelCompletedTimeline;
        [SerializeField] private GameObject gameOverCanvas;
        [SerializeField] private GameObject levelCompletedCanvas;


        public void PlayLevelCompletedTimeline()
        {
            AudioManager.Instance.StopAllMusic();
            AudioManager.Instance.StopAllSounds();
            levelCompletedTimeline.Play();
        }
        public void PlayGameOverTimeline()
        {
            AudioManager.Instance.StopAllMusic();
            AudioManager.Instance.StopAllSounds();
            Cursor.lockState = CursorLockMode.Locked;

            gameOverTimeline.Play();
        }
        public void ShowLevelCompletedCanvas()
        {
            levelCompletedCanvas.SetActive(true);
        }

        public void GoToNextLevel()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            Debug.Log("The current scene is: " + currentScene);
            switch (currentScene)
            {
                case "FirstLevel":
                    Debug.Log("Go to second level");
                    StartTransitionToLevel("ForestScene");
                    break;
                case "ForestScene":
                    StartTransitionToLevel("ThirdLevel");
                    break;
                case "ThirdLevel":
                    Debug.Log("Victory. GG");
                    break;
            }
        }

        public void StartTransitionToLevel(string nextLevelName)
        {
            Debug.Log("Going to second level");
            SceneManager.LoadScene(nextLevelName);
        }



        public void GameOver()
        {
            SavePlayerInfo();
            AudioManager.Instance.PlayMusic(SoundNames.MainMenu);
            Cursor.lockState = CursorLockMode.None;

            SceneManager.LoadScene(LevelNames.MainMenuScene);
        }

        public void ShowGameOverCanvas()
        {
            gameOverCanvas.SetActive(true);
        }
        private void SavePlayerInfo()
        {
            var playerInfo = new PlayerInfo
            {
                playerName = PlayerPrefs.GetString(PlayerPrefNames.Username),
                musicVolume = PlayerPrefs.GetFloat(PlayerPrefNames.MusicVolume),
                soundVolume = PlayerPrefs.GetFloat(PlayerPrefNames.SoundVolume),
                levelName = SceneManager.GetActiveScene().name,
                healthPoints = PlayerPrefs.GetFloat(PlayerPrefNames.Health),
                vaccineBags = PlayerPrefs.GetInt(PlayerPrefNames.VaccineBags),
                mainBag = PlayerPrefs.GetInt(PlayerPrefNames.MainBag)
            };
            SaveSystem.SavePlayerInfoToJson(playerInfo);
        }
    }
}