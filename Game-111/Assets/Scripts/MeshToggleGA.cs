using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshToggleGA : GameActions
{
    [SerializeField]
    [Tooltip("When true, DeAction shows and Action hides")]
    private bool bShow;
    [SerializeField]
    private List<MeshRenderer> meshRenderers;
    
    public override void Action()
    {
        if(bShow)
        {
            foreach (MeshRenderer item in meshRenderers)
                item.enabled = false;
            return;
        }
        foreach(MeshRenderer item in meshRenderers)
        {
            if(item.enabled) 
                item.enabled = false;
            else
                item.enabled = true;
        }
    }
    public override void DeAction()
    {
        if (bShow)
        {
            foreach (MeshRenderer item in meshRenderers)
                item.enabled = true;
            return;
        }
        foreach (MeshRenderer item in meshRenderers)
        {
            if (item.enabled)
                item.enabled = false;
            else
                item.enabled = true;
        }
    }
    public override void ResetToDefaults()
    {
        DeAction();
    }
    public override void CheckpointReset()
    {
        DeAction();
    }
}
