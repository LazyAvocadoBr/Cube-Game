using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CubeManager : MonoBehaviour
{    
    [SerializeField]
    private List<CubeData> cubeData = new List<CubeData>();
    private List<Vector3> coloredBlocksList = new List<Vector3>();
    private List<Vector3> validBlocks = new List<Vector3>();
    private bool bForward,bBack,bRight,bLeft;
    private List<Vector3> directions = new List<Vector3>() 
    {
        Vector3.forward,Vector3.back,Vector3.right,Vector3.left
    };
    private void Awake()
    {       
        cubeData.Clear();
        ChangeColor.ColorChanged += UpdateCubeData;
        ChangeColor.RegisterCube += RegisterCube;
    }
    private void OnDisable()
    {
        ChangeColor.ColorChanged -= UpdateCubeData;
        ChangeColor.RegisterCube -= RegisterCube;
    }
    private void RegisterCube(Vector3 position)
    {
        cubeData.Add(new CubeData(position));
    }
    public void UpdateCubeData(Vector3 position, bool bColored) //update based on position which aligns with 2D array index
    {
       for(int x = 0; x < cubeData.Count; x++) 
        {
            if (cubeData[x].position ==  position) 
            {
                cubeData[x] = new CubeData(position,true,false);
                break;
            }
        }
    }
    //recursive
    private bool Contiguous(Vector3 position)
    {
        validBlocks.Clear();
        //adds new block
        if (IsColored(position) && !PositionExist(position))
        {
            coloredBlocksList.Add(position);
        }

        //check contiguity
        foreach(Vector3 pos in coloredBlocksList) 
        {
            if (ColoredDirectionCheck(pos))
                validBlocks.Add(pos);          
        }
        //contiguous check of new list
        if (validBlocks.Count > 3)
            return true;
        else
            return false;
    }
    private bool IsColored(Vector3 position) 
    {
        for(int x = 0; x < cubeData.Count;x++)
        {
            if (cubeData[x].position == position && cubeData[x].bColored) return true;
        }
        return false;
    }
    private bool ColoredDirectionCheck(Vector3 position) // checks if nearby blocks are colored and is a minimum of 2
    {
        bForward = bBack = bRight = bLeft = false;

        for(int x = 0; x < directions.Count;x++) 
        {
            if(x == 0 && PositionExist(position + directions[x]))
            {
                bForward = true;
            }
            else if (x == 1 && PositionExist(position + directions[x]))
            {
                bBack = true;
            }
            else if (x == 2 && PositionExist(position + directions[x]))
            {
                bRight = true;
            }
            else if (x == 3 && PositionExist(position + directions[x]))
            {
                bLeft = true;
            }
        }

        if(bForward && bBack) return true;
        if(bRight && bLeft) return true;
        if(bForward && bRight) return true;
        if(bForward && bLeft) return true;
        if(bBack && bLeft) return true; 
        if(bBack && bRight) return true;
        else return false;
    }
    private bool PositionExist(Vector3 position)
    {
        foreach(Vector3 pos in coloredBlocksList)
        {
            if(pos == position) return true;
        }
        return false;
    }
}
[Serializable]
public struct CubeData
{
    public Vector3 position;
    public bool bColored;
    public bool bOccupied;
    public bool bPushable;
    public CubeData(Vector3 value)
    {
        position = value;
        bOccupied = bColored = bPushable = false;
    }
    public CubeData(Vector3 value, bool bColorValue,bool bOccupiedValue)
    {
        position = value;
        bOccupied = bOccupiedValue;
        bColored = bColorValue;
        bPushable = false;
    }
    public CubeData(Vector3 value, bool bColorValue, bool bOccupiedValue, bool bPush)
    {
        position = value;
        bOccupied = bOccupiedValue;
        bColored = bColorValue;
        bPushable = bPush;
    }
};