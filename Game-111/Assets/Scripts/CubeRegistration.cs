using UnityEngine;

public class CubeRegistration : MonoBehaviour
{
    public RegisterCube[] cubes;
    public void RegisterCubes()
    {
        foreach (var cube in cubes) 
            cube.UpdateCubeData();
    }
    public void UnRegisterCubes()
    {
        foreach(var cube in cubes)
            cube.RemoveCube();
    }
}
