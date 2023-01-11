using MyGame;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Global
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private PlayableDirector gameOverTimeline;
        [SerializeField] private PlayableDirector firstLevelCompletedTimeline;

        public void PlayFirstLevelCompleted()
        {
            AudioManager.Instance.StopAllMusic();
            AudioManager.Instance.StopAllSounds();
            firstLevelCompletedTimeline.Play();

        }

        public void GoToNextLevel()
        {

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
    }
}