using System;
using System.Collections;
using MyGame;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Global
{
    public class AudioManager : MonoBehaviour
    {
        internal bool isPaused = false;

        [Tooltip("The volume when this sound button is toggled on")]
        public float volumeOn = 1;

        [Tooltip("The volume when this sound button is toggled off")]
        public float volumeOff = 0;

        public Sound[] music;
        public Sound[] soundFxs;

        public static AudioManager Instance;

        void Awake()
        {
            #region SINGLETON

            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            #endregion

            PlayerPrefs.SetFloat(PlayerPrefNames.MusicVolume, 1);
            PlayerPrefs.SetFloat(PlayerPrefNames.SoundVolume, 1);

            foreach (var m in music)
            {
                m.source = gameObject.AddComponent<AudioSource>();
                m.source.clip = m.audioClip;
                m.source.volume = PlayerPrefs.GetFloat(PlayerPrefNames.MusicVolume);
                m.source.pitch = m.pitch;
                m.source.loop = true;
            }

            foreach (var s in soundFxs)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.audioClip;
                s.source.volume = PlayerPrefs.GetFloat(PlayerPrefNames.SoundVolume);
                s.source.pitch = s.pitch;
            }

           // DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Cursor.visible = true;
            PlayMusic(SceneManager.GetActiveScene().name);
        }

        public void PlaySoundOneTime(string name)
        {
            Sound s = Array.Find(soundFxs, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log("AudioClip not found => maybe the name in inspector is wrong or it is not there");
                return;
            }

            s.source.volume = PlayerPrefs.GetFloat(PlayerPrefNames.SoundVolume);
            s.source.PlayOneShot(s.audioClip);
        }

        public void PlayMusic(string name)
        {
            foreach (var old in music)
            {
                old.source.Stop();
            }

            Sound m = Array.Find(music, song => song.name == name);
            if (m == null)
            {
                Debug.Log("Sound not found => maybe the name in inspector is wrong or it is not there");
                return;
            }

            m.source.volume = PlayerPrefs.GetFloat(PlayerPrefNames.MusicVolume);
            m.source.Play();
        }

        public void PlayTransition(string songName, float transitionTime)
        {
            StartCoroutine(CrossFadeAudio(songName, transitionTime));
        }

        public IEnumerator CrossFadeAudio(string songName, float transitionTime)
        {
            Sound song = Array.Find(music, m => m.name == songName);
            if (song == null)
            {
                Debug.Log(
                    "AudioClip not found => maybe the name in inspector is wrong or it is not there=> " + songName);
                yield return null;
            }

            float timeOut = 1;
            while (timeOut > 0)
            {
                foreach (var m in music)
                {
                    if (m?.source.isPlaying == true)
                    {
                        m.source.volume = timeOut;
                        if (timeOut - transitionTime <= 0)
                        {
                            Debug.Log("Stopped playing: " + m.source.name);
                            m.source.Stop();
                        }
                    }
                }

                if ((1 - timeOut) <= PlayerPrefs.GetFloat(PlayerPrefNames.MusicVolume))
                {
                    song.source.volume = (1 - timeOut);
                }
                else
                {
                    song.source.volume = PlayerPrefs.GetFloat(PlayerPrefNames.MusicVolume);
                }

                if (!song.source.isPlaying)
                {
                    Debug.Log("PLAY");
                    song.source.Play();
                }

                yield return new WaitForSeconds(transitionTime);
                timeOut -= transitionTime;
            }
        }

        public void StopAllSounds()
        {
            foreach (var s in soundFxs)
            {
                if (s?.source.isPlaying == true)
                {
                    s.source.Stop();
                }
            }
        }

        public void StopAllMusic()
        {
            foreach (var s in music)
            {
                if (s?.source.isPlaying == true)
                {
                    s.source.Stop();
                }
            }
        }

        public void SetMusicVolume(float value)
        {
            PlayerPrefs.SetFloat(PlayerPrefNames.MusicVolume, value);
            foreach (var m in music)
            {
                m.source.volume = value;
            }
        }

        public void SetSoundsVolume(float value)
        {
            PlayerPrefs.SetFloat(PlayerPrefNames.SoundVolume, value);
            foreach (var s in soundFxs)
            {
                s.source.volume = value;
            }
        }
    }
}