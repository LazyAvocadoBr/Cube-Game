using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockPushable : MonoBehaviour
{
    public LayerMask mask;
    public PushableBlock pBlock;
    public GameObject trigger;
    private PlayerInput pInput;
    private bool bLock;
    private int defaultLayer;
    private void OnEnable()
    {
        defaultLayer = trigger.layer;
        CuboidMaster.DelResetGame += ResetObject;
        CuboidMaster.DelCheckpoint += ResetObject;
        pInput = new PlayerInput();
        pInput.Enable();
    }
    private void OnDisable()
    {
        pInput.Disable();
        CuboidMaster.DelResetGame -= ResetObject;
        CuboidMaster.DelCheckpoint -= ResetObject;
    }
    public void LockBlock()
    {
        bLock = true;
        trigger.layer = 0;
    }
    public void UnlockLock()
    {
        bLock = false;
        trigger.layer = defaultLayer;
    }
    private void ResetObject()
    {
        trigger.layer = defaultLayer;
        bLock = false;
    }
    private void ResetObject(Vector3 value)
    {
        trigger.layer = defaultLayer;
    }
    public bool IsLocked { get { return bLock; } }
}