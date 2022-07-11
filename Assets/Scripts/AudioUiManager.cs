using System.Collections;
using UnityEngine;

namespace MyGame
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

    }
}