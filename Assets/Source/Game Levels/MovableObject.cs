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
        private MinimalCollisionsObjective collisionTracker;


        // FUNCTIONS //
        // Unity defaults
        protected override void Awake()
        {
            // Runs TooltipComponent awake
            base.Awake();

            // Finds some other objects/components
            collisionTracker = FindObjectOfType<MinimalCollisionsObjective>();
            objRigidbody = GetComponent<Rigidbody2D>();

            // Sets initial position to the instantiation position
            initialPosition = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collisionTracker != null)
            {
                collisionTracker.HandleObjectiveCollision();
            }
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