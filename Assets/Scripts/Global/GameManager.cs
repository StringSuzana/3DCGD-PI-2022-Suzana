using Assets.Scripts.Global;
using UnityEngine;
using UnityEngine.Playables;

namespace MyGame
{
    public class GameManager : MonoBehaviour, IGameManager
    {

        [SerializeField] private PlayableDirector _gameOverTimeline;

        public void PlayGameOver()
        {
            _gameOverTimeline.Play();
        }


    }
}
