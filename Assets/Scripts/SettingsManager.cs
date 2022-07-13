using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MyGame
{

    public class SettingsManager : MonoBehaviour
    {
        [SerializeField]
        private Canvas settingsMenu;
        [SerializeField]
        private Slider musicSlider;
        [SerializeField]
        private Slider soundSlider;
        private void Start()
        {
            //Get music and sound volume and return 1 if value does not exist
            var musicVolume = PlayerPrefs.GetFloat(PlayerPrefNames.MusicVolume, 1);
            var soundVolume = PlayerPrefs.GetFloat(PlayerPrefNames.SoundVolume, 1);

            musicSlider.value = musicVolume;
            soundSlider.value = soundVolume;

            settingsMenu.gameObject.SetActive(false);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Settings or Quit");
                settingsMenu.gameObject.SetActive(!settingsMenu.isActiveAndEnabled);
            }
            if (settingsMenu.isActiveAndEnabled)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = true;
                Time.timeScale = 1;
            }
        }
        public void SetMusicVolume()
        {
            PlayerPrefs.SetFloat(PlayerPrefNames.MusicVolume, musicSlider.value);
            AudioManager.Instance.SetMusicVolume(musicSlider.value);
        }
        public void SetSoundsVolume()
        {
            PlayerPrefs.SetFloat(PlayerPrefNames.SoundVolume, soundSlider.value);
            AudioManager.Instance.SetSoundsVolume(soundSlider.value);
        }
        public void CloseMenu()
        {
            settingsMenu.gameObject.SetActive(false);
        }
        public void OpenMenu()
        {
            settingsMenu.gameObject.SetActive(true);
        }
        public void ExitButtonPressed()
        {
            AudioManager.Instance.PlayMusic(SoundNames.MainMenu);
            SceneManager.LoadScene(LevelNames.MainMenuScene);
        }
    }
}
