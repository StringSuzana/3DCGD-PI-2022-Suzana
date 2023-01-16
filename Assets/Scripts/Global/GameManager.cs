using System;
using System.Collections;
using System.Collections.Generic;
using MyGame;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Global
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private PlayableDirector gameOverTimeline;
        [SerializeField] private PlayableDirector levelCompletedTimeline;
        [SerializeField] private GameObject gameOverCanvas;
        [SerializeField] private GameObject levelCompletedCanvas;


        public void PlayLevelCompleted()
        {
            AudioManager.Instance.StopAllMusic();
            AudioManager.Instance.StopAllSounds();
            levelCompletedTimeline.Play();
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

        public void PlayGameOverTimeline()
        {
            AudioManager.Instance.StopAllMusic();
            AudioManager.Instance.StopAllSounds();
            Cursor.lockState = CursorLockMode.Locked;

            gameOverTimeline.Play();
        }

        public void GameOver()
        {
            AudioManager.Instance.PlayMusic(SoundNames.MainMenu);
            string username = PlayerPrefs.GetString(PlayerPrefNames.Username);
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(LevelNames.MainMenuScene);
        }

        public void ShowGameOverCanvas()
        {
            gameOverCanvas.SetActive(true);
        }
    }
}