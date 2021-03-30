using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SelectedBall : MonoBehaviour
{
    Vector3 startPos, endPos, forceAtPlayer;
    Rigidbody rb;
    LineRenderer lr;

    [SerializeField] float deadZone = 1f;
    [SerializeField] float force = 10;
    [SerializeField] List<GameObject> balls;
    public float sensivity = 0.4f;
    public float speed = 5f;
    public float forceFactor = 5f;
    float rotateSpeedX = 5f;
    float rotateSpeedY = 5f;
    int i;

    bool isRotating;
    bool kontrol = true;
    GameObject manager;
    LevelManager managerScript;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");
        managerScript = manager.GetComponent<LevelManager>();
    }

    void Update()
    {
       
    }


    void Rotate()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y - transform.position.y;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        float angle = Mathf.Atan2(transform.position.z - mouseWorldPos.z, (transform.position.x - mouseWorldPos.x) * rotateSpeedX) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle + 90, 0), 20 * Time.deltaTime);
    }
    void InputControl()
    {

        if (Input.GetMouseButtonDown(0))
        {

            startPos = transform.GetChild(0).transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 10;
            Rotate();
        }

        if (Input.GetMouseButtonUp(0))
        {
            managerScript.state = LevelManager.GameState.Shooting;
            rb = transform.GetChild(0).transform.GetComponentInChildren<Rigidbody>();
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
            transform.GetChild(0).DetachChildren();
            transform.DORotate(new Vector3(0, 0, 0), 0.1f);
            balls[i].transform.DOMove(startPos, 1f);
            balls[i].transform.SetParent(gameObject.transform.GetChild(0));
            balls[i].transform.rotation = transform.rotation;
            i++;

        }
    }
}
