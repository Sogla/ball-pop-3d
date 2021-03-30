using UnityEngine;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening;

public class Bezier : MonoBehaviour
{
    public GameObject p0, p1, p2;
    public GameObject ball;
    private int numPoints = 50;
    private Vector3[] positions = new Vector3[50];
    bool denemeK = true;
    public GameObject[] trajectoryDots;

    [SerializeField] float Speed = 10;
    RaycastHit hit;

    GameObject trajectory;

    Vector3 startPos, endPos, dir;
    GameObject changerObject;
    Changer changer;
    GameObject manager;
    LevelManager managerScript;

    bool isShootingStart;






    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");
        managerScript = manager.GetComponent<LevelManager>();

        changerObject = transform.parent.GetChild(1).gameObject;
        changer = changerObject.GetComponent<Changer>();

        trajectory = transform.GetChild(0).gameObject;

    }
    void Update()
    {

        if (managerScript.state == LevelManager.GameState.Normal && Camera.main.ScreenToViewportPoint(Input.mousePosition).y < 0.9f)
        {
            InputControl();
        }

    }

    void InputControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.WorldToScreenPoint(transform.position);
            isShootingStart = true;
        }
        if (Input.GetMouseButton(0) && isShootingStart)
        {
            endPos = Input.mousePosition;
            dir = (endPos - startPos);
            dir.z = dir.y;
            dir.y = 0;
            DrawQuadradicCurve();
            p1.transform.position = new Vector3(dir.x / 10, p1.transform.position.y,p1.transform.position.z);
            p2.transform.position = new Vector3(-dir.x / 50, p2.transform.position.y, p2.transform.position.z);
            
            
        }
    }

    void DrawQuadradicCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateCubicBezierPoint(t, p0.transform.position, p1.transform.position, p2.transform.position);
        }
        int j = 1;
        foreach (var item in trajectoryDots)
        {
            item.gameObject.transform.position = positions[j * 5];
            j += 1;
        }
        
    }

    void Shoot()
    {
        ball.transform.DOPath(positions, 3, PathType.Linear, PathMode.Full3D);
        denemeK = false;
    }
    


    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}