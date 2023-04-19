using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    //public Vector3 GetAndSetCenter { get { return center; } set { center = value; } }  
    public Vector3 GetCenter { get { return center; } }

    public void SetCenter(Vector3 value) { center = value; }

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

    public bool CheckCollisionWithSphere(ISphere sphere)
    {
        var cubeToLocal = Matrix4x4.TRS(center, Quaternion.identity, Vector3.one);
        var worldToCube = cubeToLocal.inverse * transform.worldToLocalMatrix;

        // Cube‚Ì‹óŠÔ‚É‚¨‚¯‚é‹…‚Ì’†S“_
        var sphereCenter = worldToCube.MultiplyPoint(sphere.GetWorldCenter);

        // ‹——£‚Ì“ñæ‚ğ‹‚ß‚é
        var sqLength = 0.0f;
        for (int i = 0; i < 3; i++)
        {
            var point = sphereCenter[i];
            var boxMin = size[i] * -0.5f;
            var boxMax = size[i] * 0.5f;
            if (point < boxMin)
            {
                sqLength += (point - boxMin) * (point - boxMin);
            }
            if (point > boxMax)
            {
                sqLength += (point - boxMax) * (point - boxMax);
            }
        }

        // ‹——£‚Ì“ñæ‚ª0‚¾‚Á‚½‚çCube‚Ì“à•”‚ÉSphere‚Ì’†S‚ª‚ ‚é‚Æ‚¢‚¤‚±‚Æ
        if (sqLength == 0.0f)
        {
            return true;
        }

        return sqLength <= sphere.GetRadius * sphere.GetRadius;
    }

    public bool CheckCollisionWithCapsule(ICapsule t)
    {
        throw new System.NotImplementedException();
    }
}

