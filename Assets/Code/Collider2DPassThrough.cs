using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider2DPassThrough : MonoBehaviour
{
    [SerializeField]
    private GameObject passTarget;

    private void OnCollisionEnter2D(Collision2D c)
    {
        passTarget.SendMessage("OnCollisionEnter2D", c);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        passTarget.SendMessage("OnTriggerEnter2D", c);
    }

}
