using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactionGA : GameActions
{
    [SerializeField]
    private GameActions action;
    public override void Action()
    {
        action.DeAction();
    }
}
