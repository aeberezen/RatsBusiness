using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlagManager : MonoBehaviour, IInteractable
{
    public GameObject pin;

    bool IInteractable.Interact()
    {
        Debug.Log("Use the Flag?");
        return true;
    }

    public void ChangeColor()
    {
        pin.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
    }
}
