using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public static class GlobalData
    {
        // DATA //
        public static List<LevelData> gameLevels = new List<LevelData>();
        //looking online people say scriptable objects are the best way to store data for levels
        //maybe we should have
        //TODO: populating levelsInGame at runtime
        //maybe have all files in a single JSON file, and unload all things from that file with a function
        
        /* static void ChangeCompletionOnLevel(int id, Completion newCompl)
        {
            GameLevels.Find(x => x.LevelID == id).IsBeat = newCompl;
        }*/
        

        // FUNCTIONS //
        public static LevelData GetLevel(int ID)
        {
            return GlobalData.gameLevels.Find(x => x.LevelID == ID);
        }
        
        public static void PopulateGameLevels()
        {
            //Note: We can use a dictionary for fast lookup
            // Uses Unity's Resources class to find all items of type "LevelData" within a folder named "Resources"
            gameLevels = new List<LevelData>(Resources.FindObjectsOfTypeAll<LevelData>());
        }
    }
}