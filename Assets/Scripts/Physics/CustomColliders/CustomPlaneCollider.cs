using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class CustomPlaneCollider : MonoBehaviour , ICollider, IPlane
{
    public float GetXSize { get { return xSize; } }

    public float GetZSize { get { return zSize; } }

    [SerializeField]
    private float xSize = 5.0f;

    [SerializeField]
    private float zSize = 10.0f;

    private Vector3 center;
    public Vector3 GetCenter()
    {
        return center;
    }

    void Awake()
    {
        center = transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public Vector3 getForward() 
    {
        return transform.forward;
    }

    public Vector3 GetNormal()
    {
        return transform.up;
    }
    
    // ïKóvÇ…Ç»Ç¡ÇΩç€Ç…í«â¡
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

    public bool CheckCollisionWithSphere(ISphere sphere)
    {
        // TODO: Implement this method properly.
        return false;
    }
}
