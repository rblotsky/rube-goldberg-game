using UnityEngine;
using System.Collections.Generic;

namespace RubeGoldbergGame
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data Object")]
    public class LevelData: ScriptableObject
    {
        // DATA //
        // Basic Level Data
        public string levelFileName;
        public string displayName;
        public string displayDescription;
        public LevelData nextLevel;
        public int bestObjectivesCompleted;

        public Completion completionStatus = Completion.NotPassed;
        public string[] objectiveDescriptions = new string[3];


        // FUNCTIONS //
        // External Management
        public bool UpdateLevelBests(Completion completion, bool[] instanceCompletedObjectives)
        {
            // If the level is won, updates the completion status and the completed objectives
            if (completion == Completion.Passed)
            {
                completionStatus = completion;

                // Counts up number of completed objectives
                int numObjectivesCompleted = 0;
                for(int i = 0; i < instanceCompletedObjectives.Length; i++)
                {
                    if(instanceCompletedObjectives[i] == true)
                    {
                        numObjectivesCompleted++;
                    }
                }

                // The best objectives completed is the highest of current and new value. Returns true if the new value is higher.
                if(numObjectivesCompleted > bestObjectivesCompleted)
                {
                    bestObjectivesCompleted = numObjectivesCompleted;
                    return true;
                }
            }

            // Returns false by default if player didn't get more objectives completed this run
            return false;
        }


        // Data Management
        public void ResetLevelData()
        {
            completionStatus = Completion.NotPassed;
            bestObjectivesCompleted = 0;
        }

        public string SaveLevelData()
        {
            // Saves data
            string levelDataString = name + "," + (int)completionStatus;
            levelDataString += ",";
            levelDataString += bestObjectivesCompleted.ToString();

            // Returns it
            return levelDataString;
        }

        public bool LoadLeveldata(string[] levelDataItems)
        {
            // Expected format: LevelName,Completion,Objective1Completion,Objective2Completion,Objective3Completion
            // If invalid amount of items, doesn't load and returns false
            if(levelDataItems.Length != 2)
            {
                return false;
            }

            int completionStatusInt = (int)Completion.NotPassed;
            if(!int.TryParse(levelDataItems[1], out completionStatusInt))
            {
                completionStatus = Completion.NotPassed;
            }
            else
            {
                completionStatus = (Completion)completionStatusInt;
            }

            if (int.TryParse(levelDataItems[2], out bestObjectivesCompleted))
            {
                bestObjectivesCompleted = 0;
            }

            // Returns true if all worked properly
            return true;
        }
    }
}