using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LaserAtteributes/LaserDatabase")]
public class LaserProperties : ScriptableObject
{
   public List<LaserAttributes> laserVariants;
}
[Serializable]
public struct LaserAttributes
{
    public string name;
    public LayerMask mask;
    public LaserType type;
    public Material material;
};
public enum LaserType { Red,Black,White}