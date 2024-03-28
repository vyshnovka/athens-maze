using System;
using UnityEngine;

[Serializable]
public class Tile
{
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Quaternion rotation;

    public Vector3 Position { get => position; set => position = value; }
    public Quaternion Rotation { get => rotation; set => rotation = value; }

    public Tile(Vector3 pos, Quaternion rot)
    {
        Position = pos;
        Rotation = rot;
    }
}
