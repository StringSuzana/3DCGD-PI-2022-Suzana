using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Slider Slider;

        void Start()
        {
            Slider.maxValue = GameData.MaxPlayerHealth;
            Slider.minValue = 0;
        }
        public void SetHealth(float healthPoints)
        {
            Slider.value = healthPoints;
        }

    }
}
