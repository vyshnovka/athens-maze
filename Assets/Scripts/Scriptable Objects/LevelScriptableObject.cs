using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Object/Level")]
public class LevelScriptableObject : ScriptableObject
{
    [Header("Level frame size")]
    public int Width;
    public int Height;

    [Header("Essential elements' positions")]
    public Vector2 PlayerPosition;
    public Vector2 EnemyPosition;
    public Vector2 ExitPosition;

    [Header("Inside walls")]
    public List<Tile> Grid;

    [Header("Tip/info in the corner")]
    public string Note;
}
