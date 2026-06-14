using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAwayGA : GameActions
{
    [SerializeField]
    private List<Transform> tObjects;

    private bool bActive;

    public override void Action()
    {
        if (bActive) return;
        bActive = true;
        StartCoroutine(nameof(Shrink));
    }
    public override void DeAction()
    {
        ResetToDefaults();
    }
    public override void ResetToDefaults()
    {
        foreach(Transform item in tObjects)
        {
            //item.gameObject.SetActive(true); // I am not sure why I did this
            item.localScale = new Vector3(1, 1, 1);
        }
        bActive = false;
    }
    IEnumerator Shrink()
    {
        //make kinematic
        foreach (Transform item in tObjects)
            item.GetComponent<Rigidbody>().isKinematic = true;
        float rate = 0;
        while(bActive)
        {
            rate += Time.deltaTime;

            foreach (Transform item in tObjects)
                item.localScale = Vector3.Lerp(new Vector3(1, 1, 1), Vector3.zero, rate);
            yield return new WaitForEndOfFrame();
            if(rate >= 1)
            {
                foreach (Transform item in tObjects)
                {
                    item.gameObject.SetActive(false);
                    item.localScale = new Vector3(1, 1, 1);
                }
                bActive = false;
            }
        }
    }
}
