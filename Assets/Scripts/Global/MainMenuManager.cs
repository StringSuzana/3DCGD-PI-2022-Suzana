using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace MyGame
{
    public class MainMenuManager : MonoBehaviour
    {
    
        public void OpenSettings()
        {
            SceneManager.LoadScene(LevelNames.Settings);
        }
        public void StartGame()
        {
            Debug.Log("Start Game Pressed");
            StartCoroutine(PlaySoundStart());

            StartCoroutine(LoadGame());
        }
        private IEnumerator LoadGame()
        {
            yield return new WaitForSecondsRealtime(6);
            SceneManager.LoadScene(LevelNames.FirstLevel);

        }
        private IEnumerator PlaySoundStart()
        {
            AudioManager.Instance.PlaySoundOneTime(SoundNames.GameStart);
            yield return new WaitForSecondsRealtime(6);

            AudioManager.Instance.PlayMusic(SoundNames.FirstLevel);


        }
    }
}
