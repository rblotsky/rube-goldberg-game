using UnityEngine;

namespace RubeGoldbergGame
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data Object")]
    public class LevelData: ScriptableObject
    {
        // DATA //
        // Basic Level Data
        public int levelID;
        public string displayName;
        public string displayDescription;
        public float bestTime;
        public int bestBlocksUsed;
        public Completion completionStatus;


        // FUNCTIONS //
        // External Management
        public void ResetLevelData()
        {
            bestTime = -1;
            bestBlocksUsed = -1;
            completionStatus = Completion.NotPassed;
        }

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
    }
}