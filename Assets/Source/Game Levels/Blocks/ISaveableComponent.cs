using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public interface ISaveableComponent
    {
        public string SaveComponent();
        public void LoadComponent(string[] savedData);
    }
}