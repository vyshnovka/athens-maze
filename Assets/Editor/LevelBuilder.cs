using UnityEditor;
using UnityEngine;

public class LevelBuilder : EditorWindow
{
    private Transform player;
    private Transform enemy;
    private Transform exit;
    private Transform tilesTransform;
    private GameObject tilePrefab;
    private LevelScriptableObject levelData;

    [MenuItem("Window/Level Builder")]
    public static void ShowWindow()
    {
        GetWindow<LevelBuilder>("Level Builder");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Gameplay Elements & Entities", EditorStyles.boldLabel);
        player = EditorGUILayout.ObjectField("Player", player, typeof(Transform), true) as Transform;
        enemy = EditorGUILayout.ObjectField("Enemy", enemy, typeof(Transform), true) as Transform;
        exit = EditorGUILayout.ObjectField("Exit", exit, typeof(Transform), true) as Transform;
        tilesTransform = EditorGUILayout.ObjectField("Tiles Zone", tilesTransform, typeof(Transform), true) as Transform;

        EditorGUILayout.LabelField("Objects to spawn", EditorStyles.boldLabel);
        tilePrefab = EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false) as GameObject;

        EditorGUILayout.LabelField("Data to save", EditorStyles.boldLabel);
        levelData = EditorGUILayout.ObjectField("Level Data", levelData, typeof(LevelScriptableObject), false) as LevelScriptableObject;

        if (GUILayout.Button("Draw Level Frame"))
        {
            DrawBorders();
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save Entities"))
        {
            SaveEntities();
        }

        if (GUILayout.Button("Save Tiles"))
        {
            FillLevelObstacles();
        }

        GUILayout.EndHorizontal();
    }

    private void SaveEntities()
    {
        levelData.PlayerPosition = player.position;
        levelData.EnemyPosition = enemy.position;
        levelData.ExitPosition = exit.position;
    }

    private void FillLevelObstacles()
    {
        if (IsLevelDataNull())
        {
            return;
        }

        //Clear existing grid.
        levelData.Grid.Clear();

        //Define the size of the borders.
        int borderSize = (levelData.Width - 2) / 2;

        foreach (Transform tile in tilesTransform)
        {
            Vector2 position = tile.position;

            // heck if the tile is within the borders to avoid frame duplication.
            if (position.x > -borderSize && position.x < borderSize && position.y > -borderSize && position.y < borderSize)
            {
                //Create a new Tile instance and save it.
                Tile newTile = new(tile.position, tile.rotation);
                levelData.Grid.Add(newTile);
            }
        }

        //Prevent scriptable object from resetting due to changes made by script.
        EditorUtility.SetDirty(levelData);
    }

    private void DrawBorders()
    {
        if (IsLevelDataNull())
        {
            return;
        }

        //Remove all existing tiles.
        for (int i = tilesTransform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(tilesTransform.GetChild(i).gameObject);
        }

        //Calculate the half-width and half-height of the level area.
        float halfWidth = levelData.Width / 2f;
        float halfHeight = levelData.Height / 2f;

        //Instantiate vertical walls.
        for (float i = -halfWidth + 0.5f; i <= halfWidth - 0.5f; i++)
        {
            Instantiate(tilePrefab, new Vector2(i, halfHeight), Quaternion.Euler(0, 0, 90), tilesTransform);
            Instantiate(tilePrefab, new Vector2(i, -halfHeight), Quaternion.Euler(0, 0, 90), tilesTransform);
        }

        //Instantiate horizontal walls.
        for (float i = -halfHeight + 0.5f; i <= halfHeight - 0.5f; i++)
        {
            Instantiate(tilePrefab, new Vector2(-halfWidth, i), Quaternion.identity, tilesTransform);
            Instantiate(tilePrefab, new Vector2(halfWidth, i), Quaternion.identity, tilesTransform);
        }
    }

    private bool IsLevelDataNull()
    {
        if (levelData != null)
        {
            return false;
        }

        Debug.LogError("Please assign a valid Scriptable Object of type LevelScriptableObject.");
        return true;
    }
}
