using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
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