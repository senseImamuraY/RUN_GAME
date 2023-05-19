using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CustomSphereCollider : MonoBehaviour, ICollider, ISphere, IBounds
{
    [SerializeField]
    private Vector3 center;
    public Vector3 GetCenter { get { return center; } }
    [SerializeField]
    private float radius = 0.5f;
    public float GetRadius { get { return radius; } }

    public Vector3 GetWorldCenter { get { return sphereTransform + center; } }

    private Vector3 sphereTransform;

    public Vector3 Center() { return center; }

    [SerializeField]
    private Vector3 size;

    public Vector3 Size() { return size; }

    public void Enter()
    {


    }
    // sphere‚ÌˆÊ’u‚ð“ü‚ê‚é

    void Start()
    {
        SetPosition();
    }

    void Update()
    {
        center = transform.position;
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
