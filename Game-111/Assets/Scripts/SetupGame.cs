using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGame : MonoBehaviour
{
    public GameObject playerCamera;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("MainCamera"))
            return;
        else 
            Instantiate(playerCamera);
    }
}
