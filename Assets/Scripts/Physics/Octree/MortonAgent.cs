using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An agent will be in octree.
/// </summary>
public class MortonAgent : MonoBehaviour
{
    private LinearTreeManager<GameObject> _manager;
    public LinearTreeManager<GameObject> Manager
    {
        get
        {
            return _manager;
        }
        set
        {
            if (_manager == value)
            {
                return;
            }

            // Remove from current manager.
            TreeData.Remove();

            // Change to new manager and register myself.
            _manager = value;
            RegisterUpdate();
        }
    }

    public TreeData<GameObject> TreeData { get; private set; }

    private MyBounds MyBounds;

    private IBounds m_Bounds;

    #region MonoBehaviour
    void Awake()
    {
        TreeData = new TreeData<GameObject>(gameObject);
        m_Bounds = this.gameObject.GetComponent<IBounds>();
        //Debug.Log(name);
        MyBounds = GetComponent<MyBounds>();
        MyBounds.center = m_Bounds.Center();
        MyBounds.size = m_Bounds.Size();
        //MyBounds.center = transform.position;
        //MyBounds.size = transform.localScale;
    }

    private void FixedUpdate()
    {
        //MyBounds.center = transform.position;
        //MyBounds.size = transform.localScale;
        MyBounds.center = m_Bounds.Center();
        MyBounds.size = m_Bounds.Size();
        //Debug.Log("name = " + name + "MyBounds = " + MyBounds.name);
    }

    void OnDestroy()
    {
        TreeData.Remove();
    }

    void Update()
    {
        if (_manager == null)
        {
            return;
        }

        RegisterUpdate();
    }
    #endregion MonoBehaviour

    void RegisterUpdate()
    {
        _manager.Register(MyBounds, TreeData);
    }
}
