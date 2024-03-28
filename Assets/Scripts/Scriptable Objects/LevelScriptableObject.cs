using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Object/Level")]
public class LevelScriptableObject : ScriptableObject
{
    public int Width;
    public int Height;

    public Vector2 PlayerPosition;
    public Vector2 EnemyPosition;
    public Vector2 ExitPosition;

    public List<Tile> Grid;

    public string Note;
}
