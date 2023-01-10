using Assets.Scripts.Global;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class GameManager : MonoBehaviour, IGameManager
    {

        [SerializeField] private PlayableDirector _gameOverTimeline;

        public void PlayGameOverTimeline()
        {
            AudioManager.Instance.StopAllMusic();
            Cursor.lockState = CursorLockMode.Locked;

            _gameOverTimeline.Play();
        }

        public void GameOver()
        {
            AudioManager.Instance.PlayMusic(SoundNames.MainMenu);
            var username = PlayerPrefs.GetString(PlayerPrefNames.Username);
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(LevelNames.MainMenuScene);
        }


    }
}
