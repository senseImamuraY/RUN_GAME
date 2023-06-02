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
    //public Vector3 GetWorldCenter { get { return sphereTransform ; } }

    private Transform sphereTransform;

    // EnemyÉNÉâÉXÇ÷ÇÃéQè∆
    public IEnemy Enemy { get; set; }

    public IItem Item { get; set; }
    public bool IsColliding { get; set; } = false;

    public Vector3 Center() { return center; }

    [SerializeField]
    private Vector3 size;

    public Vector3 Size() { return size; }

    [SerializeField]
    Vector3 adjustmentCenter = new Vector3(0, 0, 0);

    void Awake()
    {
        sphereTransform = GetComponent<Transform>();
    }
    void Start()
    {
        //SetPosition();
        center = transform.position;
    }

    void Update()
    {
        center = transform.position;
    }

    //public void SetPosition()
    //{
    //    sphereTransform = this.transform.position;
    //}
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

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(0.2f, 0.5f, 1.0f);
    //    Gizmos.matrix = transform.localToWorldMatrix;
    //    Gizmos.DrawWireSphere(Vector3.zero, radius);
    //}


}
