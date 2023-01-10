using Data;
using MyGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Global
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
            Debug.Log("Reset to health from PlayerPrefs in HealthBar script");
            HealthAmount.text = $@"{PlayerPrefs.GetFloat(PlayerPrefNames.Health)}/{GameData.MaxPlayerHealth}";
        }

        public void SetHealth(float healthPoints)
        {
            Slider.value = healthPoints;
            HealthAmount.text = $@"{healthPoints}/{GameData.MaxPlayerHealth}";
        }
    }
}