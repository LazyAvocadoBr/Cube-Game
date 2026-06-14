using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CubePositionDebugger : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        if (CuboidMaster.CubeInfo == null) return;
        foreach(CubeData cube in CuboidMaster.CubeInfo.Values) 
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(cube.position, new Vector3(0.25f, 0.25f, 0.25f));
        }
    }
}
