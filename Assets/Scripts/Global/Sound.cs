using UnityEngine;

namespace MyGame
{
    [System.Serializable]
    public class Sound
    {
        public AudioClip audioClip;
        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 2f)]
        public float pitch;
        public string name;
        [HideInInspector]
        public AudioSource source;
    }
}