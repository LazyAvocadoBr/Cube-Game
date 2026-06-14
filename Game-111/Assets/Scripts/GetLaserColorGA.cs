using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLaserColorGA : GameActions
{
    [SerializeField]
    private LaserBeam laserBeam;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out LaserBeam beam))
        {
            laserBeam.SetBeam(beam.LaserType);
        }
    }
}
