using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    #region Values
    public enum BallColor
    {
        Purple,
        Blue,
        Green,
        Yellow,
        Orange,
        Red,
        NULL
    }
    #endregion

    public PathType pathsystem = PathType.Linear;
    public List<Vector3> path;
    public Tween t;

    public BallColor ballColor;
    public int ballPosition = 255;
    public int ballPositionOnTube = 255;

    public static float BallSpeedOnTube = 0.3f; //Süre

    GameObject manager;
    LevelManager managerScript;

    GameObject ballLauncher;
    BallLauncher bl;

    GameObject tubeObject;
    Tube tube;
    Rigidbody rg;

    void Start()
    {
        ballLauncher = GameObject.FindGameObjectWithTag("Launcher");
        bl = ballLauncher.GetComponentInChildren<BallLauncher>();
        manager = GameObject.FindGameObjectWithTag("GameController");
        managerScript = manager.GetComponent<LevelManager>();
        tubeObject = GameObject.FindGameObjectWithTag("Tube");
        tube = tubeObject.GetComponent<Tube>();
        rg = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log(rg.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            //Debug.Log(bl.velocity);
            //Debug.Log(Vector3.Reflect(bl.velocity, collision.contacts[0].normal));
            //Debug.Log(rg.velocity);
            Vector3 velocity = Vector3.Reflect(bl.velocity, collision.contacts[0].normal);
            rg.AddForce(velocity);
            //Debug.Log(rg.velocity);
        }
        else if (gameObject.CompareTag("Player") && managerScript.state == LevelManager.GameState.Shooting)
        {
            rg.velocity = Vector3.zero;
            rg.constraints = RigidbodyConstraints.FreezeAll;
            gameObject.layer = 9;
            gameObject.tag = "BallsInTube";
            FindNearestPoint();

            if (collision.gameObject.CompareTag("BallsInTube"))
            {

                Ball otherBall = collision.gameObject.GetComponent<Ball>();

                if (collision.transform.position.x > transform.position.x)
                {
                    managerScript.ballsInLine.Insert(otherBall.ballPosition, gameObject);
                    ballPosition = otherBall.ballPosition;
                }
                else
                {
                    managerScript.ballsInLine.Insert(otherBall.ballPosition + 1, gameObject);
                    ballPosition = otherBall.ballPosition + 1;
                }
                gameObject.transform.SetParent(collision.gameObject.transform.parent);
                FindNearestPoint();
            }
            if (collision.gameObject.CompareTag("Tube"))
            {

                if (managerScript.ballsInLine.Count == 0)
                {
                    managerScript.ballsInLine.Add(gameObject);
                    ballPosition = 0;
                }
                else if (managerScript.ballsInLine[managerScript.ballsInLine.Count - 1].transform.position.x < transform.position.x)
                {
                    managerScript.ballsInLine.Add(gameObject);
                    ballPosition = managerScript.ballsInLine.Count;
                }
                else if (managerScript.ballsInLine[0].transform.position.x > transform.position.x)
                {
                    managerScript.ballsInLine.Insert(0,gameObject);
                    ballPosition = 0;
                }
                gameObject.transform.SetParent(collision.gameObject.transform);
               
            }
            managerScript.state = LevelManager.GameState.Balancing;
            tube.BalanceBalls();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
    public void CreateAPathAndFollow(int targetPoint)
    {
        if (targetPoint > ballPositionOnTube)
        {
            for (int i = 1; i <= targetPoint - ballPositionOnTube; i++)
            {
                path.Add(tube.path[ballPositionOnTube + i]);
            }
        }
        else if (targetPoint == ballPositionOnTube)
        {
            gameObject.transform.DOMove(tube.path[targetPoint], BallSpeedOnTube);
            return;
        }
        else
        {
            for (int i = 1; i <= ballPositionOnTube - targetPoint; i++)
            {
                path.Add(tube.path[ballPositionOnTube - i]);
            }
        }
        t = gameObject.transform.DOPath(path.ToArray(), BallSpeedOnTube, pathsystem);
        path.Clear();
    }

    public void FindNearestPoint()
    {
        int fixedPoint = tube.FixedPoint;
        Vector3 pos = transform.position;
        ballPositionOnTube = fixedPoint;
        if (pos.x >= 0)
        {
            float min = (pos - tube.path[fixedPoint]).magnitude;
            for (int j = fixedPoint; j >= 0; j--)
            {
                if ((pos - tube.path[j]).magnitude < min)
                {
                    min = (pos - tube.path[j]).magnitude;
                    ballPositionOnTube = j;
                }
            }
        }
        else if( pos.x < 0)
        {
            float min = (pos - tube.path[fixedPoint]).magnitude;
            for (int j = fixedPoint; j < tube.path.Length; j++)
            {
                if ((pos - tube.path[j]).magnitude < min)
                {
                    min = (pos - tube.path[j]).magnitude;
                    ballPositionOnTube = j;
                }
            }
        }
    }
}
