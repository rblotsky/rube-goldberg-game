using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RubeGoldbergGame
{
    public class MainMenuManager : MonoBehaviour
    {
        // FUNCTIONS //
        // UI Events
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }

    }
}