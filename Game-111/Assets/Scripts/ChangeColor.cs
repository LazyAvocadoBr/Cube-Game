using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeColor : MonoBehaviour
{
    [SerializeField]
    private bool bPermanent = true;
    [SerializeField]
    private float colorChangeTime = 3;
    [SerializeField]
    private Color color;
    [SerializeField]
    private MeshRenderer mRenderer;
    [SerializeField]
    private Transform parent;
    private static Color baseColor = new Color(0.75f,0.75f,0.75f,1);
    private bool bColorChanged;
    public static Action<Vector3,bool> ColorChanged = delegate { };
    public static Action<Vector3> RegisterCube = delegate { };

    private void Start()
    {
        RegisterCube(parent.position);
    }
    private void OnEnable()
    {
        CuboidMaster.DelResetGame += ResetColor;
        CuboidMaster.DelCheckpointReset += ResetColor;       
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetColor;
        CuboidMaster.DelCheckpointReset -= ResetColor;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (bColorChanged) return;
        mRenderer.material.color = color;
        ColorChanged(parent.position, bColorChanged);
        if (bPermanent) return;
        StartCoroutine(nameof(RevertColor));
    }
    private void ResetColor()
    {        
        mRenderer.material.color = baseColor;
        bColorChanged = false;
        ColorChanged(parent.position, bColorChanged);
    }

    IEnumerator RevertColor()
    {
        bColorChanged = true;
        yield return new WaitForSeconds(colorChangeTime);
        mRenderer.material.color = baseColor;
        bColorChanged = false;
        ColorChanged(parent.position, bColorChanged);
    }
}
