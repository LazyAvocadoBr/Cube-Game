using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBaseInput : MonoBehaviour
{
    public static Transform cameraDirection;

    private void Awake()
    {
        cameraDirection = transform;
    }
}
