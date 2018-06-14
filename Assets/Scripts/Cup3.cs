using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace main
{

    public partial class Cup3 : MonoBehaviour
    {
        public static Matrix4x4 world2LessonCoordinate = Matrix4x4.TRS(new Vector3(0, 1, 0), Quaternion.Euler(0, 90, 0), Vector3.one);
        static float waterTopY = -0.05f;
        static float waterBottomY = -2.00f;

        protected Material _diffuseMaterial;
        protected Material cupTopMaterial;
        protected Camera rayTraceCamera;

        protected GameObject container;
        protected GameObject cupBottomF;
        protected GameObject cupBottomR;
        protected GameObject cupOutside;
        protected GameObject cupInside;
        protected GameObject cupTop;
        protected RenderTexture renderTexture;
        protected AnimationCoroutine1 animationCoroutine1;

        // use on editor scripts.
        public bool sceneViewDebug = false; 

        void Start()
        {
            Prepare();
            Create();
            Init();

            StartCoroutine(animationCoroutine1.UpTheWaterLevel());
        }

        void Prepare()
        {
            animationCoroutine1 = new AnimationCoroutine1(this);
            SetRayTraceCamera(Camera.main);
            cupBottomF = transform.Find("CupBottomF").gameObject;
            cupBottomR = transform.Find("CupBottomR").gameObject;
            cupOutside = transform.Find("CupOutside").gameObject;
            cupInside = transform.Find("CupInside").gameObject;
            cupTop = transform.Find("CupTop").gameObject;
            cupTopMaterial = cupTop.GetComponent<MeshRenderer>().sharedMaterial;
            renderTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
            rayTraceCamera.targetTexture = renderTexture;
        }

        void SetRayTraceCamera(Camera targetCamera){
            rayTraceCamera = GameObject.Find("Cup3Camera").GetComponent<Camera>();
            rayTraceCamera.transform.position = targetCamera.transform.position;
            rayTraceCamera.transform.rotation = targetCamera.transform.rotation;
        }

        void Create()
        {
            
        }

        void Init()
        {
            SetMaterialValue();

            Vector3 p = Vector3.zero;
            p.y = -1;
            cupTop.transform.position = p;
        }

        void FixedUpdate()
        {
            SetMaterialValue();
        }

        private void SetMaterialValue()
        {
            if (sceneViewDebug == false)
            {
                cupTopMaterial.SetMatrix("_RTCameraProjection", rayTraceCamera.projectionMatrix);
                cupTopMaterial.SetMatrix("_RTCameraView", rayTraceCamera.worldToCameraMatrix);
                cupTopMaterial.SetTexture("_MainTex", renderTexture);
            }
        }

        static void GetMeshRendererAndSetMaterial(GameObject g, Material m)
        {
            MeshRenderer mr = g.GetComponent<MeshRenderer>();
            mr.sharedMaterial = m;
        }

        public Material diffuseMaterial
        {
            get
            {
                if (_diffuseMaterial != null)
                {
                    return _diffuseMaterial;
                }
                GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
                primitive.SetActive(false);
                _diffuseMaterial = primitive.GetComponent<MeshRenderer>().sharedMaterial;
                Destroy(primitive);
                return _diffuseMaterial;
            }
        }
#if UNITY_EDITOR && false
        void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(0.3f, 0.0f, 0.0f, 1f);
            //Gizmos.DrawWireSphere(Vector3.zero, 3);
            //Gizmos.DrawLine(Vector3.zero, Vector3.one);

        }
#endif
    }
}