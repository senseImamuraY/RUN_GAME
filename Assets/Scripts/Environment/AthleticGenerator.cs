using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AthleticGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> AthleticList;

    [SerializeField]
    private List<Transform> spawnPositionList;

    [SerializeField]
    private Transform parent;
    private void Awake()
    {
        int i = 0;
        while (i < 3)
        {
            if (AthleticList != null && AthleticList.Count > 0)
            {
                GameObject randomElement = AthleticList.RandomElement();
                Instantiate(randomElement, spawnPositionList[i].position, spawnPositionList[i].rotation, parent);
            }

            i++;
        }

    }
}


// ListÇÃóvëfÇ©ÇÁíºê⁄
public static class RandomListElement
{
    private static readonly System.Random random = new System.Random();
    
    public static T RandomElement<T>(this List<T> list)
    {
        int index = random.Next(0, list.Count);
        return list[index];
    }
}