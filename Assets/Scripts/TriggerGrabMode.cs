using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGrabMode : MonoBehaviour
{
    public Backpack.GrabMode whichGrabMode;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Controller"))
        {
            if(whichGrabMode == Backpack.GrabMode.Marshmallow)
            {
                Backpack.Instance.SetGrabModeMarshmallow();
            }
            else if(whichGrabMode == Backpack.GrabMode.Kindling)
            {
                Backpack.Instance.SetGrabModeKindling();
            }
        }
    }
}
