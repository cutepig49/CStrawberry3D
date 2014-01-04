using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.loader
{
    public class Loader
    {
        private static Loader _singleton;
        public static Loader getSingleton()
        {
            if (_singleton == null)
            {
                _singleton = new Loader();
            }
            return _singleton;
        }

        private Dictionary<string, Resource> _assets = new Dictionary<string, Resource>();
        public Loader()
        {
            if (_singleton != null)
            {
                throw new Exception("Exception: Use Loader.getSingleton()");
            }
            _singleton = this;

            _createDefaultCube();
        }
        public bool hasAsset(string assetName)
        {
            return _assets.ContainsKey(assetName);
        }
        public object getAsset(string assetName)
        {
            return _assets[assetName];
        }
        public Mesh getMesh(string assetName)
        {
            return (Mesh)_assets[assetName];
        }
        bool createManualMesh(string assetName, float[] positionArray, ushort[] indexArray, float[] texCoordArray = null, float[] normalArray = null, float[] colorArray = null)
        {
            if (hasAsset(assetName))
                return false;
            Mesh asset = new Mesh(positionArray, indexArray, texCoordArray, normalArray, colorArray);
            if (asset == null)
            {
                return false;
            }
            _assets[assetName] = asset;
            return true;
        }
        private void _createDefaultCube()
        {
            string assetName = "Cube";
            float[] positionArray = {
                                        // Front face
                                        -1.0f, -1.0f, 1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 1.0f, -1.0f, 1.0f, 1.0f,
                                        // Back face
                                        -1.0f, -1.0f, -1.0f, -1.0f, 1.0f, -1.0f, 1.0f, 1.0f, -1.0f, 1.0f, -1.0f, -1.0f,
                                        // Top face
                                        -1.0f, 1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, -1.0f,
                                        // Bottom face
                                         -1.0f, -1.0f, -1.0f, 1.0f, -1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f, -1.0f, 1.0f,
                                        // Right face
                                        1.0f, -1.0f, -1.0f, 1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 1.0f, -1.0f, 1.0f,
                                        // Left face
                                        -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, 1.0f, -1.0f, 1.0f, 1.0f, -1.0f, 1.0f, -1.0f
                                    };
            ushort[] indexArray = {
                                      // Front face
                                      0, 1, 2, 0, 2, 3,
                                      // Back face
                                      4, 5, 6, 4, 6, 7,
                                      // Top face
                                      8, 9, 10, 8, 10, 11,
                                      // Bottom face
                                      12, 13, 14, 12, 14, 15,
                                      // Right face
                                      16, 17, 18, 16, 18, 19,
                                      // Left face
                                      20, 21, 22, 20, 22, 23
                                  };
            float[] texCoordArray = {
                                        // Front face
                                        0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f,

                                        // Back face
                                        1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f,

                                        // Top face
                                        0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,

                                        // Bottom face
                                        1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,

                                        // Right face
                                        1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f,

                                        // Left face
                                        0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f
                                    };
            float[] normalArray = {
                                      // Front face
                                      0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
                                      // Back face
                                      0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f,
                                      // Top face
                                      0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
                                      // Bottom face
                                      0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f,
                                      // Right face
                                      1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                                      // Left face
                                      -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f
                                   };
            List<float> colorArray = new List<float>();
            for (int i = 0; i < 24; i++)
            {
                colorArray.Add(1.0f);
                colorArray.Add(1.0f);
                colorArray.Add(1.0f);
                colorArray.Add(1.0f);
            }
            createManualMesh(assetName, positionArray, indexArray, texCoordArray, normalArray, colorArray.ToArray());
        }
    }
}
