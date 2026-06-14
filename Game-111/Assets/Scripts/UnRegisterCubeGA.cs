using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnRegisterCubeGA : GameActions
{
    [SerializeField]
    private RegisterCube registerCube;

    public override void Action()
    {
        registerCube.RemoveCube();
    }
    public override void DeAction()
    {
        registerCube.AddCube();
    }
}
