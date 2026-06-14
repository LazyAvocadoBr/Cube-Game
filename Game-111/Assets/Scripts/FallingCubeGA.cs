using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCubeGA : GameActions
{
    public bool bStay;
    public Rigidbody rBody;
    public MeshRenderer mRenderer;
    [SerializeField]
    private Collider triggerCollider, cubeCollider;
    [SerializeField]
    private RegisterCube cubeRegister;
    private Vector3 origPosition;
    private Quaternion origRotation;
    private bool bFallen,bCheckpointFallen;
    private float timeStamp;
    private bool bCollided;
    private void Awake()
    {
        origPosition = cubeRegister.transform.position;
        origRotation = cubeRegister.transform.rotation;
        CuboidMaster.DelResetGame += ResetObject;
        CuboidMaster.DelCheckpointReset += CheckpointReset;
        CuboidMaster.DelCheckpoint += Checkpoint;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetObject;
        CuboidMaster.DelCheckpointReset -= CheckpointReset;
        CuboidMaster.DelCheckpoint -= Checkpoint;
    }
    private void ResetObject()
    {       
        StopAllCoroutines();
        triggerCollider.enabled = true;
        cubeCollider.enabled = true;
        rBody.isKinematic = true;
        rBody.useGravity = false;       
        cubeRegister.transform.position = origPosition;
        cubeRegister.UpdateCubeData();
        cubeRegister.transform.rotation = origRotation;
        mRenderer.enabled = true;
        bFallen = false;
        bCheckpointFallen = false;
    }
    private void CheckpointReset(Vector3 value)
    {
        CheckpointReset();
    }
    public override void CheckpointReset()
    {
        StopAllCoroutines();
        if (bCheckpointFallen) return;
        rBody.isKinematic = true;
        rBody.useGravity = false;
        cubeRegister.transform.position = origPosition;
        cubeRegister.UpdateCubeData();
        cubeRegister.transform.rotation = origRotation;
        mRenderer.enabled = true;
    }
    public override void DeAction()
    {
        rBody.isKinematic = false;
        rBody.useGravity = true;
        if (bStay) return;
        StartCoroutine(nameof(Fall));
    }
    private void Checkpoint(Vector3 value)
    {
        if (bFallen)
            bCheckpointFallen = true;
    }
    IEnumerator Fall()
    {
        //cube out of reach
        cubeRegister.UpdateOut();

        timeStamp = Time.time;   
        bCollided = false;
        while(Time.time - timeStamp < 5)
        {
            yield return new WaitForFixedUpdate();
            if(rBody.linearVelocity.magnitude == 0)
            {
                bCollided = true;
                break;
            }
        }
        //stops physics
        rBody.isKinematic = true;
        rBody.useGravity = false;

        if (bCollided)
        {
            cubeRegister.UpdateCubeData();           
        }
        else
        {           
            mRenderer.enabled = false;         
        }
        bFallen = true;
    }
}
