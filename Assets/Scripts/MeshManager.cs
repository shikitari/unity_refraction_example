using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace main
{
    public class MeshManager
    {
        private static GameObject _arrowPrefab;
        public static GameObject arrowPrefab
        {
            get
            {
                if (_arrowPrefab == null)
                {
                    _arrowPrefab = Resources.Load("Prefabs/Arrow") as GameObject;
                }
                return _arrowPrefab;
            }
        }

        private static Mesh _halfLens;
        public static Mesh halfLens {
            get{
                if (_halfLens == null) {
                    GameObject g = Resources.Load("Meshes/Lens_half2") as GameObject;
                    _halfLens = g?.GetComponent<MeshFilter>().sharedMesh;
                }
                return _halfLens;
            }
        }

        private static Mesh _cup2;
        public static Mesh cup2
        {
            get
            {
                if (_cup2 == null)
                {
                    GameObject g = Resources.Load("Meshes/Cup2") as GameObject;
                    _cup2 = g?.GetComponent<MeshFilter>().sharedMesh;
                }
                return _cup2;
            }
        }

        static public Mesh FlipFace(Mesh targetMesh)
        {
            Mesh mesh = new Mesh();

            // refarence(not copy)
            mesh.vertices = targetMesh.vertices;

            // new instance(copy)
            int[] triangles = new int[targetMesh.triangles.Length];
            int n = triangles.Length / 3;
            for (int i = 0; i < n; i++) {
                int startIndex = i * 3;
                triangles[startIndex] = targetMesh.triangles[startIndex];
                triangles[startIndex + 1] = targetMesh.triangles[startIndex + 2];
                triangles[startIndex + 2] = targetMesh.triangles[startIndex + 1];
            }
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }

        static public void IdentifyTriagleByTriangleIndex(int startTriangleIndex, Mesh mesh, out int[] triangleIndexes, out Vector3[] triangle, Matrix4x4? model2World = null)
        {
            triangle = new Vector3[3];
            triangleIndexes = new int[3];
            int index = startTriangleIndex * 3;

            Matrix4x4 model2WorldCasted = model2World ?? Matrix4x4.identity;

            for (int i = 0; i < 3; i++)
            {
                int vertexIndex = mesh.triangles[index + i];

                triangleIndexes[i] = vertexIndex;
                triangle[i] = mesh.vertices[vertexIndex];
                if (model2World != null)
                {
                    triangle[i] = model2WorldCasted.MultiplyPoint3x4(triangle[i]);
                }
                //Debug.DrawLine(Vector3.zero, triangle[i], Color.green, 10f, false);
            }
        }

        static private bool CheckDuplicatePoint(ref int[] triangleIndex, int tartgetIndex) {
            int i;
            for (i = 0; i < 3; i++)
            {
                if (triangleIndex[i] == tartgetIndex) break;//same
            }
            if (i < 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Creates the dummy mesh for debug.
        /// </summary>
        /// <returns>The dummy mesh.</returns>
        static public Mesh CreateDummyMesh()
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[3];
            vertices[0] = new Vector3(5, 5, 0);
            vertices[1] = new Vector3(-5, 5, 0);
            vertices[2] = new Vector3(0, -5, 0);

            int[] triangles = new int[3];
            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            //mesh.RecalculateNormals();

            Vector3[] normals = new Vector3[3];
            normals[0] = new Vector3(1, 0, 0);
            normals[1] = new Vector3(0, 1, 0);
            normals[2] = new Vector3(1, 0, 1);

            mesh.normals = normals;

            return mesh;
        }
    }
}
