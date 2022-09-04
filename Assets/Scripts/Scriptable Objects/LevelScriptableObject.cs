using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Object/Level")]
public class LevelScriptableObject : ScriptableObject
{
    //height and width must me even for the code to work
    public int width;
    public int height;

    public Vector2 playerPos;
    public Vector2 enemyPos;

    public List<Tile> grid;
    public GameObject tile;

    public Vector2 exitPos;

    public string notification;
}
