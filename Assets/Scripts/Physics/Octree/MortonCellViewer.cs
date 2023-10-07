using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortonCellViewer : MonoBehaviour
{
    #region Variables
    private float left;
    public float Left
    {
        get
        {
            return left;
        }
        set
        {
            left = value;
            width = right - left;
            UpdateCells();
        }
    }

    private float right;
    public float Right
    {
        get
        {
            return right;
        }
        set
        {
            right = value;
            width = right - left;
            UpdateCells();
        }
    }

    private float top;
    public float Top
    {
        get
        {
            return top;
        }
        set
        {
            top = value;
            height = top - bottom;
            UpdateCells();
        }
    }

    private float bottom;
    public float Bottom
    {
        get
        {
            return bottom;
        }
        set
        {
            bottom = value;
            height = top - bottom;
            UpdateCells();
        }
    }

    private float front;
    public float Front
    {
        get
        {
            return front;
        }
        set
        {
            front = value;
            depth = back - front;
            UpdateCells();
        }
    }

    private float back;
    public float Back
    {
        get
        {
            return back;
        }
        set
        {
            back = value;
            depth = back - front;
            UpdateCells();
        }
    }

    private float width;
    private float height;
    private float depth;

    private int division;
    public int Division
    {
        get
        {
            return division;
        }
        set
        {
            division = value;
            UpdateCells();
        }
    }

    private float unitWidth;
    private float unitHeight;
    private float unitDepth;
    private int halfDivision;

    private Color normalColor = new Color(1f, 0, 0, 0.5f);
    private Color centerColor = new Color(0, 0, 1f, 1f);
    #endregion Variables

    void Start()
    {
        UpdateCells();
    }

    void UpdateCells()
    {
        // ひとつの区間の単位
        unitWidth = width / Division;
        unitHeight = height / Division;
        unitDepth = depth / Division;

        halfDivision = Division / 2;
    }

    /// <summary>
    /// On draw gizomos.
    /// </summary>
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Vector3 tow = transform.right * width;
        Vector3 toh = transform.up * height;
        Vector3 tod = transform.forward * depth;

        // XY平面
        for (int i = 0; i <= Division; i++)
        {
            for (int j = 0; j <= Division; j++)
            {
                bool isCenter = (i == halfDivision || j == halfDivision);
                Gizmos.color = isCenter ? centerColor : normalColor;

                Vector3 offset = (transform.right * (unitWidth * i + left)) + (transform.up * (unitHeight * j + bottom)) + (transform.forward * front);
                Vector3 from = transform.position + offset;
                Vector3 to = from + tod;
                Gizmos.DrawLine(from, to);
            }
        }

        // YZ平面
        for (int i = 0; i <= Division; i++)
        {
            for (int j = 0; j <= Division; j++)
            {
                bool isCenter = (i == halfDivision || j == halfDivision);
                Gizmos.color = isCenter ? centerColor : normalColor;

                Vector3 offset = (transform.forward * (unitDepth * i + front)) + (transform.up * (unitHeight * j + bottom)) + (transform.right * left);
                Vector3 from = transform.position + offset;
                Vector3 to = from + tow;
                Gizmos.DrawLine(from, to);
            }
        }

        // XZ平面
        for (int i = 0; i <= Division; i++)
        {
            for (int j = 0; j <= Division; j++)
            {
                bool isCenter = (i == halfDivision || j == halfDivision);
                Gizmos.color = isCenter ? centerColor : normalColor;

                Vector3 offset = (transform.forward * (unitDepth * i + front)) + (transform.right * (unitWidth * j + left)) + (transform.up * bottom);
                Vector3 from = transform.position + offset;
                Vector3 to = from + toh;
                Gizmos.DrawLine(from, to);
            }
        }
    }
}
