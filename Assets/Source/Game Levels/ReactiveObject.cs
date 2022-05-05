using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ReactiveObject : BlockBase
    {
        //this is a test object
        private void OnTriggerStay2D(Collider2D col)
        {
            TriggerBlockFunctionality(col);
        }

        private void OnMouseEnter()
        {
            //Debug.Log("I am hovered over!");
            FindObjectOfType<LevelUIManager>().ToggleTooltipUI(displayName +": "+ displayDescription); //TODO replace this with a better method
        }

        private void OnMouseExit()
        {
            //Debug.Log("I am not hovered over anymore");
            FindObjectOfType<LevelUIManager>().ToggleTooltipUI("");
        }

        public new void TriggerBlockFunctionality()
        {
            Debug.Log("This should do something to anything that goes in my trigger zone");
        }

        public void TriggerBlockFunctionality(Collider2D col)
        {
            col.attachedRigidbody.velocity += Vector2.right * gameObject.transform.right;
        }
        
        

        //private void OnTrigger2D
    }
}