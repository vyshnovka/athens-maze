using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelConstructor : MonoBehaviour
{
    public Transform tiles;

    [SerializeField]
    public List<LevelScriptableObject> levels;
    [NonSerialized]
    public LevelScriptableObject level;

    private int currentLevel;

    //bad practice to mess up UI with actions, but yeah
    [SerializeField]
    private TextMeshProUGUI note;

    void Awake()
    {
        //tiny save system
        currentLevel = PlayerPrefs.GetInt("Level", 0);

        //checking if we are not out of boundaries of a list
        //and making the game infinite
        if (currentLevel > levels.Count - 1)
        {
            currentLevel = 0;
        }

        level = levels[currentLevel];

        note.text += level.notification;

        //drawing borders
        for (float i = 0, offset = 0.5f; i < level.width; i++, offset++)
        {
            Instantiate(level.tile, new Vector2(-level.width / 2 + offset, level.height / 2), Quaternion.Euler(0, 0, 90), tiles);
            Instantiate(level.tile, new Vector2(-level.width / 2 + offset, -level.height / 2), Quaternion.Euler(0, 0, 90), tiles);
        }
        for (float i = 0, offset = 0.5f; i < level.height; i++, offset++)
        {
            Instantiate(level.tile, new Vector2(-level.width / 2, level.height / 2 - offset), Quaternion.identity, tiles);
            Instantiate(level.tile, new Vector2(level.width / 2, level.height / 2 - offset), Quaternion.identity, tiles);
        }

        //spawning tiles for the level
        foreach (var tile in level.grid)
        {
            Instantiate(level.tile, tile.position, tile.rotation, tiles);
        }

        //setting player's and enemy's position
        GameManager.instance.player.transform.position = level.playerPos;
        GameManager.instance.enemy.transform.position = level.enemyPos;

        //setting the exit
        GameManager.instance.exit.transform.position = level.exitPos;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //next level becomes available only after passing the previous one
            if (GameManager.instance.IsWin())
            {
                PlayerPrefs.SetInt("Level", currentLevel + 1);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
