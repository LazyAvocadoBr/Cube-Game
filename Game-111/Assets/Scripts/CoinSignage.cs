using UnityEngine;
using TMPro;

public class CoinSignage : MonoBehaviour
{
    [SerializeField]
    private CoinRequirement cRequirement;
    [SerializeField]
    private TextMeshProUGUI tMesh;

    private int remainderCount;

    private void OnEnable()
    {
        CoinPickupGA.CoinCollected += UpdateRemainder;
        CuboidMaster.DelResetGame += ResetRequirement;
        CuboidMaster.DelCheckpointReset += ResetRequirement;
    }
    private void OnDisable()
    {
        CoinPickupGA.CoinCollected -= UpdateRemainder;
        CuboidMaster.DelResetGame -= ResetRequirement;
        CuboidMaster.DelCheckpointReset -= ResetRequirement;
    }
    private void UpdateRemainder()
    {
        remainderCount--;
        tMesh.text = remainderCount.ToString();
    }
    private void ResetRequirement()
    {
        Start();
    }
    private void Start()
    {
        remainderCount = cRequirement.Count;
        tMesh.text = remainderCount.ToString();
    }
}
