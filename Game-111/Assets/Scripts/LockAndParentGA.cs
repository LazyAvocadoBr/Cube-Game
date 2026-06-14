using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAndParentGA : GameActions
{
    public Transform parentObject;
    private Transform target;
    private bool bPlayer;
    public override void Action()
    {
        Lock();
    }
    public override void DeAction()
    {
        Unlock();
    }
    public void Lock()
    {
        if (target)
        {
            if (target.TryGetComponent(out RegisterCube cubeRegister))
            {
                cubeRegister.UpdateOut();
            }
            target.parent = parentObject;
            target.localPosition = new Vector3(0, 1, 0);
        }
    }
    public void Unlock()
    {
        if (target)
        {
            if (target.TryGetComponent(out RegisterCube cubeRegister))
                cubeRegister.UpdateCubeData();
            target.parent = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            bPlayer = true;
        target = other.transform;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            bPlayer = false;
        if (target)
        {
            target.parent = null;
            target = null;
        }
    }
    public bool HasPlayer { get { return bPlayer; } }
}
