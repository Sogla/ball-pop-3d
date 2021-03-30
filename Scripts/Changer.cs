using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Changer : MonoBehaviour
{
    GameObject selectedBall;
    GameObject manager;
    LevelManager managerScript;

    public List<GameObject> ballList;

    [Header("For Ball Creator")]

    [SerializeField] GameObject[] ballTypes;

    public string SpawnCode;

    // Update is called once per frame
    private void Start()
    {
        selectedBall = transform.parent.GetChild(0).gameObject;
        manager = GameObject.FindGameObjectWithTag("GameController");
        managerScript = manager.GetComponent<LevelManager>();
        BallCreator();
    }

    private void OnMouseDown()
    {
        if (managerScript.state == LevelManager.GameState.Normal)
        {
            ChangeBalls();
        }
    }

    public void ChangeBalls(bool justChanging = true)
    {
        GameObject ball = ballList[0];
        if (justChanging)
        {
            managerScript.state = LevelManager.GameState.Changing;
            GameObject oldBall = selectedBall.transform.GetChild(1).gameObject;

            oldBall.transform.DOMove(ballList[ballList.Count - 1].transform.position , 0.2f); //Süre
            oldBall.transform.SetParent(transform);
            ballList.Add(oldBall);
        }
        for (int i = ballList.Count - 1; i > 0 ; i--)
        {
            ballList[i].transform.DOMove(ballList[i - 1].transform.position, 0.2f); //Süre
        }
        ball.transform.SetParent(selectedBall.transform);
        ball.transform.DOMove(selectedBall.transform.position, 0.2f); //Süre
        ballList.RemoveAt(0);
        if (justChanging)
        {
            Invoke("Completed", 0.2f); //Süre
        }
    }

    void Completed()
    {
        managerScript.state = LevelManager.GameState.Normal;
    }

    public void BallCreator()
    {
        Vector3 position = Vector3.zero;
        SpawnCode.ToLower();
        for (int i = 0; i < SpawnCode.Length; i++)
        {
            GameObject temp;
            switch (SpawnCode[i])
            {
                case 'b':
                    temp = Instantiate(ballTypes[0], transform.position + position, transform.rotation, transform);
                    ballList.Add(temp);
                    position.x += 1;
                    break;
                case 'g':
                    temp = Instantiate(ballTypes[1], transform.position + position, transform.rotation, transform);
                    ballList.Add(temp);
                    position.x += 1;
                    break;
                case 'o':
                    temp = Instantiate(ballTypes[2], transform.position + position, transform.rotation, transform);
                    ballList.Add(temp);
                    position.x += 1;
                    break;
                case 'p':
                    temp = Instantiate(ballTypes[3], transform.position + position, transform.rotation, transform);
                    ballList.Add(temp);
                    position.x += 1;
                    break;
                case 'r':
                    temp = Instantiate(ballTypes[4], transform.position + position, transform.rotation, transform);
                    ballList.Add(temp);
                    position.x += 1;
                    break;
                case 'y':
                    temp = Instantiate(ballTypes[5], transform.position + position, transform.rotation, transform);
                    ballList.Add(temp);
                    position.x += 1;
                    break;
                default:
                    break;
            }
        }
        ChangeBalls(false);
       
    }
}
