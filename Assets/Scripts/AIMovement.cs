using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    private Transform pos;
    private NavMeshAgent agent;
    public GameObject startPoint;
    public GameObject tag;
    public Transform startPointWarp;
    private bool destinationWasSet;

    // Start is called before the first frame update
    void Start()
    {
        destinationWasSet = false;
        agent = GetComponent<NavMeshAgent>();
        pos = GetComponent<Transform>();
        Debug.Log("STARTING POSITION:" + startPointWarp.position);
    }

    // Update is called once per frame
    void Update()
    {
        //goes when triggered by Player to destination
        if (GetComponent<MessageManager>().isTriggered && !destinationWasSet)
        {
            agent.destination = target.position;
            destinationWasSet = true;
            tag.SetActive(true);
        }

        //when reached destination - Done and stops steps audio
        if (pos.position.x == target.position.x && pos.position.z == target.position.z && !GetComponent<MessageManager>().done)
        {
            //GetComponent<MessageManager>().done = true;
            GetComponent<MessageManager>().Success();
        }
    }

}
