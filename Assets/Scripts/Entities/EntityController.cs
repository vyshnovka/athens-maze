using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    protected Stack<Vector2> passedPositions = new Stack<Vector2>();

    protected Vector2 currentPosition;

    protected List<Transform> tiles = new List<Transform>();

    void Start()
    {
        LevelConstructor level = FindObjectOfType<LevelConstructor>();

        foreach (Transform tile in level.tiles)
        {
            tiles.Add(tile);
        }

        currentPosition = transform.position;
    }

    //checking for the move availability
    protected void CheckForWall(Vector2 direction)
    {
        Vector2 desiredPosition = (Vector2)transform.position + direction;

        if (direction == Vector2.right && IsWallFromSide(desiredPosition, 0.5f))
        {
            return;
        }
        else if (direction == Vector2.left && IsWallFromSide(desiredPosition, -0.5f))
        {
            return;
        } 
        else if (direction == Vector2.up && IsWallInLine(desiredPosition, 0.5f))
        {
            return;
        }
        else if (direction == Vector2.down && IsWallInLine(desiredPosition, -0.5f))
        {
            return;
        }

        Move(direction);
    }

    //checking if there is a wall on X asix
    private bool IsWallFromSide(Vector2 position, float offset)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if ((Vector2)tiles[i].position == new Vector2(position.x - offset, position.y))
            {
                if (!GameManager.instance.playerCanMove)
                {
                    PushCurrentPositionInStack();
                }
                return true;
            }
        }

        return false;
    }

    //checking if there is a wall on Y asix
    private bool IsWallInLine(Vector2 position, float offset)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if ((Vector2)tiles[i].position == new Vector2(position.x, position.y - offset))
            {
                if (!GameManager.instance.playerCanMove)
                {
                    PushCurrentPositionInStack();
                }
                return true;
            }
        }

        return false;
    }

    private void Move(Vector2 direction)
    {
        PushCurrentPositionInStack();

        currentPosition += direction;
        transform.position = currentPosition;

        NextTurn();
    }

    public void UndoMove()
    {
        if (passedPositions.Count == 0)
        {
            return;
        }

        currentPosition = passedPositions.Pop();
        transform.position = currentPosition;
    }

    public void PushCurrentPositionInStack()
    {
        passedPositions.Push(currentPosition);
    }

    protected virtual void NextTurn() { }

}
