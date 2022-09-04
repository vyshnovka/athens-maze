using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //again BAD practive to insert UI in functionalities, but it is now not that important
    [SerializeField]
    private GameObject winUI;
    [SerializeField]
    private GameObject looseUI;

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
            //double function calling since the enemy if moving twice
            //for (int i = 0; i < 2; i++)
            //{
            //    StartCoroutine(Utility.TimedEvent(() =>
            //    {
            //        enemy.FindPath();
            //        IsDead();
            //    }, 0.5f));
            //}


            //could be done in a single coroutine with two yield returns (or in a loop), but i just wanted to use that script
            StartCoroutine(Utility.TimedEvent(() =>
            {
                enemy.FindPath();
                IsDead();
            }, 0.5f));

            StartCoroutine(Utility.TimedEvent(() =>
            {
                enemy.FindPath();
                IsDead();
            }, 1f));

            StartCoroutine(WaitForEnemy());
        }
    }

    //to prevent moving while enemy is still taking steps
    private IEnumerator WaitForEnemy()
    {
        yield return new WaitForSeconds(1.1f);

        ExchangeTurns();
    }

    private bool IsDead()
    {
        if (player.transform.position == enemy.transform.position)
        {
            ExchangeTurns();

            looseUI.SetActive(true);

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
            winUI.SetActive(true);

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
