using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ReactiveObject : BlockBase
    {
        //this is a test object
        private void OnTriggerEnter2D(Collider2D col)
        {
            TriggerBlockFunctionality();
        }

        public new void TriggerBlockFunctionality()
        {
            Debug.Log("This should do something to anything that goes in my trigger zone");
        }
        
        //private void OnTrigger2D
    }
}