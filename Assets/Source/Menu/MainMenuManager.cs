using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RubeGoldbergGame
{
    public class MainMenuManager : MonoBehaviour
    {
        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Sets up the game immediately
            GlobalData.SetupGame();
        }


        // UI Events
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }

    }
}