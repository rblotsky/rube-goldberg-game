using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class LinearSlowTimeframe : MonoBehaviour
    {
        
        // DATA //
        private float timeElapsed = 0;
        private float lerpDuration = 2f;
        private float startTimeScale= 100;
        private float endTimeScale = 0;
        private float valueToLerp;
        private LevelManager levelManager;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            timeElapsed = 0;
            levelManager = FindObjectOfType<LevelManager>(true);
        }

        void Update()
        {
            // If in editor mode, freezes
            if(!levelManager.inSimulation)
            {
                timeElapsed = lerpDuration;
            }

            if (timeElapsed < lerpDuration)
            {
                valueToLerp = Mathf.Lerp(startTimeScale, endTimeScale, timeElapsed / lerpDuration);
                Time.timeScale = valueToLerp / 100f;
                timeElapsed += Time.unscaledDeltaTime;
            }

            if (timeElapsed >= lerpDuration)
            {
                Time.timeScale = 0f;
                this.enabled = false;
            }
        }


        // External Management
        public void StartTimeSlow(float initTimeScale)
        {
            startTimeScale = initTimeScale;
            timeElapsed = 0;
            Debug.Log("Started Lerp Process!");
        }

    }
}