using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CubeMovement : MonoBehaviour
{
    public float speed =  1;
    public bool bMove,bMoving;
    public AnimationCurve animCurve;
    public AudioSource aSource;
    //public bool bInputRef;
    public LayerMask mask;
    private PlayerInput pInput;
    private Quaternion desiredRot,startingRot;
    private Vector3 desiredPos, startPos,movingPosition,rotationDirection,moveDirection;
    private float rate;
    private Vector2 controlInput;
    private Vector3 origPosition,checkpoint;
    private Quaternion origRotation;
    public static Action<Transform> SendTransform = delegate { };
    private Rigidbody rBody;
    private bool bInput;
    private PushableBlock pBlock;
    private Transform localTransform;

    public static Action ReturnToCheckpointComplete = delegate { };
    private void Awake()
    {
        bInput = true;
        pInput = new PlayerInput();
        pInput.Enable();       
        MoveToGA.PlayerTransform += SendPlayerTransform;
        localTransform = transform;
        checkpoint = localTransform.position;
        origPosition = localTransform.position;
        origRotation = localTransform.rotation;
        rBody = GetComponent<Rigidbody>();
        CuboidMaster.DelEnablePlayerMovement += EnableInput;
        CuboidMaster.DelDisablePlayerMovement += DisableInput;
        CuboidMaster.DelResetGame += ResetPlayer;
        CuboidMaster.DelCheckpoint += Checkpoint;
        CuboidMaster.DelReturnToCheckpoint += ReturnToCheckpoint;
    }
    private void OnDisable()
    {
        CuboidMaster.DelEnablePlayerMovement -= EnableInput;
        CuboidMaster.DelDisablePlayerMovement -= DisableInput;
        MoveToGA.PlayerTransform -= SendPlayerTransform;
        CuboidMaster.DelResetGame -= ResetPlayer;
        CuboidMaster.DelCheckpoint -= Checkpoint;
        CuboidMaster.DelReturnToCheckpoint -= ReturnToCheckpoint;
        pInput.Disable();
    }
    private void Update()
    {
        if (!bMove && !bMoving && bInput)
            GetInput();

        if (!bInput)
        {
            if (bMoving)
                Moving();
            return;
        }
        if (bMove)
        {
            aSource.PlayOneShot(aSource.clip);
            startingRot = Quaternion.Euler(Vector3.zero);
            desiredRot = startingRot * Quaternion.Euler(rotationDirection);

            startPos = localTransform.position;
            desiredPos = localTransform.position + moveDirection;

            bMove = false;
            rate = 0;
            bMoving = true;            
        }

        if (bMoving)
            Moving();
    }   
    private void GetInput()
    {
        if (bMove) return; //only get input when not moving      
        controlInput = pInput.Player.Movement.ReadValue<Vector2>();
       
        SetMoveDirection(controlInput);
    }
    private void SetMoveDirection(Vector2 direction)
    {
        if (direction.y > 0.5f && CuboidMaster.PositionAvailable(localTransform.position + Vector3.forward,Vector3.forward))
        {
            moveDirection = CameraBaseInput.cameraDirection.forward;
            rotationDirection = CameraBaseInput.cameraDirection.right * 90;
            bMove = true;
        }
        else if (direction.y < -0.5f && CuboidMaster.PositionAvailable(localTransform.position + Vector3.back, Vector3.back))
        {
            moveDirection = -CameraBaseInput.cameraDirection.forward;
            rotationDirection = CameraBaseInput.cameraDirection.right * -90;
            bMove = true;
        }
        else if (direction.x > 0.5f && CuboidMaster.PositionAvailable(localTransform.position + Vector3.right, Vector3.right))
        {
            moveDirection = CameraBaseInput.cameraDirection.right;
            rotationDirection = CameraBaseInput.cameraDirection.forward * -90;
            bMove = true;
        }
        else if (direction.x < -0.5f && CuboidMaster.PositionAvailable(localTransform.position + Vector3.left, Vector3.left))
        {
            moveDirection = -CameraBaseInput.cameraDirection.right;
            rotationDirection = CameraBaseInput.cameraDirection.forward * 90;
            bMove = true;
        }
    }
    private void Moving()
    {
        rate += Time.deltaTime * speed;
        rate = Mathf.Clamp01(rate);
        localTransform.rotation = Quaternion.Lerp(startingRot, desiredRot, rate);
        movingPosition = Vector3.Lerp(startPos, desiredPos, rate);
        movingPosition.y = startPos.y + animCurve.Evaluate(rate);
        localTransform.position = movingPosition;
        if (rate == 1)
        {
            localTransform.position = desiredPos;
            bMoving = false;
        }
    }
    private void FloorCheck()
    {
        if(Physics.Raycast(localTransform.position,-Vector3.up,0.51f))
        {
            rBody.isKinematic = true;
            rBody.useGravity = false;
        }
        else
        {
            rBody.isKinematic = false;
            rBody.useGravity = true;
        }
    }
    private void EnableInput()
    {
        controlInput = Vector2.zero;
        bInput = true;
    }
    private void DisableInput()
    {
        bInput = false;
    }
    //public static bool GetState{ get{ return bInput; } }   
    private void SendPlayerTransform()
    {
        SendTransform(localTransform);
    }
    private void ResetPlayer()
    {        
        localTransform.position = origPosition;
        localTransform.rotation = origRotation;
        bInput = true;
    }
    private void Checkpoint(Vector3 value)
    {
        checkpoint = value;
    }
    private void ReturnToCheckpoint()
    {
        localTransform.parent = null;
        CuboidMaster.StateLock();
        localTransform.position = checkpoint;
        ReturnToCheckpointComplete();
        StartCoroutine(nameof(DelayedReset));
    }
    IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(0.1f);
        CuboidMaster.StateUnLock();
        CuboidMaster.CheckpointReset();
    }

}