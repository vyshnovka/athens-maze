using System;
using UnityEngine;

[Serializable]
public class Tile
{
    public Vector3 position;
    public Quaternion rotation;

    public Tile(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}
