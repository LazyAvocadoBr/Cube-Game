using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    [SerializeField]
    private RegisterCube cubeRegister;
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private AnimationCurve animCurve;
    [SerializeField]
    private AudioSource aSource;    
    [SerializeField]
    private DisableCubeGA cubeDisabler;

    private Collider localCollider;
    private Vector3 direction;
    private bool bMove,bColliderActive;   
    private Vector3 desiredPos, startPos, movingPosition, origPosition,checkpointPosition;
    private Quaternion origRot,checkpointRot;
    private float rate;
    private Transform localTransform;
    private bool bFalling,bCollided,bCheckpoint,bHidden,bCheckpointHiddenState,bLocked;
    private Rigidbody rBody;

    private void Awake()
    {
        localTransform = transform;
        localCollider = GetComponent<Collider>();
        rBody = GetComponent<Rigidbody>();  
        checkpointPosition = origPosition = localTransform.position;
        origRot = checkpointRot = localTransform.rotation;
        bColliderActive = localCollider.enabled;
        CuboidMaster.DelResetGame += ResetObject;
        CuboidMaster.DelCheckpoint += Checkpoint;
        CuboidMaster.DelReturnToCheckpoint += ReturnToCheckpoint;
        CuboidMaster.MovePushableCube += MoveToward;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetObject;
        CuboidMaster.DelCheckpoint -= Checkpoint;
        CuboidMaster.DelReturnToCheckpoint -= ReturnToCheckpoint;
        CuboidMaster.MovePushableCube -= MoveToward;
    }
    private void Checkpoint(Vector3 value)
    {       
        if(bFalling || bMove)
        {
            StartCoroutine(nameof(DelayedCheckpoint));
            return;
        }
        bCheckpoint = true;
        bCheckpointHiddenState = bHidden;
        checkpointPosition = localTransform.position;
        checkpointRot = localTransform.rotation;
        bColliderActive = localCollider.enabled;
    }
    //used for correctly capturing the state of a falling block
    IEnumerator DelayedCheckpoint()
    {        
        while(bMove || bFalling)
            yield return new WaitForEndOfFrame();
        checkpointPosition = localTransform.position;
        checkpointRot = localTransform.rotation;
        bColliderActive = localCollider.enabled;
        bCheckpointHiddenState = bHidden;
        bCheckpoint = true;
    }
    public void ReturnToCheckpoint()
    {
        if (bCheckpoint)
        {
            StopAllCoroutines();
            rBody.isKinematic = true;
            rBody.useGravity = false;
            localTransform.position = checkpointPosition;
            cubeRegister.UpdateCubeData();
            localTransform.rotation = checkpointRot;
            bMove = false;
            localCollider.enabled = bColliderActive;
            if(!bCheckpointHiddenState)
                cubeDisabler.DeAction();
        }
        else ResetObject();
    }
    public void MoveToward(Vector3 position, Vector3 desiredDirection)
    {
        if (bLocked) return;
        if (position == transform.position)
        {
            direction = desiredDirection;
            StartCoroutine(nameof(MoveCube));
        }
    }
    private void ResetObject()
    {
        cubeDisabler.DeAction();
        rBody.isKinematic = true;
        rBody.useGravity = false;
        StopAllCoroutines();
        if(transform.parent)
            transform.transform.parent = null;       
        localTransform.position = origPosition;
        cubeRegister.UpdateCubeData();
        localTransform.rotation = origRot;
        bMove = false;
        localCollider.enabled = true;
        bColliderActive = true;
        bFalling = false;
        bCollided = false;  
        bCheckpoint = false;
        bLocked = false;
    }
    public void LockCube() 
    { 
        bLocked = true;
        rBody.isKinematic = true;
    }  
    public void UnlockCube() 
    {  
        StartCoroutine(nameof(CheckForFloor));
    }
    IEnumerator MoveCube()
    {
        //Remove cube
        cubeRegister.UpdateOut();
      
        //set variables for movement
        bMove = true;
        aSource.PlayOneShot(aSource.clip);
        startPos = localTransform.position;
        desiredPos = localTransform.position + direction;       
        rate = 0;
             

        //move sequence
        while (bMove)
        {
            yield return new WaitForEndOfFrame();
            rate += Time.deltaTime * speed;
            movingPosition = Vector3.Lerp(startPos, desiredPos, rate);
            localTransform.position = movingPosition;
            if (rate >= 1)
            {
                localTransform.position = desiredPos;
                rate = 0;
                bMove = false;
            }
        }
        StartCoroutine(nameof(CheckForFloor));  
    }
    IEnumerator CheckForFloor()
    {
        float internalTimer = 0; //used for timing out the block when falling  
        //check for floor
        if (CuboidMaster.FloorExist(localTransform.position))
        {
            //add new position
            cubeRegister.UpdateCubeData();
            bLocked = false;
        }
        else //fall
        {
            //set variables for falling
            bFalling = true;   //tracks the state of falling
            rBody.isKinematic = false;
            rBody.useGravity = true;
            internalTimer = 0;
            bCollided = false;

            while (internalTimer < 5f)
            {
                yield return new WaitForFixedUpdate();

                if (rBody.linearVelocity.magnitude == 0)
                {
                    bCollided = true;
                    break;
                }
                internalTimer += Time.deltaTime;
            }

            //reset fall and stops physics movement
            rBody.isKinematic = true;
            rBody.useGravity = false;
            bFalling = false;

            if (bCollided)
                cubeRegister.UpdateCubeData();
            else
            {
                bHidden = true;
                cubeDisabler.Action();
            }
            bLocked = false;
        }
    }
}
