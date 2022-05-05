using UnityEngine;

namespace RubeGoldbergGame
{
    public class ActiveObject : BlockBase
    {
        public new void TriggerBlockFunctionality()
        {
            Debug.Log("This should do something to anything that goes in my trigger zone");
        }
        
        //private void OnTrigger2D
    }
}