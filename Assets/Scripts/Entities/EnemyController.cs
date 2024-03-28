using UnityEngine;

public class EnemyController : EntityController
{
    /// <summary>Find next position to get closet to the player (horizontal move is a priority).</summary>
    public Vector2 FindPath()
    {
        var player = GameManager.Instance.Player.transform.position;

        if (player.x < transform.position.x && !IsMoveBlocked(Vector2.left))
        {
            //Player is on the left.
            return Vector2.left;
        }

        if (player.x > transform.position.x && !IsMoveBlocked(Vector2.right))
        {
            //player is on the right.
            return Vector2.right;
        }

        //Player is in the same column.
        if (player.y > transform.position.y && !IsMoveBlocked(Vector2.up))
        {
            //Player is above.
            return Vector2.up;
        }
        
        if (player.y < transform.position.y && !IsMoveBlocked(Vector2.down))
        {
            //Player is below.
            return Vector2.down;
        }

        return Vector2.zero;
    }
}
