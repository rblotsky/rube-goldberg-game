using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class MovableObject : TooltipComponent
    {
        // DATA //
        // Toggles
        public bool isObjectiveObject = false;
        public string displayName;
        public string displayDescription;

        // Cached data
        private Vector3 initialPosition;
        private Rigidbody2D objRigidbody;


        // FUNCTIONS //
        // Unity defaults
        protected override void Awake()
        {
            // Runs TooltipComponent awake
            base.Awake();

            // Finds some other objects/components
            objRigidbody = GetComponent<Rigidbody2D>();

            // Sets initial position to the instantiation position
            initialPosition = transform.position;
        }


        // Object Management
        public void ResetToInitialValues()
        {
            transform.position = initialPosition;
            objRigidbody.velocity = Vector3.zero;
            objRigidbody.angularVelocity = 0;
        }

        // Interface Functions
        public override string GetTooltipText()
        {
            return LanguageManager.TranslateFromEnglish(displayName + "\n\n" + displayDescription);
        }
    }
}