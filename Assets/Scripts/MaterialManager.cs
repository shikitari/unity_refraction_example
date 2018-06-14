using UnityEngine;
using System.Collections;

namespace main
{
    public class MaterialManager
    {
        private static Material _uvCoord;
        public static Material uvCoord
        {
            get
            {
                if (_uvCoord == null)
                {
                    _uvCoord = Resources.Load("Materials/UVCorrd") as Material;
                }
                return _uvCoord;
            }
        }

        private static Material _lens;
        public static Material lens
        {
            get
            {
                if (_lens == null)
                {
                    _lens = Resources.Load("Materials/Lens") as Material;
                }
                return _lens;
            }
        }

        private static Material _wall;
        public static Material wall
        {
            get
            {
                if (_wall == null)
                {
                    _wall = Resources.Load("Materials/Wall") as Material;
                }
                return _wall;
            }
        }

        private static Material _cup3;
        public static Material cup3
        {
            get
            {
                if (_cup3 == null)
                {
                    _cup3 = Resources.Load("Materials/Cup3") as Material;
                }
                return _cup3;
            }
        }

        private static Material _cup3inner;
        public static Material cup3bottom
        {
            get
            {
                if (_cup3inner == null)
                {
                    _cup3inner = Resources.Load("Materials/Cup3Bottom") as Material;
                }
                return _cup3inner;
            }
        }

        private static Material _cup3bottom;
        public static Material cup3inner
        {
            get
            {
                if (_cup3bottom == null)
                {
                    _cup3bottom = Resources.Load("Materials/Cup3Inner") as Material;
                }
                return _cup3bottom;
            }
        }

        private static Material _diffuse;
        public static Material diffuse
        {
            get
            {
                if (_diffuse == null)
                {
                    _diffuse = Resources.Load("Materials/Diffuse") as Material;
                }
                return _diffuse;
            }
        }

        private static Material _vertexColor;
        public static Material vertexColor
        {
            get
            {
                if (_vertexColor == null)
                {
                    _vertexColor = Resources.Load("Materials/VertexColor") as Material;
                }
                return _vertexColor;
            }
        }

       
    }
}