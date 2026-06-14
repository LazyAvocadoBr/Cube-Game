using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public static class CuboidMaster
{
    public static Action DelFadeIn = delegate { };
    public static Action DelFadeOut = delegate { };
    public static Action DelResetGame = delegate { };
    public static Action DelDisablePlayerMovement = delegate { };
    public static Action DelEnablePlayerMovement = delegate { };
    public static Action<Vector3> DelCheckpoint = delegate { };
    public static Action DelReturnToCheckpoint = delegate { };
    public static Action DelResetPlayerDefaults = delegate { };
    public static Action DelCheckpointReset = delegate { };
    public static Action DelStateLock = delegate { };
    public static Action DelStateUnlock = delegate {};
    public static Action<Vector3,Vector3> MovePushableCube = delegate { };
    public static void FadeIn() { DelFadeIn(); }
    public static void FadeOut() { DelFadeOut(); }
    public static void ResetGame() { DelResetGame(); }
    public static void EnablePlayerMovement()
    {
        DelEnablePlayerMovement();
        DelResetPlayerDefaults();
    }
    public static void DisablePlayerMovement() { DelDisablePlayerMovement(); }
    public static void ReturnToCheckpoint() 
    { 
        DelReturnToCheckpoint(); 
        DelCheckpointReset();
    }
    public static void Checkpoint(Vector3 value) { DelCheckpoint(value); }
    public static void CheckpointReset() { DelCheckpointReset(); }
    public static void StateLock() { DelStateLock(); }
    public static void StateUnLock() { DelStateUnlock(); }   

    //private static List<CubeData> sceneCubes = new List<CubeData>();

    private static Dictionary<int,CubeData> cubeInfo = new Dictionary<int, CubeData>();
    public static Dictionary<int,CubeData> CubeInfo => cubeInfo;
    //public static List<CubeData> SceneCubes => sceneCubes;
    public static void RegisterCube(CubeData cube,int key) //key instanceID
    {
        cubeInfo.Add(key, cube);
    }
    public static void RemoveCube(int key)
    {
        cubeInfo.Remove(key);
    }
    public static void UpdateCubeData(int key, CubeData cube)
    {
        cubeInfo[key] = cube;
    }
    //used by player
    public static bool PositionAvailable(Vector3 position,Vector3 direction)
    {
        //checks position
        foreach(CubeData cube in cubeInfo.Values)
        {
            if(cube.position == position)
            {
                if (cube.bPushable)
                {
                    //evaluate whether it can be moved in its current position.
                    if (IsPushablePositionAvailable(cube.position + direction))
                    {
                        MovePushableCube(cube.position,direction);
                        return true;
                    }
                }
                return false;
            }
        }
        //checks floor
        foreach (CubeData cube in cubeInfo.Values)
        {
            if (cube.position + Vector3.up == position)
                return true;
        }
        return false;
    }
    //used by pushable block to determine if it should fall
    public static bool FloorExist(Vector3 position)
    {
        foreach (CubeData cube in cubeInfo.Values)
        {
            if (cube.position + Vector3.up == position)
                return true;
        }
        return false;
    }
    //used by pushable block internally
    private static bool IsPushablePositionAvailable(Vector3 pos)
    {
        foreach(CubeData cube in cubeInfo.Values)
        {
            if (cube.position == pos)
                return false;
        }
        return true;
    }
}