using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;

public class TileManager : EditorWindow
{
    [MenuItem("Window/Tile Manager")]
    public static void ShowWindow()
    {
        GetWindow<TileManager>("Grid");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Fill"))
        {
            Fill();
        }
    }

    private void Fill()
    {
        LevelConstructor constructor = FindObjectOfType<LevelConstructor>();

        //filling tile list with tile positions from the level
        foreach (Transform tile in constructor.tiles)
        {
            constructor.levels[0].grid.Add(new Tile(tile.position, tile.rotation));
        }

        //to prevent scriptable object from reseting due to changes made by script
        EditorUtility.SetDirty(constructor.levels[0]);
    }
}
