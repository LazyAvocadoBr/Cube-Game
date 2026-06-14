using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterBlockGA : GameActions
{
    [SerializeField]
    private bool bBlockOnly;
    [SerializeField]
    private GameActions playSound;
    [SerializeField]
    private Transform teleportTarget;
    [SerializeField]
    private Animator spinnerAnimator;

    private TeleporterBlockGA destination;    
    private bool bActive,bPlayer,bTeleporting,bCheckpointActive;
    private Transform objectToTeleport;

    private void OnEnable()
    {        
        CuboidMaster.DelReturnToCheckpoint += CheckpointReset;
        CuboidMaster.DelResetGame += ResetToDefaults;
        CuboidMaster.DelCheckpoint += Checkpoint;
    }
    private void OnDisable()
    {
        CuboidMaster.DelReturnToCheckpoint -= CheckpointReset;
        CuboidMaster.DelResetGame -= ResetToDefaults;
        CuboidMaster.DelCheckpoint -= Checkpoint;
    }
    public override void ResetToDefaults()
    {
        bActive = false;
        bCheckpointActive = false;
    }
    public override void CheckpointReset()
    {
        if (bCheckpointActive) return;
        bActive = false;
    }
    public override void Action()
    {
        if (bActive || bTeleporting) return;
        if (destination)
        {
            if (destination.IsOccupied) return;
        }
        else
        {
            Debug.LogWarning("Destination for " + transform.name + " has not been set.");
            return;
        }
        if(objectToTeleport == null) return;
        bActive = true;
        playSound.Action();
        StartCoroutine(nameof(TeleportSequence));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (bTeleporting) return;
        if(bBlockOnly && !other.CompareTag("Player"))
        {
            objectToTeleport = other.transform;
            return;
        }
        if (!bBlockOnly && other.CompareTag("Player"))
        {
            bPlayer = true;
            objectToTeleport = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (bTeleporting) return;
        if (other.CompareTag("Player"))
            bPlayer = false;
        bActive = false;
    }
    private void Checkpoint(Vector3 value)
    {
        if(bActive)
            bCheckpointActive = true;
    }
    public Vector3 Position { get { return teleportTarget.position; } }
    public Transform DestinationTransform =>teleportTarget.transform;
    public void MakeActive() { bActive = true; }
    public bool IsOccupied { get{ return bActive; } }
    public void TeleportingActive() { bTeleporting = true;}
    public void TeleportingInActive() { bTeleporting = false; }
    public void SetDestination(TeleporterBlockGA value)
    {
        destination = value;
    }
    private void OnDrawGizmos()
    {
        if(destination)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, destination.Position);
        }
    }
    IEnumerator TeleportSequence()
    {
        //register cube to prevent the player from occupying the teleporter's space
        CuboidMaster.RegisterCube(new CubeData(destination.Position, false, true, false), destination.DestinationTransform.GetInstanceID());

        destination.TeleportingActive();
        bTeleporting = true;

        //lock player or cube
        if (bPlayer)
            CuboidMaster.DisablePlayerMovement();
        else 
            objectToTeleport.GetComponent<PushableBlock>().LockCube();

        destination.MakeActive();

        yield return new WaitForSeconds(0.1f);

        //trigger telporting animation
        spinnerAnimator.SetTrigger("Teleporting");

        //shrink
        float rate = 0;        
        while(rate < 1)
        {
            rate = Mathf.Clamp(rate + Time.deltaTime * 2,0,1);
            objectToTeleport.localScale = Vector3.Lerp(new Vector3(1,1,1), Vector3.zero, rate);
            yield return new WaitForEndOfFrame();
        }

        objectToTeleport.position = destination.Position;
        
        //unshrink
        rate = 0;
        while (rate < 1)
        {
            rate = Mathf.Clamp(rate + Time.deltaTime * 2, 0, 1);
            objectToTeleport.localScale = Vector3.Lerp(Vector3.zero,new Vector3(1, 1, 1), rate);
            yield return new WaitForEndOfFrame();
        }

        if (bPlayer)
            CuboidMaster.EnablePlayerMovement();
        else
            objectToTeleport.GetComponent<PushableBlock>().UnlockCube();

        objectToTeleport = null;
        bTeleporting =false;
        bActive = false;
        destination.TeleportingInActive();

        //removes blocker        
        CuboidMaster.RemoveCube(destination.DestinationTransform.GetInstanceID());
    }
}
