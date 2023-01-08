using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] 
        private Slider Slider;

        [SerializeField] 
        private TMP_Text HealthAmount;

        void Start()
        {
            Slider.maxValue = GameData.MaxPlayerHealth;
            Slider.minValue = 0;
            Debug.Log("Reset to max health in HealthBar script");
            HealthAmount.text = $@"{GameData.MaxPlayerHealth}/{GameData.MaxPlayerHealth}";
        }

        public void SetHealth(float healthPoints)
        {
            Slider.value = healthPoints;
            HealthAmount.text = $@"{healthPoints}/{GameData.MaxPlayerHealth}";
        }
    }
}