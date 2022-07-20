using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class BackgroundHitbox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool isHovering = false;
        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isHovering)
                {
                    if (ManagerSelectingBase.SelectingManagerInstance.enabled)
                    {
                        ManagerSelectingBase.SelectingManagerInstance.ObjectClickedOn(gameObject);
                    }
                }
            }
        }
    }
}