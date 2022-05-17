using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class ObjectPusher : BlockBase
    {
        /*
        // DATA //
        public BoxCollider2D triggerArea;
        

        // FUNCTIONS //
        // Unity Defaults
        private void OnTriggerStay2D(Collider2D col)
        {
            TriggerBlockFunctionality(col);
        }
        

        // Block Functionality Functions
        //triggerArea collider will only be active during simulation runtime
        //allows tooltips to work only on the object
        public override void ToggleTriggerArea(bool inSimMode)
        {
            triggerArea.enabled = inSimMode;
        }
        
        public override void TriggerBlockFunctionality()
        {
            Debug.Log("This should do something to anything that goes in my trigger zone");
        }

        public void TriggerBlockFunctionality(Collider2D col)
        {
            col.attachedRigidbody.velocity += Vector2.right * gameObject.transform.right;
        }
        */
    }
}