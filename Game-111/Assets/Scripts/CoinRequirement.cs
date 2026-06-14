using UnityEngine;

public class CoinRequirement : MonoBehaviour
{
    [SerializeField]
    private int coinRequirement;
    [SerializeField]
    private GameActionTrigger gaTrigger;
    private int coinCount;
    private void Start()
    {
        CoinPickupGA.CoinCollected += UpdateCointCount;
    }
    private void OnDisable()
    {
        CoinPickupGA.CoinCollected -= UpdateCointCount;
    }
    private void UpdateCointCount()
    {
        coinCount++;
        if(coinCount == coinRequirement)
            gaTrigger.TriggerActions();
    }
    private void ResetCoinCount()
    {
        coinCount = 0;
    }
    public int Count => coinRequirement;
}
