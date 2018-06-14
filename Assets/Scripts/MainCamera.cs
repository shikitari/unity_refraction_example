using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace main
{
    public class MainCamera : MonoBehaviour
    {
        private ComponentManager c;

        public Camera Camera{get{return c.camera;}}

        void Start()
        {
            Prepare();
        }

        void FixedUpdate()
        {

        }

        void Prepare()
        {
            c = new ComponentManager(this);
        }
    }
}