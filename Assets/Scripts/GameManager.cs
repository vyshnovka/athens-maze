using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player;
    public EnemyController enemy;
    public Transform exit;

    [NonSerialized]
    public bool playerCanMove = true;
    [NonSerialized]
    public bool enemyCanMove = false;

    [SerializeField]
    private UnityEvent onWinEvent;
    [SerializeField]
    private UnityEvent onLooseEvent;

    void Awake()
    {
        if (instance)
        {
            Destroy(instance);
        }

        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UndoPreviousStep();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            player.PushCurrentPositionInStack();

            ExchangeTurns();
            NonPlayerTurn();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ExchangeTurns()
    {
        playerCanMove = !playerCanMove;
    }

    public void NonPlayerTurn()
    {
        if (!playerCanMove && !IsWin())
        {
            StartCoroutine(WaitForEnemy());
        }
    }

    //to prevent moving while enemy is still taking steps
    private IEnumerator WaitForEnemy()
    {
        yield return new WaitForSeconds(0f);

        for (int i = 0; i < 2; i++)
        {
            var direction = enemy.FindPath();

            if (direction != Vector2.zero)
            {
                yield return new WaitForSeconds(0.5f);
                enemy.Move(direction);
            }

            IsDead();
        }

        ExchangeTurns();
    }

    private bool IsDead()
    {
        if (player.transform.position == enemy.transform.position)
        {
            ExchangeTurns();

            onLooseEvent?.Invoke();

            return true;
        }

        return false;
    }

    public bool IsWin()
    {
        if (IsDead())
        {
            return false;
        }

        if (player.transform.position == exit.position)
        {
            onWinEvent?.Invoke();

            return true;
        }

        return false;
    }

    //undo enemy's and player's moves from the previous step
    private void UndoPreviousStep()
    {
        enemy.UndoMove();
        enemy.UndoMove();
        player.UndoMove();
    }
}
