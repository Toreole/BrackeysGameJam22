using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePathMover : MonoBehaviour
{
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private float endWait = 1f;
    [SerializeField]
    private float moveSpeed = 6f;

    private Vector3[] positions;
    private int currentIndex = 0;

    private float arrivalTime;
    private bool wait;
    private int direction = 1;

    private void Start()
    {
        if (wayPoints.Length <= 1)
        {
            Debug.LogWarning("SimplePathMover requires more than one point.", this);
            this.enabled = false;
        }

        positions = new Vector3[wayPoints.Length];
        for (int i = 0; i < wayPoints.Length; i++)
            positions[i] = wayPoints[i].position;
        transform.position = positions[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(wait)
        {
            if (Time.time - arrivalTime <= endWait)
                wait = false;
            return;
        }

        Vector2 targetPos = positions[currentIndex];
        Vector2 nextPos = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.position = nextPos;
        if(nextPos == targetPos)
        {
            currentIndex += direction;
            if(currentIndex >= positions.Length)
            {
                Wait();
                currentIndex = positions.Length - 2;
            } 
            else if (currentIndex < 0)
            {
                Wait();
                currentIndex = 1;
            }
        }
    }

    private void Wait()
    {
        wait = true;
        direction = -direction;
        arrivalTime = Time.time;
    }
}
