//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace tryMyBounds
//{
//    public struct MyBounds
//    {
//        private Vector3 m_Center;

//        private Vector3 m_Extents;

//        private IBounds m_Bounds;

//        private void Start()
//        {
//            //m_Bounds = this.gameObject.GetComponent<IBounds>();
//            //Debug.Log(gameObject);
//        }

//        private void Update()
//        {
//            //center = m_Bounds.Center();
//            //size = m_Bounds.Size();
//            //Debug.Log("center = "+ center + " size = " + size);
//        }
//        public MyBounds(Vector3 center, Vector3 size)
//        {
//            m_Center = center;
//            m_Extents = size * 0.5f;
//        }
//        //
//        // äTóv:
//        //     The center of the bounding box.
//        public Vector3 center
//        {
//            get
//            {
//                return m_Center;
//            }
//            set
//            {
//                m_Center = value;
//            }
//        }

//        //
//        // äTóv:
//        //     The total size of the box. This is always twice as large as the extents.
//        public Vector3 size
//        {
//            get
//            {
//                return m_Extents * 2f;
//            }
//            set
//            {
//                m_Extents = value * 0.5f;
//            }
//        }

//        //
//        // äTóv:
//        //     The extents of the Bounding Box. This is always half of the size of the Bounds.
//        public Vector3 extents
//        {
//            get
//            {
//                return m_Extents;
//            }
//            set
//            {
//                m_Extents = value;
//            }
//        }

//        //
//        // äTóv:
//        //     The minimal point of the box. This is always equal to center-extents.
//        public Vector3 min
//        {
//            get
//            {
//                return center - extents;
//            }
//            set
//            {
//                SetMinMax(value, max);
//            }
//        }

//        //
//        // äTóv:
//        //     The maximal point of the box. This is always equal to center+extents.
//        public Vector3 max
//        {
//            get
//            {
//                return center + extents;
//            }
//            set
//            {
//                SetMinMax(min, value);
//            }
//        }

//        //
//        // äTóv:
//        //     Sets the bounds to the min and max value of the box.
//        //
//        // ÉpÉâÉÅÅ[É^Å[:
//        //   min:
//        //
//        //   max:
//        public void SetMinMax(Vector3 min, Vector3 max)
//        {
//            extents = (max - min) * 0.5f;
//            center = min + extents;
//        }
//    }
//}