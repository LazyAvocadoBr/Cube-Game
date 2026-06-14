using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderToggleGA : GameActions
{
    public bool bDisable,bLock;
    public Collider localCollider;

    private void OnEnable()
    {
        CuboidMaster.DelResetGame += ResetToDefaults;
        CuboidMaster.DelReturnToCheckpoint += ResetToDefaults;
        CuboidMaster.DelStateLock += Lock;
        CuboidMaster.DelStateUnlock += UnLock;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetToDefaults;
        CuboidMaster.DelReturnToCheckpoint -= ResetToDefaults;
        CuboidMaster.DelStateLock -= Lock;
        CuboidMaster.DelStateUnlock -= UnLock;
    }
    public override void Action()
    {
        if (bLock) return;
        if (bDisable)
            localCollider.enabled = false;
        else if (localCollider.enabled)
            localCollider.enabled = false;
        else
            localCollider.enabled = true;
    }
    public override void ResetToDefaults()
    {
        localCollider.enabled = true;
    }
    private void Lock() { bLock = true; }
    private void UnLock() { bLock = false; }
}
