using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class EditorMenuItems
{
    // FUNCTIONS //
    [MenuItem("Tools/Set Sprites to 80 Pixels Per Unit")]
    public void UpdateSpriteSettings()
    {
        // Gets all sprites
        string[] files = AssetDatabase.FindAssets("t:Sprite");

        foreach(string filePath in files)
        {
        }


    }
}
