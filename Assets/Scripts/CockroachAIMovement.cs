using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class CockroachAIMovement : MonoBehaviour
{
    [Header("AI setup (EXACTLY 3)")]
    public Transform[] points;
    public bool[] fromPoint0;
    public bool[] fromPoint1;
    public bool[] fromPoint2;


    public Animator animator;

    private Transform pos;
    public int i;
    public int j;

    public bool onTheWay;
    private NavMeshAgent agent;
    public Random rand = new Random();
    public float waitPause;

    void Start()
    {
        i = -1;
        j = rand.Next(0, 2);
        onTheWay = false;
        agent = GetComponent<NavMeshAgent>();
        pos = GetComponent<Transform>();
        FindStartingPoint();
    }

    void Update()
    {
        foreach (Transform t in points)
        {
            if(pos.position.x == t.position.x && pos.position.z == t.position.z && onTheWay)
            {
                Debug.Log("COCK REACHED DESTINATION");
                animator.SetBool("IsWalking", false);
                WaitPause();
                onTheWay = false;
            }
        }

        if(i != -1 && !onTheWay)
        {
            ChooseDestination();
        }
    }

    private void ChooseDestination()
    {
        if (i == 0)
        {
            j = rand.Next(0, 3);
            while (fromPoint0[j] != true)
            {
                Debug.Log("i in 0");
                j = rand.Next(0, 3);
            }
        }
        if (i == 1)
        {
            j = rand.Next(0, 3);
            while (fromPoint1[j] != true)
            {
                Debug.Log("i in 1");
                j = rand.Next(0, 3);
            }
        }
        if (i == 2)
        {
            j = rand.Next(0, 3);
            while (fromPoint2[j] != true)
            {
                Debug.Log("i in 2");
                j = rand.Next(0, 3);
            }
        }
        if (!onTheWay)
        {
            agent.destination = points[j].position;
            animator.SetBool("IsWalking", true);
            onTheWay = true;
            i = j;
        }
    }

    private void FindStartingPoint()
    {
        if (pos.position.x == points[0].position.x && pos.position.z == points[0].position.z)
        {
            i = 0;
        }

        if (pos.position.x == points[1].position.x && pos.position.z == points[1].position.z)
        {
            i = 1;
        }

        if (pos.position.x == points[2].position.x && pos.position.z == points[2].position.z)
        {
            i = 2;
        }

        if (i == -1)
        {
            Debug.Log("Start pos has to be one of the points pos");
        }
    }

    public IEnumerator WaitPause()
    {
        yield return new WaitForSeconds(waitPause);
    }
}
