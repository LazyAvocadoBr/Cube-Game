using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointGA : GameActions
{
    public Transform checkpointPos;
    public override void Action()
    {
        StartCoroutine(nameof(DelayedCheckpoint));
    }
    IEnumerator DelayedCheckpoint()
    {
        yield return new WaitForSeconds(0.25f);
        CuboidMaster.Checkpoint(checkpointPos.position);
    }
}
