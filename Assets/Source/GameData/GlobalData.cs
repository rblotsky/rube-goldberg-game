using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace RubeGoldbergGame
{
    public static class GlobalData
    {
        // DATA //
        // Game data
        public static List<LevelData> gameLevels = new List<LevelData>();

        // Cached data
        private static bool isGameLoaded = false;

        // Constants
        public static readonly string SAVE_FILE_NAME = "GameData.csv";

        // Properties
        public static string saveFilePath { get { return Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME); } }


        // FUNCTIONS //
        // Game management
        public static LevelData GetLevel(int ID)
        {
            return gameLevels.Find(x => x.levelID == ID);
        }

        public static LevelData GetLevel(string name)
        {
            return gameLevels.Find(x => x.name == name);
        }
        
        public static void SetupGame()
        {
            // Does nothing if game is already loaded
            if(isGameLoaded)
            {
                return;
            }

            // Finds all LevelData gameObjects
            gameLevels = new List<LevelData>(Resources.LoadAll<LevelData>("LevelData"));

            // Loads the game data
            LoadGameData();

            // Updates game loaded status
            isGameLoaded = true;
        }


        // Saving/Loading
        public static void SaveGameData()
        {
            // Creates a streamwriter that'll write to the file (overwriting the existing file)
            // Note: In case of a saving failure, this will delete the previous save. Maybe do something about that?
            StreamWriter fileWriter = new StreamWriter(saveFilePath);

            // Writes all leveldata one line at a time
            foreach(LevelData level in gameLevels)
            {
                fileWriter.WriteLine(level.SaveLevelData());
            }

            // Closes the streamwriter
            fileWriter.Close();

            // Logs a success
            Debug.Log("Successfully saved the game! File: " + saveFilePath);
        }

        public static void LoadGameData()
        {
            // Creates a streamreader to read from the save file (if it exists)
            StreamReader fileReader = null;
            try
            {
                fileReader = new StreamReader(saveFilePath);
            }
            catch(FileNotFoundException)
            {
                Debug.LogWarning(string.Format("Could not load from \"{}\" because the file was not found!", saveFilePath));
                return;
            }

            // If the file reader was successfully created, reads each line and loads it individually
            string[] lines = fileReader.ReadToEnd().Split("\n");

            foreach(string line in lines)
            {
                // Gets the data items from this line
                string[] lineData = line.Trim().Split(",");

                // If there's exactly 4 items, assumes it's a correctly formatted line and loads its data
                // Expected format: LevelName,BestTime,BestBlocks,Completion
                if(lineData.Length == 4)
                {
                    // Tries loading, logs a warning if it failed to find the level data object
                    try
                    {
                        if(!GetLevel(lineData[0]).LoadLeveldata(lineData))
                        {
                            // Prints an error message if it returns false (loading error)
                            Debug.LogWarning(string.Format("Had an issue loading level data line \"{}\"!", line));
                        }
                    }
                    catch(NullReferenceException)
                    {
                        Debug.LogWarning(string.Format("Could not load level with name \"{}\" because it could not be found! Save file: \"{}\"", lineData[0], saveFilePath));
                    }
                }
            }

            // Closes the file reader
            fileReader.Close();

            // Logs a success
            Debug.Log("Successfully loaded the game! File: " + saveFilePath);
        }
    }
}