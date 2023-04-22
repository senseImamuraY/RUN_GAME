using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
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
    private bool isGround = false;
    public bool IsGround { get { return isGround; } }

    private bool isColliding = false;
    public bool GetIsColliding { get { return isColliding; } }

    public void SetColliding(bool value) { isColliding = value; }

    [SerializeField]
    private Vector3 center = Vector3.zero;
    //public Vector3 GetAndSetCenter { get { return center; } set { center = value; } }  
    public Vector3 GetCenter { get { return center; } }

    public void SetCenter(Vector3 value) { center = value; }

    [SerializeField]
    private Vector3 size = Vector3.one;
    public Vector3 GetSize { get { return size; } }

    private Transform cubeTransform;

    private void Awake()
    {
        cubeTransform = transform;
        SetCenter(this.transform.position);
    }
    public bool CheckCollisionWithCube(ICube cube)
    {
        //Debug.Log("isColliding = " + isColliding);
        Vector3 v3SubAbs = this.GetCenter - cube.GetCenter;
        v3SubAbs = new Vector3(Mathf.Abs(v3SubAbs.x), Mathf.Abs(v3SubAbs.y), Mathf.Abs(v3SubAbs.z));
        Vector3 v3AddScale = (this.GetSize + cube.GetSize) / 2.0f;
        if ((v3SubAbs.x < v3AddScale.x) &&
            (v3SubAbs.y < v3AddScale.y) &&
            (v3SubAbs.z < v3AddScale.z))
        {
            cube.SetColliding(true);
            return true;
        }
        else
        {
            cube.SetColliding(false);
            return false;
        }
    }

    public bool CheckCollisionWithSphere(ISphere sphere)
    {
        var cubeToLocal = Matrix4x4.TRS(center, Quaternion.identity, Vector3.one);
        var worldToCube = cubeToLocal.inverse * transform.worldToLocalMatrix;

        // Cubeの空間における球の中心点
        var sphereCenter = worldToCube.MultiplyPoint(sphere.GetWorldCenter);

        // 距離の二乗を求める
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

        // 距離の二乗が0だったらCubeの内部にSphereの中心があるということ
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

    public bool CheckCollisionWithPlane(IPlane plane)
    {
        throw new System.NotImplementedException();
    }
}

