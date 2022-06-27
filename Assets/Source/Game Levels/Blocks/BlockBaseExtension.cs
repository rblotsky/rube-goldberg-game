using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    //this will be put on objects that are secondary to placeables
    public class RopeBlockBaseExt : BlockBase
    {
        public BlockBase ropeBlockBase; // the head
        
        public override void OnPointerEnter(PointerEventData pointerData)
        {
            ropeBlockBase.OnPointerEnter(pointerData);
        }
        public override void OnPointerExit(PointerEventData pointerData)
        {
            ropeBlockBase.OnPointerExit(pointerData);
        }
    }
}