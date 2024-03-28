using UnityEngine;

public class PlayerController : EntityController
{
    void Update()
    {
        //Return if enemy is moving to prevent excessive moves.
        if (!GameManager.Instance.PlayerCanMove)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!IsMoveBlocked(Vector2.up))
            {
                Move(Vector2.up);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!IsMoveBlocked(Vector2.down))
            {
                Move(Vector2.down);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!IsMoveBlocked(Vector2.left))
            {
                Move(Vector2.left);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!IsMoveBlocked(Vector2.right))
            {
                Move(Vector2.right);
            }
        }
    }

    protected override void NextTurn()
    {
        GameManager.Instance.ExchangeTurns();

        //Enemy should move only if player did not win yet.
        if (!GameManager.Instance.HasPlayerWon())
        {
            GameManager.Instance.NonPlayerTurn();
        }
    }
}
