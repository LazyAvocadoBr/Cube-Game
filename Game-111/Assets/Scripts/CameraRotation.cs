using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public bool bRotate;
    private Transform playerTransform;
    [SerializeField]
    private Vector3 targetRotation;
    private Vector3 oPosition, oRotation;
    private void Update()
    {
        if(bRotate)
        {
            bRotate = false;
            StartCoroutine(nameof(Rotate));
        }
    }
    private void OnEnable()
    {
        transform.parent = null;
        SceneLoader.UpdatePlayerTransform += UpdatePlayerTransform;
        CameraOffsetGA.SetCameraOffset += OffsetCamera;
        CameraOffsetGA.PreviousCameraOffset += PreviousOffset;

        transform.rotation = Quaternion.identity;
        UpdatePlayerTransform();
    }
    private void OnDisable()
    {
        SceneLoader.UpdatePlayerTransform -= UpdatePlayerTransform;
        CameraOffsetGA.SetCameraOffset -= OffsetCamera;
        CameraOffsetGA.PreviousCameraOffset -= PreviousOffset;
    }
    private void UpdatePlayerTransform()
    {
        if (FindAnyObjectByType<CubeTrackerYToggle>())
        {
            playerTransform = FindAnyObjectByType<CubeTrackerYToggle>().transform;
            transform.position =playerTransform.position;
        }
        else
            Debug.Log("Player not found");
    }
    private void OffsetCamera(Vector3 positionOffset,Vector3 desiredRotation)
    {
        //save current position and rotation
        oRotation = transform.rotation.eulerAngles;
        oPosition = transform.position;

        targetRotation = desiredRotation;
        StartCoroutine(nameof(Rotate));
    }
    private void PreviousOffset()
    {
        targetRotation = oRotation;
        StartCoroutine(nameof(Rotate));
    }
    private void LateUpdate()
    {
        if(playerTransform) 
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, Time.deltaTime * 5);
        }
    }
    IEnumerator Rotate()
    {
        float rate = 0;
        Quaternion currentRot, desiredRot;
        currentRot = transform.rotation;
        desiredRot = Quaternion.Euler(targetRotation);

        CuboidMaster.DisablePlayerMovement();
        while(rate < 1f)
        {
            yield return new WaitForEndOfFrame();
            rate += Time.deltaTime * 3;
            transform.rotation = Quaternion.Slerp(currentRot, desiredRot, rate);
        }
        CuboidMaster.EnablePlayerMovement();
    }
}
