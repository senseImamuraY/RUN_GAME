using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    Vector3 centerPos;

    void Start()
    {
        centerPos = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerScript>().Clear(centerPos);
        }
    }
}