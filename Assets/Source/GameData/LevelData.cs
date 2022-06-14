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

        public Completion completionStatus = Completion.NotPassed;
        public string[] objectiveDescriptions = new string[3];
        public bool[] objectivesCompleted = { false, false, false };


        // FUNCTIONS //
        // External Management
        public void UpdateLevelBests(Completion completion, bool[] instanceCompletedObjectives)
        {
            // If the level is won, updates the completion status and the completed objectives
            if (completion == Completion.Passed)
            {
                completionStatus = completion;

                // Loops through objectives completed and sets them to true if they are true now or were true before
                // NOTE: This is a bad way to do it because if objectives are done in different runs, they'll still be marked as completed. Maybe we should just track the # of completed objectives and not which ones they are.
                for(int i = 0; i < objectivesCompleted.Length; i++)
                {
                    objectivesCompleted[i] = (objectivesCompleted[i] || instanceCompletedObjectives[i]);
                }
            }
        }


        // Data Management
        public void ResetLevelData()
        {
            completionStatus = Completion.NotPassed;
            for(int i = 0; i < objectivesCompleted.Length; i++)
            {
                objectivesCompleted[i] = false;
            }
        }

        public string SaveLevelData()
        {
            // Saves data
            string levelDataString = name + "," + (int)completionStatus;
            for(int i = 0; i < objectivesCompleted.Length; i++)
            {
                levelDataString += ",";
                levelDataString += objectivesCompleted[i].ToString();
            }

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
                return false;
            }
            else
            {
                completionStatus = (Completion)completionStatusInt;
            }

            if (!bool.TryParse(levelDataItems[2], out objectivesCompleted[0]))
            {
                return false;
            }
            else
            {
                objectivesCompleted[0] = false;
            }

            if (!bool.TryParse(levelDataItems[3], out objectivesCompleted[1]))
            {
                return false;
            }
            else
            {
                objectivesCompleted[1] = false;
            }

            if (!bool.TryParse(levelDataItems[4], out objectivesCompleted[2]))
            {
                return false;
            }
            else
            {
                objectivesCompleted[2] = false;
            }

            // Returns true if all worked properly
            return true;
        }
    }
}