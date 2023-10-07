using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CustomSphereCollider : MonoBehaviour, ICollider, ISphere
{
    [SerializeField]
    private Vector3 center = Vector3.zero;

    public Vector3 GetCenter { get { return center; } }

    [SerializeField]
    private float radius = 0.5f;

    public float GetRadius { get { return radius; } }

    public Vector3 GetWorldCenter { get { return sphereTransform.position + adjustmentCenter; } }

    private Transform sphereTransform;

    // EnemyÉNÉâÉXÇ÷ÇÃéQè∆
    public IEnemy Enemy { get; set; }
    public IItem Item { get; set; }
    public bool IsColliding { get; set; } = false;

    [SerializeField]
    Vector3 adjustmentCenter = new Vector3(0, 0, 0);

    void Awake()
    {
        sphereTransform = GetComponent<Transform>();
    }
    void Start()
    {
        center = transform.position;
    }

    void Update()
    {
        center = transform.position;
    }

    public bool CheckCollisionWithSphere(ISphere sphere)
    {
        var collideDistance = GetRadius + sphere.GetRadius;
        return (GetWorldCenter - sphere.GetWorldCenter).sqrMagnitude <= collideDistance * collideDistance;
    }

    public bool CheckCollisionWithCapsule(ICapsule capsule)
    {
        // TODO: Implement this method properly.
        return false;
    }

    public bool CheckCollisionWithCube(ICube box)
    {
        // TODO: Implement this method properly.
        return false;
    }

    public bool CheckCollisionWithPlane(IPlane plane)
    {
        // TODO: Implement this method properly.
        return false;
    }
}
