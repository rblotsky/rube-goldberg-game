using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class LevelCompletionStats
    {
        // DATA //
        // Statistics (Private)
        private Completion levelCompletion;
        private int bestBlocksUsed = -1;
        private float bestTime = -1;
        private bool[] optionalsCompleted = { false, false, false };
        private float[] bestTimeWithOptionalAmounts = { -1, -1, -1 };
        private int[] bestBlocksWithOptionalAmounts = { -1, -1, -1 };

        // Properties (Public accessors for statistics)
        public int MainObjBestBlocks { get { return bestBlocksUsed; } }
        public float MainObjBestTime { get { return bestTime; } }
        public bool[] CompletedOptionals { get { return optionalsCompleted; } }
        public float[] OptionalAmountBestTimes { get { return bestTimeWithOptionalAmounts; } }
        public int[] OptionalAmountBestBlocks { get { return bestBlocksWithOptionalAmounts; } }


        // CONSTRUCTOR //
        //TODO


        // FUNCTIONS //
        // Managing Bests
        public void UpdateOptionalBests(int numOptionals, int blocksUsed, float timeTaken)
        {
            //TODO
        }

        public void UpdateLevelStats(bool isComplete)
        {
            //TODO
            // Consider merging this with UpdateOptionalBests b/c 0 optionals is a valid amount of optionals to complete
        }
    
}
}