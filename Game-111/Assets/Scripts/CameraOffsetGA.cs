using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraOffsetGA : GameActions
{
    [SerializeField]
    private Vector3 positionOffset;
    [SerializeField]
    private Vector3 rotationOffset;       
  
    private bool bActive;

    public static Action DisableActiveCamerOffset = delegate { };
    public static Action<Vector3,Vector3> SetCameraOffset = delegate { };  
    public static Action PreviousCameraOffset = delegate { };

    private void Awake()
    {
        DisableActiveCamerOffset += DisableActiveCamOffset;
    }
    private void OnDisable()
    {
        DisableActiveCamerOffset -= DisableActiveCamOffset;
    }
    public override void Action()
    {
        DisableActiveCamerOffset -= DisableActiveCamOffset;
        bActive = true;
        SetCameraOffset(positionOffset, rotationOffset);
        DisableActiveCamerOffset += DisableActiveCamOffset;
    }
    public override void DeAction()
    {
        PreviousCameraOffset();
    }
    public override void CheckpointReset()
    {
        if(bActive)
            SetCameraOffset(positionOffset, rotationOffset);
    }
    public override void ResetToDefaults()
    {
        bActive = false;
    }
    private void DisableActiveCamOffset()
    {
        bActive = false;
    }
}
