using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class CustomPlaneCollider : MonoBehaviour , ICollider, IPlane
{

    //Vector3 GetPointOfPlane { get; }

    public float GetXSize { get { return xSize; } }

    public float GetZSize { get { return zSize; } }

    // å¥ì_Çí ÇÈÇ±Ç∆Ç™ëOíÒèåèÇ…Ç»Ç¡ÇƒÇµÇ‹Ç¡ÇƒÇ¢ÇÈÇΩÇﬂèCê≥
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

    void Start()
    {
        planeRotation = this.transform.rotation;
        center = transform.position;
        //VectorX = new Vector3(xSize, transform.position.y, 0f);
        //VectorZ = new Vector3(0f, transform.position.y, zSize);
        VectorX = transform.TransformVector(new Vector3(xSize, transform.position.y, 0f));
        VectorZ = transform.TransformVector(new Vector3(0f, transform.position.y, zSize));
    }

    public Vector3 getPlanePosition()
    {
        return this.transform.position;
    }
    

    Vector3 GetVectorX()
    {
        return VectorX + center;
    }

    Vector3 GetVectorZ()
    {
        return VectorZ + center;
    }



    public Vector3 GetNormal()
    {
        Vector3 x = GetVectorX();
        Vector3 z = GetVectorZ();

        return Vector3.Cross(x, z);
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
