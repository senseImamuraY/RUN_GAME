using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CustomCubeCollider : MonoBehaviour, ICollider, ICube
{
    //[SerializeField]
    //private float width, height;
    //public float GetWidth { get { return width; } }
    //public float GetHeight { get { return height; } }
    //public float GetHalfWidth { get { return width * 0.5f; } }
    //public float GetHalfHeight { get { return height * 0.5f; } }

    [SerializeField]
    private Vector3 center = Vector3.zero;
    public Vector3 GetCenter { get { return center; } }
    [SerializeField]
    private Vector3 size = Vector3.one;
    public Vector2 GetSize { get { return size; } }

    private Transform cubeTransform;

    private void Awake()
    {
        cubeTransform = transform;
    }
    public bool CheckCollisionWithCube(ICube t)
    {
        throw new System.NotImplementedException();
    }

    public bool CheckCollisionWithSphere(ISphere t)
    {
        throw new System.NotImplementedException();
    }

    public bool CheckCollisionWithCapsule(ICapsule t)
    {
        throw new System.NotImplementedException();
    }
}

