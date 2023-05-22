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

    //private MyBounds MyBounds;
    //private Bounds Bounds;

    private IBounds m_Bounds;

    #region MonoBehaviour

    private Collider _collider;
    // Bounds like AABB of this game object.
    public Bounds Bounds
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
            }
            return _collider.bounds;
        }
    }
    void Awake()
    {
        TreeData = new TreeData<GameObject>(gameObject);
        m_Bounds = this.gameObject.GetComponent<IBounds>();
        //Debug.Log(name);
        //MyBounds = GetComponent<MyBounds>();
        //Bound = GetComponent<Bounds>();
        //Debug.Log(Bounds);
        //MyBounds.center = m_Bounds.Center();
        //MyBounds.size = m_Bounds.Size();
        //MyBounds = new MyBounds(m_Bounds.Center(), m_Bounds.Size());
        //MyBounds.center = transform.position;
        //MyBounds.size = transform.localScale;
    }

    private void FixedUpdate()
    {
        //MyBounds.center = transform.position;
        //MyBounds.size = transform.localScale;
        //Bounds.center = m_Bounds.Center();
        //MyBounds.size = m_Bounds.Size();
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
        _manager.Register(Bounds, TreeData);
    }


}
