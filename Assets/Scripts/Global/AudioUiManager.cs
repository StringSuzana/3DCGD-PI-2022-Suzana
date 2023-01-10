using MyGame;
using UnityEngine;

namespace Global
{
    public class AudioUiManager : MonoBehaviour
    {
        public void PLayHoverSound()
        {
            AudioManager.Instance.PlaySoundOneTime(SoundNames.Hover);
        }

        public void PLayClickSound()
        {
            AudioManager.Instance.PlaySoundOneTime(SoundNames.Click);
        }

        public void PLayHealthBarSound()
        {
            AudioManager.Instance.PlaySoundOneTime(SoundNames.Click);
        }
    }
}