using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] float Speed = 10;
    RaycastHit hit;

    GameObject trajectory;

    public Vector3 startPos, endPos, dir, velocity;
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
            dir = dir.normalized;

            Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity);

            if (hit.transform != null && (hit.collider.gameObject.CompareTag("BallsInTube") || hit.collider.gameObject.CompareTag("Tube") || hit.collider.gameObject.CompareTag("Wall")) )
            {
                trajectory.SetActive(true);
                trajectory.transform.rotation = Quaternion.FromToRotation(Vector3.forward, dir);
                for (int i = 0; i < trajectory.transform.childCount; i++)
                {
                    if ((trajectory.transform.GetChild(i).position - transform.position).magnitude > hit.distance)
                    {
                        trajectory.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    else
                    {
                        trajectory.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    
                }
            }
            else
            {
                trajectory.SetActive(false);
            }

        }
        if (isShootingStart != false && hit.transform != null && Input.GetMouseButtonUp(0) && (hit.collider.gameObject.CompareTag("BallsInTube") || hit.collider.gameObject.CompareTag("Tube") || hit.collider.gameObject.CompareTag("Wall")))
        {
            trajectory.SetActive(false);
            Transform child = transform.GetChild(1);
            child.GetComponent<Rigidbody>().AddForce(dir * Speed, ForceMode.Impulse);
            velocity = dir * Speed;
            child.SetParent(null);
            managerScript.state = LevelManager.GameState.Shooting;

            if (changer.ballList.Count != 0)
            {
                changer.ChangeBalls(false);
            }

            isShootingStart = false;
        }
    }

}
