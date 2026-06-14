using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveToGA : GameActions
{
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private Transform localTransform,positionA,positionB;   
    private bool bMoving, bMoveTo,bPlayer;
    [SerializeField]
    private RegisterCube cubeRegister;

    private Transform objectTrans;  

    public static Action PlayerTransform = delegate { };

    private Vector3 origPosition;
    public float MoveSpeed { get { return speed; } set {  speed = value; } }

 /*   public bool bUpdate;
    private void Update()
    {
        if(bUpdate) 
        { 
            bUpdate = false;
            cubeRegister.UpdateCubeData();
        }
    }*/
    private void Awake()
    {
        CuboidMaster.DelResetGame += ResetObject;
        CuboidMaster.DelCheckpointReset += ResetObject;
        bMoveTo = true;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetObject;
        CuboidMaster.DelCheckpointReset -= ResetObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayer = true;            
        }
        objectTrans = other.transform;
    }
    private void OnTriggerExit(Collider other) //allows the player to pass over switches
    {
        if (other.CompareTag("Player"))
        {
            if (objectTrans && objectTrans.CompareTag("Player"))
            {
                objectTrans.parent = null;
                objectTrans = null;
            }
        }
    }
    public override void ResetToDefaults()
    {
        ResetObject();
    }
    private void ResetObject()
    {
        StopAllCoroutines();
        bMoveTo = true;
        bMoving = false;        
        localTransform.position = positionA.position;
        cubeRegister.UpdateCubeData();
    }
    public override void Action()
    {
        if (bMoving) return;
        if(objectTrans)
            StartCoroutine(nameof(Move));
    }
    public override void DeAction()
    {
        if (bMoving) return;
        if (objectTrans)
            StartCoroutine(nameof(Move));
    }
    IEnumerator Move()
    {
        if(bPlayer)
            CuboidMaster.DisablePlayerMovement();

        if (objectTrans) //remove pushable position from list. This removes phantom positions
        {
            if (objectTrans.TryGetComponent(out RegisterCube cubeRegister))
            {
                cubeRegister.UpdateOut();
            }
            objectTrans.parent = null;
        }

        if(!objectTrans) { yield break; }
        
        objectTrans.parent = localTransform;

        if (objectTrans.TryGetComponent(out Rigidbody rb))      
            rb.isKinematic = true;

        bMoving = true;
        float rate = 0;
        Vector3 startPos,destination;

        if(bMoveTo)
        {
            startPos = positionA.position;
            destination = positionB.position;
            bMoveTo = false;
        }
        else
        {
            startPos = positionB.position;
            destination = positionA.position;
            bMoveTo = true;
        }
        while (rate < 1)
        {
            rate += Time.deltaTime * speed;
            rate = Mathf.Clamp01(rate);
            yield return new WaitForEndOfFrame();
            localTransform.position = Vector3.Lerp(startPos, destination, rate);
        }

        //update cube list
        cubeRegister.UpdateCubeData();

        if (bPlayer)
        {
            bPlayer = false;
            CuboidMaster.EnablePlayerMovement();
        }

        if (objectTrans)
        {
            //snap position before registering. Because the system evaluates absolute position snapping prevents errors
            objectTrans.localPosition = Vector3.up;

            if(objectTrans.TryGetComponent(out RegisterCube cubeRegister))
            {
                cubeRegister.UpdateCubeData();
            }
            objectTrans.parent = null;
        }
        bMoving = false;
    }
}
