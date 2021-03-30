using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public enum GameState
    {
        Normal,
        Shooting,
        Poping,
        Balancing,
        Changing,
        Victory,
        Failure
    }

    public GameState state;
    public List<GameObject> ballsInLine;

    Tube tube;
    Changer changer;

    private void Awake()
    {
        tube = transform.GetChild(0).GetComponent<Tube>();
        changer = transform.GetChild(1).GetChild(1).GetComponent<Changer>();

        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 0);
            LoadLevel(0);
        }
        else
        {
            LoadLevel(PlayerPrefs.GetInt("level"));
        }
    }
    private void Update()
    {
        if (state == GameState.Normal && GameObject.FindGameObjectWithTag("BallsInTube") == null)
        {
            state = GameState.Victory;
        }
        else if (state == GameState.Normal && changer.ballList.Count == 0 && GameObject.FindGameObjectWithTag("Player") == null)
        {
            state = GameState.Failure;
        }
    }

    void LoadLevel(int level)
    {
        PlayerPrefs.SetInt("level", level);
        string levelCode = Levels.levels[level];
        string[] decrpytCodes = levelCode.Split('x');

        tube.SpawnCode = decrpytCodes[0];
        changer.SpawnCode = decrpytCodes[1];
        
    }

}
