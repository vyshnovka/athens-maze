using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    protected Vector2 currentPosition;
    protected Stack<Vector2> passedPositions = new();

    protected List<Transform> tiles = new();

    void Start()
    {
        LevelConstructor level = FindObjectOfType<LevelConstructor>();

        foreach (Transform tile in level.Tiles)
        {
            tiles.Add(tile);
        }

        currentPosition = transform.position;
    }

    /// <summary>Check for the move availability in a given direction (move can be blocked by a wall).</summary>
    protected bool IsMoveBlocked(Vector2 direction)
    {
        Vector2 desiredPosition = (Vector2)transform.position + direction;

        float offset = 0.5f;

        //Adjust offset when moving towards negative coordinates.
        if (direction == Vector2.left || direction == Vector2.down) 
        {
            offset *= -1;
        }

        if (direction == Vector2.left || direction == Vector2.right)
        {
            return IsWallInDirection(desiredPosition, offset, true);
        }
        else
        {
            return IsWallInDirection(desiredPosition, offset);
        }
    }

    /// <summary>Check if there is a wall in the specified direction of a given position.</summary>
    /// <param name="position">Desired position.</param>
    /// <param name="offset">Specified direction (+ is up/right, - is down/left).</param>
    /// <param name="isHorizontal">If true, the direction is either right of left.</param>
    private bool IsWallInDirection(Vector2 position, float offset, bool isHorizontal = false)
    {
        Vector2 direction;

        if (isHorizontal)
        {
            direction = new Vector2(position.x - offset, position.y);
        }
        else
        {
            direction = new Vector2(position.x, position.y - offset);
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            if ((Vector2)tiles[i].position == direction)
            {
                if (!GameManager.Instance.PlayerCanMove)
                {
                    PushCurrentPositionInStack();
                }

                return true;
            }
        }

        return false;
    }

    /// <summary>Move in a given direction saving previous position to be able to undo it.</summary>
    public void Move(Vector2 direction)
    {
        PushCurrentPositionInStack();

        currentPosition += direction;
        transform.position = currentPosition;

        NextTurn();
    }

    /// <summary>Undo previous move by changing entity's position to the previous one.</summary>
    public void UndoMove()
    {
        if (passedPositions.Count == 0)
        {
            return;
        }

        currentPosition = passedPositions.Pop();
        transform.position = currentPosition;
    }

    public void PushCurrentPositionInStack() => passedPositions.Push(currentPosition);

    protected virtual void NextTurn() { }
}
