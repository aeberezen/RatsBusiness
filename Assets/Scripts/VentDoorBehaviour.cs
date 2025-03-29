using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentDoorBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float angle;
    public bool isClosed;

    AudioSource ventAudio;
    //private Transform vent;
    void Start()
    {
        //vent = GetComponent<Transform>();
        isClosed = true;
        ventAudio = this.GetComponent<AudioSource>();
    }
    public void SetOpen()
    {
        isClosed = false;
        ventAudio.Pause();
    }
    public void SetClosed()
    {
        isClosed = true;
        ventAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Euler(angle * Time.deltaTime, -90f, 0f);
        if (isClosed)
        {
            transform.Rotate(angle * Time.deltaTime, 0, 0);
        }
    }
}
