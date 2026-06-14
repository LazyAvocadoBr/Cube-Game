using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionTrigger : MonoBehaviour
{
    public bool bTriggerOnce;
    public List<GameActions> action;
    public List<GameActions> deAction;
    private bool bActive,bDeActive,bTriggered,bLock,bStateChange;

    private void OnEnable()
    {
        CuboidMaster.DelResetGame += ResetObject;
        CuboidMaster.DelCheckpointReset += CheckpointReset;
        CuboidMaster.DelStateLock += Lock;
        CuboidMaster.DelStateUnlock += UnLock;
        CuboidMaster.DelCheckpoint += Checkpoint;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetObject;
        CuboidMaster.DelCheckpointReset -= CheckpointReset;
        CuboidMaster.DelStateLock -= Lock;
        CuboidMaster.DelStateUnlock -= UnLock;
        CuboidMaster.DelCheckpoint -= Checkpoint;
    }
    public void TriggerActions()
    {
        StartCoroutine(nameof(GameActionSequence));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (bActive || bLock) return;
        if (bTriggered && bTriggerOnce) return;
        StartCoroutine(nameof(GameActionSequence));
    }
    private void OnTriggerExit(Collider other)
    {        
        if (bDeActive || bLock) return;
        if (bTriggerOnce && bTriggered) return;
        StartCoroutine(nameof(GameDeActionSequence));
    }
    private void ResetObject()
    {
        bLock = bActive = bDeActive = bTriggered = bStateChange = false;
       
        foreach (GameActions item in action)
            item.ResetToDefaults();
        foreach (GameActions item in deAction)
            item.ResetToDefaults();
    }
    private void CheckpointReset()
    {
        if (bStateChange) return;
        bLock = bActive = bDeActive = bTriggered = false;
        foreach (GameActions item in action)
            item.CheckpointReset();
        foreach (GameActions item in deAction)
            item.CheckpointReset();       
    }
    private void Checkpoint(Vector3 value)
    {
        if (bTriggerOnce && bTriggered)
            bStateChange = true;
    }
    private void Lock() { bLock = true; }
    private void UnLock() { bLock = false; }
    IEnumerator GameActionSequence()
    {
        bActive = true;
        if (bTriggerOnce)
            bTriggered = true;
        int count = 0;
        foreach(GameActions item in action)
        {
            count++;
            yield return new WaitForSeconds(item.delay);
            item.Action();
        }
        bActive = false;
    }
    IEnumerator GameDeActionSequence()
    {        
        bDeActive = true;
        foreach (GameActions item in deAction)
        {
            yield return new WaitForSeconds(item.delay);
            item.DeAction();
        }
        bDeActive = false;
    }
}
