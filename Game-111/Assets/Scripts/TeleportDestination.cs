using UnityEngine;

public class TeleportDestination : MonoBehaviour
{
    [SerializeField]
    private TeleportDestination destination;

    private void OnValidate()
    {
        Start();
    }

    private void Start()
    {
        if(destination)
            gameObject.GetComponentInChildren<TeleporterBlockGA>().SetDestination(destination.GetTeleporter);
    }
    public TeleporterBlockGA GetTeleporter => gameObject.GetComponentInChildren<TeleporterBlockGA>();   
}
