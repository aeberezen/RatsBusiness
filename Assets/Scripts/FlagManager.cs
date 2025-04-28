using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlagManager : MonoBehaviour, IInteractable
{
    public GameObject pin;

    //TODO: only 3 flags max
    bool IInteractable.Interact()
    {
        return true;
    }

    public void ChangeColor()
    {
        pin.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
    }

    //???
    bool IInteractable.getIsUsed()
    {
        return true;
    }


}
