using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelConstructor : MonoBehaviour
{
    [Header("Gameplay Elements & Entities")]
    [SerializeField]
    private Transform tiles;
    [SerializeField]
    private Note note;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private EnemyController enemy;
    [SerializeField]
    private Transform exit;

    [Header("Objects to spawn")]
    [SerializeField]
    private GameObject tilePrefab;

    [Header("Level Info")]
    [SerializeField]
    private List<LevelScriptableObject> levels;

    private LevelScriptableObject currentLevelData;
    private int currentLevelIndex;

    public Transform Tiles { get => tiles; set => tiles = value; }

    void Awake()
    {
        //Save level index.
        currentLevelIndex = PlayerPrefs.GetInt("Level", 0);

        //Make the game infinite.
        if (currentLevelIndex > levels.Count - 1)
        {
            currentLevelIndex = 0;
        }

        currentLevelData = levels[currentLevelIndex];

        note.ShowLevelInfo(currentLevelData.Note);

        DrawBorders();
        DrawObstacles();

        //Set entities' positions.
        player.transform.position = currentLevelData.PlayerPosition;
        enemy.transform.position = currentLevelData.EnemyPosition;

        //Set exit position.
        exit.transform.position = currentLevelData.ExitPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Next level becomes available only after passing the previous one.
            if (GameManager.Instance.HasPlayerWon())
            {
                PlayerPrefs.SetInt("Level", currentLevelIndex + 1);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    #region Level Walls Builder
    /// <summary>Draw level frame depending on the specified size.</summary>
    private void DrawBorders()
    {
        //Calculate the half-width and half-height of the level area.
        float halfWidth = currentLevelData.Width / 2f;
        float halfHeight = currentLevelData.Height / 2f;

        //Instantiate vertical walls.
        for (float i = -halfWidth + 0.5f; i <= halfWidth - 0.5f; i++)
        {
            Instantiate(tilePrefab, new Vector2(i, halfHeight), Quaternion.Euler(0, 0, 90), Tiles);
            Instantiate(tilePrefab, new Vector2(i, -halfHeight), Quaternion.Euler(0, 0, 90), Tiles);
        }

        //Instantiate horizontal walls.
        for (float i = -halfHeight + 0.5f; i <= halfHeight - 0.5f; i++)
        {
            Instantiate(tilePrefab, new Vector2(-halfWidth, i), Quaternion.identity, Tiles);
            Instantiate(tilePrefab, new Vector2(halfWidth, i), Quaternion.identity, Tiles);
        }
    }

    /// <summary>Draw inner walls for the current level.</summary>
    private void DrawObstacles()
    {
        foreach (var tile in currentLevelData.Grid)
        {
            Instantiate(tilePrefab, tile.Position, tile.Rotation, Tiles);
        }
    }
    #endregion
}
