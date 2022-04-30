using System.Collections.Generic;

namespace RuthGoldbergGame
{
    public static class GlobalData
    {
        static List<LevelData>levelsInGame = new List<LevelData>();
        //looking online people say scriptable objects are the best way to store data for levels
        //maybe we should have
        //TODO: create a function to load a level from json file
        //have all files in a single file, and unload all things from that file
    }
}