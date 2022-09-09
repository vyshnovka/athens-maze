using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    void Update()
    {
        //return if enemy is moving to prevent excessive moves
        if (!GameManager.instance.playerCanMove)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!CheckForWall(Vector2.up))
            {
                Move(Vector2.up);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!CheckForWall(Vector2.down))
            {
                Move(Vector2.down);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!CheckForWall(Vector2.left))
            {
                Move(Vector2.left);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!CheckForWall(Vector2.right))
            {
                Move(Vector2.right);
            }
        }
    }

    protected override void NextTurn()
    {
        GameManager.instance.ExchangeTurns();

        //enemy moving only if player did not win
        if (!GameManager.instance.IsWin())
        {
            GameManager.instance.NonPlayerTurn();
        }
    }
}
