using System.Collections.Generic;

namespace RuthGoldbergGame
{
    public static class GlobalData
    {
        static List<LevelData>levelsInGame = new List<LevelData>();
        //looking online people say scriptable objects are the best way to store data for levels
        //maybe we should have
        //TODO: populating levelsInGame at runtime
        //maybe have all files in a single JSON file, and unload all things from that file with a function
        
        /* static void ChangeCompletionOnLevel(int id, Completion newCompl)
        {
            levelsInGame.Find(x => x.LevelID == id).IsBeat = newCompl;
        }*/
    }
}