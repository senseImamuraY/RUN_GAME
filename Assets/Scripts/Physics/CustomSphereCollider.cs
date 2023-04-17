using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSphereCollider : MonoBehaviour, ICollider, ISphere
{
    public bool CheckCollisionWithSphere(ISphere target)
    {
        throw new System.NotImplementedException();
    }

    public bool CheckCollisionWithCapsule(ICapsule capsule)
    {
        throw new System.NotImplementedException();
    }

    public bool CheckCollisionWithBox(IBox box)
    {
        throw new System.NotImplementedException();
    }
}
