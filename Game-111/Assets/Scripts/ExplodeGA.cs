using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeGA : GameActions
{
    [SerializeField]
    private RegisterCube cubeRegister;
    [SerializeField]
    private MeshRenderer origMesh;
    [SerializeField]
    private PushableBlock pushBlock;
    [SerializeField]
    private List<Rigidbody> pieces;
    [SerializeField] 
    private bool bCheckpoint,bPlayer,bDestroyed;
    private Transform parentTrans;
    private Vector3 parentOrigPos;
    private void Awake()
    {    
        CubeMovement.ReturnToCheckpointComplete += CheckpointComplete;
        CuboidMaster.DelReturnToCheckpoint += CheckpointReset;
        CuboidMaster.DelResetGame += ResetObject;
        CuboidMaster.DelCheckpoint += CheckpointTriggered;
        parentTrans = transform.parent;
        parentOrigPos = parentTrans.position;
    }
    private void OnDisable()
    {
        CuboidMaster.DelReturnToCheckpoint -= CheckpointReset;
        CuboidMaster.DelResetGame -= ResetObject;
        CubeMovement.ReturnToCheckpointComplete -= CheckpointComplete;
        CuboidMaster.DelCheckpoint -= CheckpointTriggered;
    }   
    public override void Action()
    {
        origMesh.enabled = false;
        transform.parent = null;
        foreach(Rigidbody item in pieces)
        {
            item.gameObject.SetActive(true);
            item.isKinematic = false;
            item.AddExplosionForce(3, transform.position, 1.5f,2,ForceMode.Impulse);
        }
        if (bPlayer)
        {
            CuboidMaster.DisablePlayerMovement();
            StartCoroutine(nameof(ResetToCheckpoint));
        }
        else
            cubeRegister.UpdateOut();
    }
    private void CheckpointTriggered(Vector3 value)
    {
        if (origMesh.enabled) return;
        bDestroyed = true;
    }
    public override void CheckpointReset()
    {
        if(bDestroyed) return;
        ResetObject();
    }
    private void ResetObject()
    {        
        origMesh.enabled = true;
        transform.parent = parentTrans;
        transform.localPosition = Vector3.zero;
        foreach(Rigidbody item in pieces)
        {
            item.isKinematic = true;
            item.transform.localPosition = Vector3.zero;
            item.gameObject.SetActive(false);   
        }
    }
    private void CheckpointComplete()
    {
        bCheckpoint = true;
    }
    IEnumerator ResetToCheckpoint()
    {
        yield return new WaitForSeconds(1);
        ResetObject();
        CuboidMaster.ReturnToCheckpoint();        
        while (!bCheckpoint)
            yield return new WaitForEndOfFrame();
        CuboidMaster.EnablePlayerMovement();
        bCheckpoint = false;
    }
}
