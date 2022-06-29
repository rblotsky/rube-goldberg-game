using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    //this will be put on objects that are secondary to placeables
    public class BlockBaseExtension : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public BlockBase baseCore; // the head
        
        public void OnPointerEnter(PointerEventData pointerData)
        {
            baseCore.OnPointerEnter(pointerData);
        }
        public void OnPointerExit(PointerEventData pointerData)
        {
            baseCore.OnPointerExit(pointerData);
        }
    }
}