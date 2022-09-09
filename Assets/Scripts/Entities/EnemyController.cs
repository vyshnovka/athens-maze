using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyController : EntityController
{
    public Vector2 FindPath()
    {
        var player = GameManager.instance.player.transform.position;

        if (player.x < transform.position.x && !CheckForWall(Vector2.left))
        {
            //player is on the left
            return Vector2.left;
        }

        if (player.x > transform.position.x && !CheckForWall(Vector2.right))
        {
            //player is on the right
            return Vector2.right;
        }

        //player is in the same column
        if (player.y > transform.position.y && !CheckForWall(Vector2.up))
        {
            //player is above
            return Vector2.up;
        }
        
        if (player.y < transform.position.y && !CheckForWall(Vector2.down))
        {
            //player is below
            return Vector2.down;
        }

        return Vector2.zero;
    }
}
