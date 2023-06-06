using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    Vector3 centerPos;

    CustomCubeCollider goal;

    [SerializeField]
    List<GameObject> confettiList;

    void Start()
    {
        centerPos = transform.position;
        //centerPos = new Vector3(transform.position.x, 0, transform.position.z);
        goal = GetComponent<CustomCubeCollider>();
        goal.SetCenter(centerPos);
        Debug.Log(centerPos);
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
        }
    }
}