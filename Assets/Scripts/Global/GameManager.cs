using MyGame;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Global
{
    public class GameManager : MonoBehaviour, IGameManager
    {

        [SerializeField] private PlayableDirector gameOverTimeline;

        public void PlayGameOverTimeline()
        {
            AudioManager.Instance.StopAllMusic();
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


    }
}
