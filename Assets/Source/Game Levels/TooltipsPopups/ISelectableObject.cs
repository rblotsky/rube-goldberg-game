using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public interface ISelectableObject
    {
        public void ActivateSelectionPanel(UISelectionBox selectionPanel);
    }
}