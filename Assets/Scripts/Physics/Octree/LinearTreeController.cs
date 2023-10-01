using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearTreeController : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private int level = 3;

    [SerializeField]
    private float left = 0f;

    [SerializeField]
    private float top = 0f;

    [SerializeField]
    private float right = 10f;

    [SerializeField]
    private float bottom = 10f;

    [SerializeField]
    private float front = 0f;

    [SerializeField]
    private float back = 10f;
    #endregion SerializeField

    #region Variables
    private LinearTreeManager<GameObject> manager;
    private MortonCellViewer cellViewer;
    private MortonCellViewer CellViewer
    {
        get
        {
            if (cellViewer == null)
            {
                MortonCellViewer viewer = GetComponent<MortonCellViewer>();
                if (viewer == null)
                {
                    viewer = gameObject.AddComponent<MortonCellViewer>();
                }
                cellViewer = viewer;
            }
            return cellViewer;
        }
    }

    public List<GameObject> objects;

    public List<GameObject> Objects
    {
        get { return objects; }
        set { objects = value; }
    }
    #endregion Variables

    #region MonoBehaviour
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        CellViewer.Left = left;
        CellViewer.Right = right;
        CellViewer.Top = top;
        CellViewer.Bottom = bottom;
        CellViewer.Front = front;
        CellViewer.Back = back;
        CellViewer.Division = 1 << level;
    }

    void Awake()
    {
        CellViewer.Left = left;
        CellViewer.Right = right;
        CellViewer.Top = top;
        CellViewer.Bottom = bottom;
        CellViewer.Front = front;
        CellViewer.Back = back;
        CellViewer.Division = 1 << level;
    }

    void Start()
    {
        manager = new LinearTreeManager<GameObject>(level, left, top, right, bottom, front, back);

        // オブジェクトを登録
        RegisterObjects();

    }

    private List<GameObject> collisionList = new List<GameObject>();
    public List<GameObject> GetCollisionList() { return collisionList; }

    void Update()
    {
        manager.GetAllCollisionList(collisionList);
    }

    void OnDrawGizmos()
    {
        // Connect collision pairs with line.
        Gizmos.color = Color.cyan;
        for (int i = 0; i < collisionList.Count; i += 2)
        {
            GameObject g0 = collisionList[i + 0];
            GameObject g1 = collisionList[i + 1];

            Gizmos.DrawLine(g0.transform.position, g1.transform.position);
        }
    }
    #endregion MonoBehaviour

    /// <summary>
    /// ゲームオブジェクトを登録する
    /// </summary>
    /// <param name="target">ターゲットのゲームオブジェクト</param>
    void RegisterObject(GameObject target)
    {
        MortonAgent agent = target.GetComponent<MortonAgent>();
        if (agent == null)
        {
            return;
        }

        agent.Manager = manager;
    }


    /// <summary>
    /// オブジェクトを登録
    /// </summary>
    void RegisterObjects()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            RegisterObject(objects[i]);
        }
    }
}
