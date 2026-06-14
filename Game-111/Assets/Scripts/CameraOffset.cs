using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraOffset : MonoBehaviour
{
    private CinemachineTransposer transposer;
    private CinemachineVirtualCamera vCam;
    private Vector3 oPositionOffset, oRotationOffset;
    private Vector3 desiredPositionOffset, desiredRotationOffset;
    private bool bPrevious;
    private void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        //vCam.transform.parent = null;
        vCam.transform.localRotation = Quaternion.Euler(45, 0, 0);
        transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        CameraOffsetGA.SetCameraOffset += SetCameraOffset;
        CameraOffsetGA.PreviousCameraOffset += PreviousCameraOffset;
    }
    private void OnDisable()
    {
        CameraOffsetGA.SetCameraOffset -= SetCameraOffset;
        CameraOffsetGA.PreviousCameraOffset -= PreviousCameraOffset;
    }
    private void SetCameraOffset(Vector3 positionOffset, Vector3 rotationOffset)
    {
        oPositionOffset = transposer.m_FollowOffset;
        oRotationOffset = vCam.transform.rotation.eulerAngles;

        desiredPositionOffset = positionOffset;
        desiredRotationOffset = rotationOffset;
        StartCoroutine(nameof(BlendToNewOffset));

      /*  transposer.m_FollowOffset = positionOffset;
        vCam.transform.rotation = Quaternion.Euler(rotationOffset);*/
    }
    private void PreviousCameraOffset()
    {        
        bPrevious = true;
        StartCoroutine(nameof(BlendToNewOffset));
    }
    IEnumerator BlendToNewOffset()
    {
        float rate = 0;
        while(rate < 1.0f) 
        {
            yield return new WaitForEndOfFrame();
            rate += Time.deltaTime * 2;
            if (bPrevious)
            {
                transposer.m_FollowOffset = Vector3.Slerp(desiredPositionOffset, oPositionOffset, rate);
                vCam.transform.rotation = Quaternion.Euler(Vector3.Slerp(desiredRotationOffset, oRotationOffset, rate));
            }
            else
            {
                transposer.m_FollowOffset = Vector3.Slerp(oPositionOffset, desiredPositionOffset, rate);
                vCam.transform.rotation = Quaternion.Euler(Vector3.Slerp(oRotationOffset, desiredRotationOffset, rate));
            }
        }
        bPrevious = false;
    }
}
