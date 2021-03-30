using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        int aspect = Screen.height * 100 / Screen.width;
        Camera.main.fieldOfView = (((aspect - 216) * 25) / 91) + 75;
    }

}
