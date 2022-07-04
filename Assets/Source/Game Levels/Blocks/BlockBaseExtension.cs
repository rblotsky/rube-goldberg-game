using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    //this will be put on objects that are secondary to placeables
    public class BlockBaseExtension : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public BlockBase baseCore; // the head

        public bool localIsHoveredOn = false;

        
        private void Start()
        {
            baseCore = GetComponentInParent<BlockBase>();
            baseCore.levelManager.onSimulationStart += enableColliders;
            baseCore.levelManager.onSimulationFinish += disableColliders;
        }
        
        private void OnDestroy()
        {
            baseCore.levelManager.onSimulationStart -= enableColliders;
            baseCore.levelManager.onSimulationFinish -= disableColliders;
        }
        

        private void enableColliders(LevelManager thisLevelManager)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        
        private void disableColliders()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        public void OnPointerEnter(PointerEventData pointerData)
        {
            baseCore.OnPointerEnter(pointerData);
            localIsHoveredOn = true;
        }
        public void OnPointerExit(PointerEventData pointerData)
        {
            if (localIsHoveredOn)
            {
                baseCore.OnPointerExit(pointerData);
                localIsHoveredOn = false;
            }
            
        }
    }
}