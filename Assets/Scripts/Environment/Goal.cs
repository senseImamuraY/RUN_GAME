using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    Vector3 centerPos;

    CustomCubeCollider goal;

    [SerializeField]
    List<GameObject> confettiList;

    AudioSource audioSource;

    [SerializeField]
    AudioClip cracker, prise;

    void Start()
    {
        centerPos = transform.position;
        goal = GetComponent<CustomCubeCollider>();
        goal.SetCenter(centerPos);
        audioSource = GetComponent<AudioSource>();
    }

    public void GoalEffect(Player player)
    {
        if(goal.CheckCollisionWithCapsule(player.GetCustomCapsuleCollider()))
        {
            foreach (var confetti in confettiList)
            {
                confetti.SetActive(true);
            }
            player.Clear(centerPos + new Vector3(0, transform.localScale.y /2, 0));

            Invoke("PlayCracker", 0.2f);
            Invoke("PlayPrise", 1f);
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