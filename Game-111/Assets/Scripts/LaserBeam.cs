using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LaserBeam : GameActions
{
    [SerializeField]
    private Transform laserTarget;
    [SerializeField]
    private LaserType type;
    [SerializeField]
    private LaserProperties laserDatabase;
    [SerializeField]
    private bool bSetDistance,bDisableOnStart;
    [SerializeField]
    private float setDistance = 5;    
    [SerializeField]
    private LineRenderer laser;
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private float distance;   
    private LayerMask mask;
    private RaycastHit hitInfo;
    private bool bActive = true;

    private void Awake()
    {
        SetBeam(type);
        CuboidMaster.DelResetGame += ResetToDefaults;
        CuboidMaster.DelCheckpointReset += ResetToDefaults;
    }
    public void SetBeam(LaserType laserType)
    {
        foreach (LaserAttributes item in laserDatabase.laserVariants)
        {
            if (laserType == item.type)
            {
                mask = item.mask;
                laser.material = item.material;
                return;
            }
        }
    }
    public LaserType LaserType { get { return type; } }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetToDefaults;
        CuboidMaster.DelCheckpointReset -= ResetToDefaults;
    }
    public override void Action()
    {
        bActive = true;
        bActive = true;
        laser.enabled = true;
        boxCollider.enabled = true;
    }
    public override void DeAction()
    {
        bActive = false;
        laser.enabled = false;
        boxCollider.enabled = false;
    }
    private void Start()
    {
        laser.SetPosition(0,Vector3.zero);
        if (bDisableOnStart) DeAction();
    }
    private void FixedUpdate()
    {
        if (bActive)
            UpdateLaser();
    }
    public override void ResetToDefaults()
    {
        if (bDisableOnStart)
        {
            laser.enabled = false;
            boxCollider.enabled = false;
        }
        else
        {
            boxCollider.size = new Vector3(0.25f, 0.25f, 0.25f);
            bActive = false;
            StartCoroutine(nameof(DelayedReset));
        }
    }
    IEnumerator DelayedReset()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        bActive = true;
    }
    private void UpdateLaser()
    {
        if (bSetDistance)
            distance = setDistance;
        else if (Physics.Raycast(transform.position, transform.right, out hitInfo, 20, mask))
        {
            distance = hitInfo.distance;
        }
        else
            distance = 20;

        //update line renderer and box collider
        boxCollider.size = new Vector3(distance, 0.25f, 0.25f);
        boxCollider.center = new Vector3(distance * 0.5f, 0, 0);
        laser.SetPosition(1, Vector3.right * distance);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(laserTarget)
        {
            distance = Vector3.Distance(transform.position, laserTarget.position);
            boxCollider.size = new Vector3(distance, 0.25f, 0.25f);
            boxCollider.center = new Vector3(distance * 0.5f, 0, 0);
            laser.SetPosition(1, Vector3.right * distance);
        }
    }
#endif
}
