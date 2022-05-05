using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class MovableObject : MonoBehaviour
    {
        // DATA //
        // Toggles
        public bool isObjectiveObject = false;
        public string displayName;
        public string displayDescription;
        public Sprite displaySprite;
        
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
    }
}