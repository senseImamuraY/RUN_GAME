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
        //centerPos = new Vector3(transform.position.x, 0, transform.position.z);
        goal = GetComponent<CustomCubeCollider>();
        goal.SetCenter(centerPos);
        //Debug.Log(centerPos);
        audioSource = GetComponent<AudioSource>();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        other.GetComponent<BonusPlayer>().Clear(centerPos);
    //    }
    //}

    public void GoalEffect(Player player)
    {
        if(goal.CheckCollisionWithCapsule(player.GetCustomCapsuleCollider()))
        {
            foreach (var confetti in confettiList)
            {
                confetti.SetActive(true);
            }
            player.Clear(centerPos + new Vector3(0, transform.localScale.y /2, 0));
            Debug.Log("clear!");

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