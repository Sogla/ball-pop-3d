using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public Vector3[] path;
    public List<Vector3> fixedPoints;

    readonly float radius = 6f;
    readonly float bRadius = 0.40f;
    private float Theta;

    public int FixedPoint;

    GameObject manager;
    LevelManager managerScript;

    [Header("For Ball Creator")]

    [SerializeField] GameObject[] ballTypes;

    public string SpawnCode;

    void Start()
    {
        CreateFixedPoints();
        path = new Vector3[fixedPoints.Count];
        path = fixedPoints.ToArray();
        manager = GameObject.FindGameObjectWithTag("GameController");
        managerScript = manager.GetComponent<LevelManager>();
        FixedPoint = path.Length / 2;
        BallCreator();
    }

    void CreateFixedPoints()
    {
        float tempCalc = ((2*(radius * radius) - (bRadius * bRadius)) / (radius * radius * 2));
        Theta = Mathf.Acos(tempCalc);
        int maxSize = (int)(Mathf.PI / Theta);
        float minAngle = 0;
        for (float i = Mathf.PI/2 ; i > 0; i -= Theta)
        {
            minAngle = i;
        }
        Theta *= 180 / Mathf.PI;
        minAngle = minAngle * 180 / Mathf.PI;

        for (int i = 0; i < maxSize; i++)
        {
            float x = radius * Mathf.Cos(minAngle * (Mathf.PI / 180));
            float z = radius * Mathf.Sin(minAngle * (Mathf.PI / 180));
            fixedPoints.Add(new Vector3(x, 0, transform.position.z + z));
            minAngle += Theta;
        }

    }

    public void BalanceBalls()
    {
        int size = managerScript.ballsInLine.Count;
        if (size % 2 == 0)
        {
            int j = (size / 2);
            for (int i = 0; i < j; i++)
            {
                Ball a = managerScript.ballsInLine[j + i].GetComponent<Ball>();
                Ball b = managerScript.ballsInLine[j - i - 1].GetComponent<Ball>();
                a.ballPosition = j + i;
                b.ballPosition = j - i - 1;
                a.CreateAPathAndFollow(FixedPoint - 1 - (i * 2));
                a.ballPositionOnTube = FixedPoint - 1 - (i * 2);
                b.CreateAPathAndFollow(FixedPoint + 1 + (i * 2));
                b.ballPositionOnTube = FixedPoint + 1 + (i * 2);
            }
        }
        else
        {
            int j = (size / 2);
            Ball midBall = managerScript.ballsInLine[j].GetComponent<Ball>();
            midBall.ballPosition = j;
            midBall.CreateAPathAndFollow(FixedPoint);
            midBall.ballPositionOnTube = FixedPoint;
            for (int i = 0; i < j; i++)
            {
                Ball a = managerScript.ballsInLine[j + 1 + i].GetComponent<Ball>();
                Ball b = managerScript.ballsInLine[j - 1 - i].GetComponent<Ball>();
                a.ballPosition = j + 1 + i;
                b.ballPosition = j - 1 - i;
                a.CreateAPathAndFollow(FixedPoint - ((i + 1) * 2));
                a.ballPositionOnTube = FixedPoint - ((i + 1) * 2);
                b.CreateAPathAndFollow(FixedPoint + ((i + 1) * 2));
                b.ballPositionOnTube = FixedPoint + ((i + 1) * 2);
            }
        }
        Invoke("IsPoping", Ball.BallSpeedOnTube + 0.2f); //Süre
    }

    void IsPoping()
    {
        int j = 1;
        Ball.BallColor tempColor = Ball.BallColor.NULL;
        for (int i = 0; i < managerScript.ballsInLine.Count; i++)
        {
            Ball a = managerScript.ballsInLine[i].GetComponent<Ball>();
            if (tempColor == a.ballColor)
            {
                j++;
                if ((managerScript.ballsInLine.Count - 1 == i && j >= 3))
                {
                    managerScript.state = LevelManager.GameState.Poping;
                    Pop(i - j + 1, i);
                    return;
                }
            }
            else if ((tempColor != a.ballColor && j >= 3))
            {
                managerScript.state = LevelManager.GameState.Poping;
                Pop(i - j, i - 1);
                return;
            }
            else
            {
                tempColor = a.ballColor;
                j = 1;
            }
        }
        managerScript.state = LevelManager.GameState.Normal;
    }

    void Pop(int startIndex, int endIndex)
    {
        GameObject[] temps = new GameObject[(endIndex - startIndex) + 1];
        int j = 0;
        for (int i = startIndex; i <= endIndex ; i++)
        {
            temps[j] = managerScript.ballsInLine[i];
            Destroy(temps[j]);
            j++;
        }
        managerScript.ballsInLine.RemoveRange(startIndex, temps.Length);
        BalanceBalls();
    }

    public void BallCreator()
    {
        managerScript.state = LevelManager.GameState.Balancing;
        SpawnCode.ToLower();
        for (int i = 0; i < SpawnCode.Length; i++)
        {
            switch (SpawnCode[i])
            {
                case 'b':
                    SpawnBall(0);
                    break;
                case 'g':
                    SpawnBall(1);
                    break;
                case 'o':
                    SpawnBall(2);
                    break;
                case 'p':
                    SpawnBall(3);
                    break;
                case 'r':
                    SpawnBall(4);
                    break;
                case 'y':
                    SpawnBall(5);
                    break;
                case 'z':
                    SpawnBall(6);
                    break;
                default:
                    break;
            }
        }
        Invoke("BalanceBalls", 0.1f);
    }

    void SpawnBall(int type)
    {
        GameObject temp;
        temp = Instantiate(ballTypes[type], transform.position + new Vector3(0, 0, transform.position.z*2), transform.rotation, transform);
        temp.GetComponent<Ball>().ballPositionOnTube = FixedPoint;
        temp.tag = "BallsInTube";
        temp.layer = 9;
        temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        managerScript.ballsInLine.Add(temp);
    }
}
