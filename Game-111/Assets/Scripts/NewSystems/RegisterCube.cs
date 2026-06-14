using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterCube : MonoBehaviour
{
    [SerializeField]
    private bool bPushable,bDonotRegisterOnStart;

    private void Start()
    {
        if (bDonotRegisterOnStart) return;
        AddCube();
    }
    public void RemoveCube()
    {
        CuboidMaster.RemoveCube(GetInstanceID());      
    }
    public void AddCube()
    {
        CuboidMaster.RegisterCube(new CubeData(transform.position, false, true, bPushable),GetInstanceID());
    }
    public void UpdateCubeData()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        CuboidMaster.UpdateCubeData(GetInstanceID(),new CubeData(transform.position, false, true, bPushable));
    }
    public void UpdateOut() //sets position to Vector3 999, removing it from being a valid position
    {
        CuboidMaster.UpdateCubeData(GetInstanceID(), new CubeData(new Vector3(999,999,999), false, true, bPushable));
    }
    private void OnDisable()
    {
        RemoveCube();
    }
}
