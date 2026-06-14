using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCubeGA : GameActions
{
    [SerializeField]
    private List<Collider> colliders;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private List<GameActions> actionsToReset;
    [SerializeField]
    private List<GameObject> additionalObjects;
    public override void Action()
    {
        foreach (Collider item in colliders)
            item.enabled = false;
        meshRenderer.enabled = false;
        foreach(GameObject item in additionalObjects)
            item.SetActive(false);
    }
    public override void DeAction()
    {
        foreach (Collider item in colliders)
            item.enabled = true;
        meshRenderer.enabled = true;
        foreach (GameObject item in additionalObjects)
            item.SetActive(true);
        foreach (GameActions item in actionsToReset)
            item.DeAction();
    }
}
