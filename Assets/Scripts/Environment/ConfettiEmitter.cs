using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiEmitter : MonoBehaviour
{
    [SerializeField]
    List<GameObject> confettiList;

    AudioSource audioSource;

    [SerializeField]
    AudioClip cracker, prise;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var confetti in confettiList)
            {
                confetti.SetActive(true);

                Invoke("PlayCracker", 0.2f);
                Invoke("PlayPrise", 1f);
            }
        }
    }

    private void PlayCracker()
    {
        audioSource.clip = cracker;
        audioSource.Play();
    }

    private void PlayPrise()
    {
        audioSource.clip = prise;
        audioSource.Play();
    }

}