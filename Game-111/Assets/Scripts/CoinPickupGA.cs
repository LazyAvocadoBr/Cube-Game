using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickupGA : GameActions
{
    public static Action CoinCollected = delegate { };

    public override void Action()
    {
        CoinCollected();
    }
}
