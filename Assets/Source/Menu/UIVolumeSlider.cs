using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

namespace RubeGoldbergGame
{
    public class UIVolumeSlider : MonoBehaviour
    {
        // DATA //
        // Scene References
        public TextMeshProUGUI currentValueText;
        public Slider propertySlider;
        public AudioMixer mainAudioMixer;

        // Volume Data
        public string audioMixPropertyName;
        public static readonly float minValue = -80;
        public static readonly float maxValue = 20;
        public float currentValue;


        // FUNCTIONS //
        // Unity Defaults
        private void OnEnable()
        {
            // Gets the current volume value
            mainAudioMixer.GetFloat(audioMixPropertyName, out currentValue);

            // Updates UI with the new values
            UpdateUI();
        }


        // UI Management
        public void UpdateUI()
        {
            // Updates UI slider with new values for min/max and current
            propertySlider.minValue = minValue;
            propertySlider.maxValue = maxValue;
            propertySlider.value = currentValue;
            currentValueText.SetText(currentValue.ToString());
        }


        // UI Events
        public void OnSliderValueChange()
        {
            // Updates current value
            currentValue = propertySlider.value;

            // Updates main audio mixer
            mainAudioMixer.SetFloat(audioMixPropertyName, currentValue);

            // Updates the current value text
            currentValueText.SetText(currentValue.ToString());
        }

    }
}