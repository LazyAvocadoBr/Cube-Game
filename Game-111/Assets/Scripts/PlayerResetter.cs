using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResetter : MonoBehaviour
{
    [SerializeField]
    private List<GameActions> gameActions;
    [SerializeField]
    private Transform piecesParent;

    private void OnEnable()
    {
        CuboidMaster.DelResetPlayerDefaults += ResetDefaults;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetPlayerDefaults -= ResetDefaults;
    }
    private void ResetDefaults()
    {
        foreach (GameActions item in gameActions)
            item.ResetToDefaults();
        piecesParent.rotation = Quaternion.identity;
    }
}
