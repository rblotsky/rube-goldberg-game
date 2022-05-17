using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    [CreateAssetMenu(fileName = "New Block Asset", menuName = "Block Asset")]
    public class BlockAsset : ScriptableObject
    {
        // DATA //
        // References
        public BlockBase blockPrefab;
    }
}