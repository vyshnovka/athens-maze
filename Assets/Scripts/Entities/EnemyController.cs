using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    public void FindPath()
    {
        var player = GameManager.instance.player.transform.position;

        if (player.x < transform.position.x)
        {
            //player is on the left
            CheckForWall(Vector2.left);
        }
        else if (player.x > transform.position.x)
        {
            //player is on the right
            CheckForWall(Vector2.right);
        }
        else
        {
            //player is in the same column
            if (player.y > transform.position.y)
            {
                //player is above
                CheckForWall(Vector2.up);
            }
            else if (player.y < transform.position.y)
            {
                //player is below
                CheckForWall(Vector2.down);
            }
        }
    }
}
