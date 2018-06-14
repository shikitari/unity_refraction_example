using UnityEngine;
using System.Collections;

namespace main
{
    public class Arrow : MonoBehaviour
    {
        static float coneHeight = (1 - 0.7198647f);
        static float coneBottomScale = 1.92000019661f;
        const float defalutRadius = 0.06510416f;

        private ComponentManager c;
        private Mesh mesh;
        private bool isPrepare = false;

        void Start()
        {
            Prepare();
        }

        void Prepare()
        {
            c = new ComponentManager(this);
            isPrepare = true;
        }

        public void Create(Vector3 start, Vector3 end, float radius = defalutRadius)
        {
            if (isPrepare == false) {
                Prepare();
            }
            c.meshFilter.mesh = CreateMesh(start, end, radius);
            transform.position = start;
        }

        private void Deform(Vector3 start, Vector3 dir)
        {
            if (mesh == null) {
                mesh = new Mesh();
            }
        }

        private static Mesh _arrow;
        private static Mesh arrow
        {
            get
            {
                if (_arrow == null)
                {
                    GameObject g = Resources.Load("Meshes/arrow") as GameObject;
                    _arrow = g?.GetComponent<MeshFilter>().sharedMesh;
                }
                return _arrow;
            }
        }

        private static Mesh CreateMesh(Vector3 start, Vector3 end, float radius)
        {
            float d = Vector3.Distance(start, end);
            Vector3 direction = Vector3.Normalize(end - start);
            Quaternion q = Quaternion.FromToRotation(Vector3.up, direction);

            Vector3[] vertices;
            Color[] colors;
            CreateArrowVertexAndColor(out vertices, out colors, d, radius);

            Mesh deformedRot = new Mesh();
            int n = vertices.Length;
            Vector3[] verticesRotated = new Vector3[n];

            for (int i = 0; i < n; i++) {
                verticesRotated[i] = q * vertices[i];
            }

            deformedRot.vertices = verticesRotated;
            deformedRot.triangles = Arrow.arrow.triangles;
            deformedRot.colors = colors;
            deformedRot.uv = Arrow.arrow.uv;
            deformedRot.RecalculateBounds();
            deformedRot.RecalculateNormals();

            return deformedRot;
        }

        private static void CreateArrowVertexAndColor(out Vector3[] vertices, out Color[] colors, float length = 1.0f, float radius = defalutRadius)
        {
            Mesh mesh = Arrow.arrow;
            int n = mesh.vertexCount;
            vertices = new Vector3[n];
            colors = new Color[n];

            float coneBotomY = length - coneHeight;

            for (int i = 0; i < n; i++)
            {
                Vector2 posAsXZ = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
                float distanceXZ = Vector2.Distance(posAsXZ, Vector2.zero);
                float rad = Mathf.Atan2(posAsXZ.y, posAsXZ.x);

                Vector2 scaledPosAsXZ = new Vector2(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius);
                Vector2 scaled2PosAsXZ = new Vector2(Mathf.Cos(rad) * radius * coneBottomScale, Mathf.Sin(rad) * radius * coneBottomScale);

                Vector3 p = mesh.vertices[i];

                if (mesh.normals[i] == Vector3.down && mesh.vertices[i].y <= Mathf.Epsilon)
                {
                    if (distanceXZ <= 0.001f)// bottom center
                    {
                    }
                    else//bottom side
                    {
                        p.x = scaledPosAsXZ.x;
                        p.z = scaledPosAsXZ.y;
                    }
                }
                else if (mesh.vertices[i].y <= Mathf.Epsilon)// cylinder bottom side
                {
                    p.x = scaledPosAsXZ.x;
                    p.x = scaledPosAsXZ.x;
                    p.z = scaledPosAsXZ.y;
                }
                else if (mesh.normals[i] == Vector3.down)// cone bottom side
                {
                    p.y = coneBotomY;
                    if (distanceXZ >= 0.124f) {//outside
                        p.x = scaled2PosAsXZ.x;
                        p.z = scaled2PosAsXZ.y;
                    } else {
                        p.x = scaledPosAsXZ.x;
                        p.z = scaledPosAsXZ.y;
                    }
                }
                else if (distanceXZ >= 0.124f)// cone side
                {
                    p.y = coneBotomY;
                    p.x = scaled2PosAsXZ.x;
                    p.z = scaled2PosAsXZ.y;
                }
                else if (distanceXZ >= 0.065f)//cylinder top side
                {
                    p.y = coneBotomY;
                    p.x = scaledPosAsXZ.x;
                    p.z = scaledPosAsXZ.y;
                }
                else//top(center)
                {
                    p.y = length;
                }

                vertices[i] = p;
                colors[i] = new Color(vertices[i].y / length, 0, 0, 1);

            }
        }
    }
}