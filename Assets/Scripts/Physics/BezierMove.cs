using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMove : MonoBehaviour
{
    public Transform point0, point1, point2, point3;
    public float speed = 1.0f;
    private float t = 0.0f;

    [SerializeField]
    private bool randomizePositions = false;

    [SerializeField]
    private Vector3 randomPositionRange = new Vector3(1f, 1f, 1f);

    private void Start()
    {
        if (randomizePositions)
        {
            RandomizePointsPositions();
        }
    }

    void Update()
    {
        t += Time.deltaTime * speed;

        if (t > 1.0f)
        {
            t = 0.0f;
        }

        transform.position = CalculateBezierCurve(t, point0.position, point1.position, point2.position, point3.position);
    }

    public static Vector3 CalculateBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 B = new Vector3();
        B = uuu * p0; //point 0 influence
        B += 3 * uu * t * p1; //point 1 influence
        B += 3 * u * tt * p2; //point 2 influence
        B += ttt * p3; //point 3 influence

        return B;
    }

    private void RandomizePointsPositions()
    {
        point0.localPosition = RandomizePosition(point0.localPosition);
        point1.localPosition = RandomizePosition(point1.localPosition);
        point2.localPosition = RandomizePosition(point2.localPosition);
        point3.localPosition = RandomizePosition(point3.localPosition);
    }

    private Vector3 RandomizePosition(Vector3 position)
    {
        return position + new Vector3(
            Random.Range(-randomPositionRange.x, randomPositionRange.x),
            Random.Range(-randomPositionRange.y, randomPositionRange.y),
            Random.Range(-randomPositionRange.z, randomPositionRange.z)
        );
    }
}
