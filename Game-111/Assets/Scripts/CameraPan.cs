using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPan : MonoBehaviour
{
    public float speed = 2;
    private CinemachineVirtualCamera vCam;

    private PlayerInput pInput;
    private Vector2 direction;
    private CinemachineTransposer transposer;
    private Vector3 origOffset;
    
    private void Awake()
    {   
        vCam = GetComponent<CinemachineVirtualCamera>();
        vCam.transform.parent = null;
        vCam.transform.localRotation = Quaternion.Euler(45, 0, 0);
        transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        origOffset = transposer.m_FollowOffset;     
    }
    private void OnEnable()
    {
        SceneLoader.UpdatePlayerTransform += UpdatePlayerTransform;
        SceneLoader.DisableCameraDamp += DisableDamp;
        SceneLoader.EnableCameraDamp += EnableDamp;
        UpdatePlayerTransform();       
    }
    private void OnDisable()
    {
        SceneLoader.UpdatePlayerTransform -= UpdatePlayerTransform;
        SceneLoader.DisableCameraDamp -= DisableDamp;
        SceneLoader.EnableCameraDamp -= EnableDamp;
    }
    private void UpdatePlayerTransform()
    {
        if (GameObject.FindAnyObjectByType<CubeTrackerYToggle>())
        {
            Transform target = GameObject.FindAnyObjectByType<CubeTrackerYToggle>().transform;
            vCam.Follow = target;           
            //vCam.transform.position = origOffset + target.position;
        }
        else
            Debug.Log("Player not found");
    }
    private void DisableDamp()
    {
        transposer.m_XDamping = 0;
        transposer.m_YDamping = 0;
        transposer.m_ZDamping = 0;
    }
    private void EnableDamp()
    {
        transposer.m_XDamping = 2;
        transposer.m_YDamping = 2;
        transposer.m_ZDamping = 2;
    }   
}
