using UnityEngine;
using System.Collections.Generic;

namespace RubeGoldbergGame
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data Object")]
    public class LevelData: ScriptableObject
    {
        // DATA //
        // Basic Level Data
        public int levelID;
        public string levelFileName;
        public string displayName;
        public string displayDescription;
        public float bestTime;
        public int bestBlocksUsed;
        public Completion completionStatus;
        public LevelData nextLevel;


        // FUNCTIONS //
        // External Management
        public void UpdateLevelBests(float newTime, int newBlocksUsed)
        {
            // If level isn't complete yet, sets these as the best regardless of old values
            if (completionStatus == Completion.NotPassed)
            {
                bestTime = newTime;
                bestBlocksUsed = newBlocksUsed;
                completionStatus = Completion.Passed;
            }

            // Otherwise, takes the lowest of the old and new values
            else
            {
                bestTime = Mathf.Min(bestTime, newTime);
                bestBlocksUsed = Mathf.Min(bestBlocksUsed, newBlocksUsed);
            }
        }


        // Data Management
        public void ResetLevelData()
        {
            bestTime = -1;
            bestBlocksUsed = -1;
            completionStatus = Completion.NotPassed;
        }

        public string SaveLevelData()
        {
            return name + "," + bestTime + "," + bestBlocksUsed + "," + (int)completionStatus + ",";
        }

        public bool LoadLeveldata(string[] levelDataItems)
        {

            // Expected format: LevelName,BestTime,BestBlocks,Completion
            // If invalid amount of items, doesn't load and returns false
            if(levelDataItems.Length != 4)
            {
                return false;
            }

            // Otherwise, loads the data and returns false on a parsing error
            if(!float.TryParse(levelDataItems[1], out bestTime))
            {
                return false;
            }
            
            if(!int.TryParse(levelDataItems[2], out bestBlocksUsed))
            {
                return false;
            }

            int completionStatusInt = (int)Completion.NotPassed;
            if(!int.TryParse(levelDataItems[3], out completionStatusInt))
            {
                return false;
            }
            else
            {
                completionStatus = (Completion)completionStatusInt;
            }

            // Returns true if all worked properly
            return true;
        }
    }
}