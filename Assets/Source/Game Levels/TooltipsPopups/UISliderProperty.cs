using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace RubeGoldbergGame
{
    public class UISliderProperty : MonoBehaviour
    {
        // DATA //
        // References
        public Slider propertySlider;
        public TextMeshProUGUI propertyTitle;
        public TextMeshProUGUI propertyValueText;

        // Cached data
        private event FloatValueDelegate onValueChangeEvent;


        // FUNCTIONS //
        // Management
        public void SetupProperty(string propName, float currentValue, float minVal, float maxVal, FloatValueDelegate newOnChangeFunc)
        {
            // Updates values
            onValueChangeEvent = newOnChangeFunc;
            propertyTitle.SetText(propName);
            propertySlider.minValue = minVal;
            propertySlider.maxValue = maxVal;

            // Sets current value, running OnSliderValueChange in the process
            propertySlider.value = currentValue;
            propertyValueText.SetText(currentValue.ToString());
        }

        public void DisconnectProperty()
        {
            // Disconnects from the given function so it won't get reset to 0 once the object is destroyed
            onValueChangeEvent = null;
        }


        // UI Events
        public void OnSliderValueChange()
        {
            propertyValueText.SetText(propertySlider.value.ToString());
            if (onValueChangeEvent != null)
            {
                onValueChangeEvent(propertySlider.value);
            }
        }
    }
}