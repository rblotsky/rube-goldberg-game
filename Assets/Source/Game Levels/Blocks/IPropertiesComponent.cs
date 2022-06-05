using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public interface IPropertiesComponent
    {
        public void ActivateSelectionPanel(UISelectionBox selectionPanel);
        public string SaveProperties();
        public void LoadProperties(string[] propertyStrings);
    }
}