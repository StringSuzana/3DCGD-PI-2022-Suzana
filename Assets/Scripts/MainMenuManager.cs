using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class MainMenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            Debug.Log("Start Game Pressed");
            SceneManager.LoadScene(LevelNames.FirstLevel);
        }

        public void OpenSettings()
        {
            SceneManager.LoadScene(LevelNames.Settings);
        }
    }
}
