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
    protected bool CheckForWall(Vector2 direction)
    {
        Vector2 desiredPosition = (Vector2)transform.position + direction;

        float offset = 0.5f;

        if (direction == Vector2.left || direction == Vector2.down) 
        {
            offset *= -1;
        }

        if (direction == Vector2.left || direction == Vector2.right)
        {
            return IsWallFromSide(desiredPosition, offset, true);
        }
        else
        {
            return IsWallFromSide(desiredPosition, offset, false);
        }
    }

    //checking if there is a wall depending on isSide
    private bool IsWallFromSide(Vector2 position, float offset, bool isSide)
    {
        Vector2 direction = new Vector2(0, 0);

        if (isSide)
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
                if (!GameManager.instance.playerCanMove)
                {
                    PushCurrentPositionInStack();
                }
                return true;
            }
        }

        return false;
    }

    public void Move(Vector2 direction)
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
