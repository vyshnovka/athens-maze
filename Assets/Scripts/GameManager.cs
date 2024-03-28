using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Entities")]
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private EnemyController enemy;

    [Header("Essential elements")]
    [SerializeField]
    private Transform exit;

    private bool playerCanMove = true;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onWinEvent;
    [SerializeField]
    private UnityEvent onLooseEvent;

    public PlayerController Player { get => player; set => player = value; }
    public bool PlayerCanMove { get => playerCanMove; set => playerCanMove = value; }

    void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (PlayerCanMove) 
            {
                UndoPreviousStep();
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (PlayerCanMove)
            {
                Player.PushCurrentPositionInStack();

                ExchangeTurns();
                NonPlayerTurn();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (PlayerCanMove || IsPlayerDead())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    #region Entity Moves Management
    public void ExchangeTurns() => PlayerCanMove = !PlayerCanMove;

    /// <summary>Prevent player movement while enemy is taking steps.</summary>
    public void NonPlayerTurn()
    {
        if (!PlayerCanMove && !HasPlayerWon())
        {
            StartCoroutine(WaitForEnemy());
        }
    }

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

            CheckForDeathAndDie();
        }

        ExchangeTurns();
    }

    private bool IsPlayerDead() => Player.transform.position == enemy.transform.position;

    private void CheckForDeathAndDie()
    {
        if (IsPlayerDead())
        {
            ExchangeTurns();

            onLooseEvent?.Invoke();
        }
    }

    public bool HasPlayerWon()
    {
        if (IsPlayerDead())
        {
            return false;
        }

        if (Player.transform.position == exit.position)
        {
            onWinEvent?.Invoke();

            return true;
        }

        return false;
    }

    /// <summary>Undo both enemy and player moves from the previous step.</summary>
    private void UndoPreviousStep()
    {
        enemy.UndoMove();
        enemy.UndoMove();
        Player.UndoMove();
    }
    #endregion
}
