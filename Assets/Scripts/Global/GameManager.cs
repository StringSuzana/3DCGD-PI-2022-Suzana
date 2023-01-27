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
                    StartTransitionToLevel(LevelNames.SecondLevel);
                    break;
                case "ForestScene":
                    StartTransitionToLevel(LevelNames.ThirdLevel);
                    break;
                case "ThirdLevel":
                    Debug.Log("Victory. GG");
                    break;
            }
        }

        

        public void StartTransitionToLevel(string nextLevelName)
        {
            Debug.Log("Going to next level");
            SceneManager.LoadScene(nextLevelName);
        }


        public void GameOver()
        {
            SavePlayerInfoGameOver();
            AudioManager.Instance.PlayMusic(SoundNames.MainMenu);
            Cursor.lockState = CursorLockMode.None;

            SceneManager.LoadScene(LevelNames.MainMenuScene);
        }

        public void ShowGameOverCanvas()
        {
            gameOverCanvas.SetActive(true);
        }

        private void SavePlayerInfoGameOver()
        {
            /*Reset values so that player starts from the beginning*/
            PlayerInfo playerInfo = new PlayerInfo
            {
                playerName = PlayerPrefs.GetString(PlayerPrefNames.Username),
                musicVolume = PlayerPrefs.GetFloat(PlayerPrefNames.MusicVolume),
                soundVolume = PlayerPrefs.GetFloat(PlayerPrefNames.SoundVolume),
                levelName = LevelNames.FirstLevel,
                healthPoints = GameData.MaxPlayerHealth,
                vaccineBags = 0,
                mainBag = 0
            };
            SaveSystem.SavePlayerInfoToJson(playerInfo);
        }
    }
}