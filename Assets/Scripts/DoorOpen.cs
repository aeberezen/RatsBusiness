using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] public VentDoorBehaviour[] vent;
    [SerializeField] public GameObject obstacle;
    [SerializeField] public GameObject particle;
    public SoundManager SoundManager;
    public Material buttonPressed;
    public Material buttonReleased;

    public AudioClip buttonAudioClip;

    public bool isPressed = false;
    public bool isOpen = false;

    public bool stay;
    public bool isColliding;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
        isOpen = false;
        stay = false;
        isColliding = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (stay)
        {
            isColliding = true;
            stay = false;
        }
        else
        {
            isColliding = false;
        }
    }
    void Update()
    {
        if (isPressed && !isColliding)
        {
            //Debug.Log("Player LEFT THE BUTTONI!");
            OnTriggerExitREPLACEMENT();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player TOUCHED THE BUTTONI!");
            foreach (VentDoorBehaviour i in vent)
            {
                i.SetOpen();
            }
            obstacle.SetActive(false);

            if (particle != null)
            {
                particle.SetActive(false);
            }

            GetComponent<MeshRenderer>().material = buttonPressed;

            if (!isPressed)
            {
                SoundManager.Play("Button", 0f);
                isPressed = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stay = true;
        }
    }

    //EXIT doesn't work, made with OnTriggerStay
    private void OnTriggerExitREPLACEMENT()
    {
        foreach (VentDoorBehaviour i in vent)
        {
            //for multiplayer
            //i.SetClosed();
        }
        //obstacle.SetActive(true);
        //GetComponent<MeshRenderer>().material = buttonReleased;

        if (isPressed)
        {
            //SoundManager.PlaySound(buttonAudioClip, 1);
            isPressed = false;
        }
    }
}