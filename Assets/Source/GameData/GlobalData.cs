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
        // Overall Game Management
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

            // Loads the language manager
            LanguageManager.RegenerateTranslationTable();

            // Finds all LevelData gameObjects
            gameLevels = new List<LevelData>(Resources.LoadAll<LevelData>("LevelData"));

            // Loads the game data
            LoadGameData();

            // Updates game loaded status
            isGameLoaded = true;
        }


        // Saving/Loading Game
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

            // If in the unity editor, saves unrecorded strings
#if UNITY_EDITOR
            LanguageManager.SaveUnrecordedStrings();
#endif

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
                Debug.LogWarning(string.Format("Could not load from \"{0}\" because the file was not found!", saveFilePath));
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
                            Debug.LogWarning(string.Format("Had an issue loading level data line \"{0}\"!", line));
                        }
                    }
                    catch(NullReferenceException)
                    {
                        Debug.LogWarning(string.Format("Could not load level with name \"{0}\" because it could not be found! Save file: \"{1}\"", lineData[0], saveFilePath));
                    }
                }
            }

            // Closes the file reader
            fileReader.Close();

            // Logs a success
            Debug.Log("Successfully loaded the game! File: " + saveFilePath);
        }


        // Managing Level Saves
        public static string LoadLevel(string levelName, string saveName)
        {
            // Gets a path to the file
            string savePath = Path.Combine(GetLevelFolderPath(levelName), saveName + ".txt");

            // Logs error and exits if it can't find the file
            if(!File.Exists(savePath))
            {
                Debug.LogError(string.Format("Could not load save \"{0}\" because the file does not exist!", savePath));
                return null;
            }

            // Otherwise, reads it in its entirety and returns the text.
            else
            {
                StreamReader fileReader = new StreamReader(savePath);
                string fileContent = fileReader.ReadToEnd();
                fileReader.Close();
                return fileContent;
            }
        }

        public static void DeleteLevelSave(string levelName, string saveName)
        {
            // Gets save name, creates if nonexistent
            string savePath = Path.Combine(GetLevelFolderPath(levelName), saveName + ".txt");

            // Attempts deleting it
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                
            }

            else
            {
                Debug.Log(string.Format("Could not find save {0} in order to delete it!", savePath));
            }
        }

        public static void CreateNewLevelSave(string levelName, string saveName, string[] blockData)
        {
            // Gets save folder, creates if nonexistent
            string saveFolderpath = GetLevelFolderPath(levelName);

            if (!Directory.Exists(saveFolderpath))
            {
                Directory.CreateDirectory(saveFolderpath);
            }

            // Uses a StreamWriter to write to the file
            StreamWriter fileWriter = new StreamWriter(Path.Combine(GlobalData.GetLevelFolderPath(levelName), saveName + ".txt"));
            foreach(string data in blockData)
            {
                fileWriter.WriteLine(data);
            }
            fileWriter.Close();
        }

        public static string GetLevelFolderPath(string levelName)
        {
            string saveFolderPath = Application.persistentDataPath + "\\" + levelName + "\\";
            return saveFolderPath;
        }
    }
}