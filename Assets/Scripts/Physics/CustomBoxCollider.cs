using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CustomBoxCollider : MonoBehaviour, ICollider, IBox
{
    [SerializeField]
    private float width, height;
    public float GetWidth { get { return width; } }
    public float GetHeight { get { return height; } }
    public float GetHalfWidth { get { return width * 0.5f; } }
    public float GetHalfHeight { get { return height * 0.5f; } }
    public bool CheckCollisionWithBox(IBox t)
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

