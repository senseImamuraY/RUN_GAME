using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSphereCollider : MonoBehaviour, ICollider, ISphere
{
    [SerializeField]
    private Vector3 center = Vector3.zero;
    public Vector3 GetCenter { get { return center; } }
    [SerializeField]
    private float radius = 0.5f;
    public float GetRadius { get { return radius; } }

    public Vector3 GetWorldCenter { get { return sphereTransform + center; } }

    private Vector3 sphereTransform;

    public void Enter()
    {


    }
    // sphere‚ÌˆÊ’u‚ð“ü‚ê‚é

    void Start()
    {
        SetPosition();
    }
    public void SetPosition()
    {
        sphereTransform = this.transform.position;
    }
    public bool CheckCollisionWithSphere(ISphere sphere)
    {
        var collideDistance = GetRadius + sphere.GetRadius;
        return (GetWorldCenter - sphere.GetWorldCenter).sqrMagnitude <= collideDistance * collideDistance;
    }

    public bool CheckCollisionWithCapsule(ICapsule capsule)
    {
        throw new System.NotImplementedException();
    }

    public bool CheckCollisionWithCube(ICube box)
    {
        throw new System.NotImplementedException();
    }

    public bool CheckCollisionWithPlane(IPlane plane)
    {
        throw new System.NotImplementedException();
    }
}
