using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class CustomPlaneCollider : MonoBehaviour , ICollider, IPlane
{

    //Vector3 GetPointOfPlane { get; }

    public float GetXSize { get { return xSize; } }

    public float GetZSize { get { return zSize; } }

    // 原点を通ることが前提条件になってしまっているため修正
    //Vector3 GetCenter { get; }

    [SerializeField]
    private float xSize = 5.0f;
    [SerializeField]
    private float zSize = 10.0f;
    private Quaternion planeRotation;
    Vector3 VectorX;
    Vector3 VectorZ;

    
    private Vector3 center;
    public Vector3 GetCenter()
    {
        return center;
    }

    public Vector3 Center() { return center; }

    //[SerializeField]
    //private Vector3 size = new Vector3(1,1,1);

    //public Vector3 Size() { return size; }

    void Awake()
    {
        center = transform.position;
    }
    void Start()
    {
        planeRotation = this.transform.rotation;
        VectorX = transform.TransformVector(new Vector3(transform.right.x * xSize, transform.position.y, 0f));
        VectorZ = transform.TransformVector(new Vector3(0f, transform.position.y, transform.forward.z * zSize));

    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public Vector3 getForward() 
    {
        return transform.forward;
    }

    //public void movePlayer(float num)
    //{
        
    //}

    public Vector3 getPlanePosition()
    {
        return this.transform.position;
    }
    

    Vector3 GetVectorX()
    {
        return this.transform.right.x * VectorX + center;
    }

    Vector3 GetVectorZ()
    {
        return this.transform.forward.z * VectorZ + center;
    }



    public Vector3 GetNormal()
    {
        Vector3 x = GetVectorX();
        Vector3 z = GetVectorZ();

        //return Vector3.Cross(x, z);
        return transform.up;
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

    public bool CheckCollisionWithSphere(ISphere sphere)
    {
        throw new System.NotImplementedException();
    }
}
