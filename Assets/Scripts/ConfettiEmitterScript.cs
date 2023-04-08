using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiEmitterScript : MonoBehaviour
{
    [SerializeField]
    List<GameObject> confettiList;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var confetti in confettiList)
            {
                confetti.SetActive(true);
                Debug.Log("OK");
            }
        }
    }
}