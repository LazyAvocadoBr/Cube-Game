using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalToggle : GameActions
{
    [SerializeField]
    private DecalProjector dProjector;
    [SerializeField]
    [Tooltip("When enabled, disables projector on Action")]
    private bool bDisable;

    public override void Action()
    {
        if(bDisable)
            dProjector.enabled = false;
        else
        {
            if (dProjector.enabled)
                dProjector.enabled = false;
            else 
                dProjector.enabled = true;
        }
    }
    public override void DeAction()
    {
        if (bDisable)
            dProjector.enabled = true;
        else
        {
            if (dProjector.enabled)
                dProjector.enabled = false;
            else
                dProjector.enabled = true;
        }
    }
    public override void ResetToDefaults()
    {
        DeAction();
    }
}
