using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinCountText;
    private int coinCount;
    private void OnEnable()
    {
        CuboidMaster.DelResetGame += Reset;
        CuboidMaster.DelCheckpointReset += Reset;
        CoinPickupGA.CoinCollected += UpdateCoinCount;
    }
    private void OnDisable()
    {
        CoinPickupGA.CoinCollected -= UpdateCoinCount;
        CuboidMaster.DelCheckpointReset -= Reset;
        CuboidMaster.DelResetGame -= Reset;
    }
    private void UpdateCoinCount()
    {
        coinCount++;
        coinCountText.text = coinCount.ToString();
    }
    private void Reset()
    {
        coinCount = 0;
        coinCountText.text = coinCount.ToString();
    }
}
