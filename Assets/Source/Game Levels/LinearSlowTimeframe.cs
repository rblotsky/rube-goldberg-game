using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class LinearSlowTimeframe : MonoBehaviour
    {
        
        public float timeElapsed = 0;
        private float lerpDuration = 2f;
        private float startValue= 100;
        private float endValue = 0;
        public float valueToLerp;


        private void Awake()
        {
            timeElapsed = 0;
        }

        public void startLerp(float initTimeValue)
        {
            startValue = initTimeValue;
            timeElapsed = 0;
            Debug.Log("Started Lerp Process!");
        }

        void Update()
        {
            if (timeElapsed < lerpDuration)
            {
                valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                Time.timeScale = valueToLerp / 100f;
                timeElapsed += Time.unscaledDeltaTime;
            }

            if (timeElapsed >= lerpDuration)
            {
                Time.timeScale = 0f;
                this.enabled = false;
            }
        }
    }
}