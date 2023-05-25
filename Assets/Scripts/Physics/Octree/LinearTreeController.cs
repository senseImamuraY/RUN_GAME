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
    private float _top = 10f;

    [SerializeField]
    private float _right = 10f;

    [SerializeField]
    private float _bottom = 0f;

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
            return _cellViewer;
        }
    }

    [SerializeField]
    private List<GameObject> _objects;

    //[SerializeField]
    //private GameObject _agentPrefab;
    #endregion Variables

    //public List<>

    #region MonoBehaviour
    void OnValidate()
    {
        if (!Application.isPlaying || _cellViewer == null)
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

        MortonCellViewer viewer = GetComponent<MortonCellViewer>();
        if (viewer == null)
        {
            viewer = gameObject.AddComponent<MortonCellViewer>();
        }
        _cellViewer = viewer;
    }

	void Start()
    {
        // Set initial values
        CellViewer.Left = _left;
        CellViewer.Right = _right;
        CellViewer.Top = _top;
        CellViewer.Bottom = _bottom;
        CellViewer.Front = _front;
        CellViewer.Back = _back;
        CellViewer.Division = 1 << _level;

        _manager = new LinearTreeManager<GameObject>(_level, _left, _top, _right, _bottom, _front, _back);

        // オブジェクトを仮登録してみる
        RegisterObjects();
	}

    private List<GameObject> _collisionList = new List<GameObject>();

    public List<GameObject> GetCollisionList(){ return _collisionList; }

    //void Update()
    void FixedUpdate()
    {
        // Update values if needed
        CellViewer.Left = _left;
        CellViewer.Right = _right;
        CellViewer.Top = _top;
        CellViewer.Bottom = _bottom;
        CellViewer.Front = _front;
        CellViewer.Back = _back;
        CellViewer.Division = 1 << _level;
        // Check collisions
        //if (_manager.isClearing) return; 
        //_manager.GetAllCollisionList(_collisionList);

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    GameObject obj = Instantiate(_agentPrefab);
        //    float s = Random.Range(0.5f, 2.5f);
        //    obj.transform.localScale = Vector3.one * s;
        //    MortonAgent agent = obj.GetComponent<MortonAgent>();
        //    agent.Manager = _manager;
        //}

    }

    void OnDrawGizmos()
    {
        //if(_manager.isClearing) return;
        // Connect collision pairs with line.
        for (int i = 0; i < _collisionList.Count; i++)
        {
            GameObject go = _collisionList[i] as GameObject;
            if (go == null || !go.activeInHierarchy)
            {
                Debug.Log($"GameObject at index {i} is either destroyed or inactive.");
            }
        }
        Gizmos.color = Color.cyan;
        for (int i = 0; i < _collisionList.Count; i += 2)
        {
            GameObject g0 = _collisionList[i + 0];
            GameObject g1 = _collisionList[i + 1];

            Gizmos.DrawLine(g0.transform.position, g1.transform.position);

            //Debug.Log("g1 = " + g0.name +" g2 = " + g1.name);
            if(g0.name == "Player" || g1.name == "Player")
            {
                Debug.Log("g1 = " + g0.name + " g2 = " + g1.name);
            }
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

    void UnregisterObject(GameObject target)
    {

    }

    /// <summary>
    /// オブジェクトを仮に登録してみる
    /// </summary>
    void RegisterObjects()
    {
        //RegisterObject(_object1);
        //RegisterObject(_object2);
        //RegisterObject(_object3);
        //RegisterObject(_object4);

        for (int i = 0; i < _objects.Count; i++)
        {
            RegisterObject(_objects[i]);
        }

        #region For Mock
        //TreeData<GameObject> data1 = new TreeData<GameObject>(_object1);
        //TreeData<GameObject> data2 = new TreeData<GameObject>(_object2);
        //TreeData<GameObject> data3 = new TreeData<GameObject>(_object3);
        //TreeData<GameObject> data4 = new TreeData<GameObject>(_object4);

        //// size: 3
        //_manager.Register(-4f, 4.5f, -1f, 1.5f, -5f, -2f, data1);

        //// size: 1
        //_manager.Register(-0.5f, 4f, 0.5f, 3f, -5f, -4f, data2);

        //// size: 1
        //_manager.Register(2.6f, 3.6f, 3.6f, 2.6f, -3.7f, -2.7f, data3);

        //// size: 1.5
        //_manager.Register(0.5f, 7f, 2f, 5.5f, -5f, -3.5f, data4);
        #endregion For Mock
    }
}
