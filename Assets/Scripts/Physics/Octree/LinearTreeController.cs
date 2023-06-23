using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearTreeController : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private int _level = 3;

    [SerializeField]
    private float _left = 0f;

    [SerializeField]
    private float _top = 0f;

    [SerializeField]
    private float _right = 10f;

    [SerializeField]
    private float _bottom = 10f;

    [SerializeField]
    private float _front = 0f;

    [SerializeField]
    private float _back = 10f;
    #endregion SerializeField

    #region Variables
    private LinearTreeManager<GameObject> _manager;
    private MortonCellViewer _cellViewer;
    private MortonCellViewer CellViewer
    {
        get
        {
            if (_cellViewer == null)
            {
                MortonCellViewer viewer = GetComponent<MortonCellViewer>();
                if (viewer == null)
                {
                    viewer = gameObject.AddComponent<MortonCellViewer>();
                }
                _cellViewer = viewer;
            }
            return _cellViewer;
        }
    }

    public List<GameObject> _objects;

    public List<GameObject> Objects
    {
        get { return _objects; }
        set { _objects = value; }
    }
    #endregion Variables

    #region MonoBehaviour
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        CellViewer.Left = _left;
        CellViewer.Right = _right;
        CellViewer.Top = _top;
        CellViewer.Bottom = _bottom;
        CellViewer.Front = _front;
        CellViewer.Back = _back;
        CellViewer.Division = 1 << _level;
    }

    void Awake()
    {
        CellViewer.Left = _left;
        CellViewer.Right = _right;
        CellViewer.Top = _top;
        CellViewer.Bottom = _bottom;
        CellViewer.Front = _front;
        CellViewer.Back = _back;
        CellViewer.Division = 1 << _level;
    }

    void Start()
    {
        _manager = new LinearTreeManager<GameObject>(_level, _left, _top, _right, _bottom, _front, _back);

        Debug.Log("objects = " + _objects.Count);
        // オブジェクトを登録
        RegisterObjects();

    }

    private List<GameObject> _collisionList = new List<GameObject>();
    public List<GameObject> GetCollisionList() { return _collisionList; }

    void Update()
    {
        _manager.GetAllCollisionList(_collisionList);
    }

    void OnDrawGizmos()
    {
        // Connect collision pairs with line.
        Gizmos.color = Color.cyan;
        for (int i = 0; i < _collisionList.Count; i += 2)
        {
            GameObject g0 = _collisionList[i + 0];
            GameObject g1 = _collisionList[i + 1];

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
            Debug.LogWarningFormat("Augument must have a `MortonAgent` component. {0}", target);
            return;
        }

        agent.Manager = _manager;
    }


    /// <summary>
    /// オブジェクトを登録
    /// </summary>
    void RegisterObjects()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            RegisterObject(_objects[i]);
        }
    }
}
