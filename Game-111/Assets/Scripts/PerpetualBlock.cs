using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualBlock : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float pauseTime = 2;
    [SerializeField]
    private LockAndParentGA lockParent;   
    [SerializeField]
    private RegisterCube cubeRegister;

    private List<Transform> wayPoints;
    private float rate,holdTime;
    private bool bForward = true;
    private Transform localTrans;

    private void Awake()
    {
        localTrans = transform;
        wayPoints = new List<Transform>();
        for (int x = 0; x < transform.childCount; x++)
        {
            if(!transform.GetChild(x).GetComponent<Collider>())
                wayPoints.Add(transform.GetChild(x));
        }
        //unparent wapoints
        foreach (Transform item in wayPoints)
            item.parent = null;
    }
   
    void Update()
    {
       if(rate < 1)
        {
            rate += Time.deltaTime * speed; 
            rate = Mathf.Clamp01(rate);
           
            if(bForward)
                localTrans.position = Vector3.Lerp(wayPoints[0].position, wayPoints[1].position, rate);
            else
                localTrans.position = Vector3.Lerp(wayPoints[1].position, wayPoints[0].position, rate);

            if (rate == 1)
            {
                if(lockParent.HasPlayer)
                    CuboidMaster.EnablePlayerMovement(); 
                
                cubeRegister.UpdateCubeData();

                //allows for the attached no player block to set position in cubeList
                lockParent.Unlock();
            }
        }
       else
        {
            holdTime += Time.deltaTime;
            if(holdTime > pauseTime - 0.25f && lockParent.HasPlayer)
            {
                CuboidMaster.DisablePlayerMovement();
            }
            if(holdTime > pauseTime)
            {
                holdTime = 0;
                rate = 0;
                if (bForward)
                {
                    bForward = false;
                }
                else
                {
                    bForward = true;
                }
                lockParent.Lock();
                cubeRegister.UpdateOut();
            }
        }
    }
}
