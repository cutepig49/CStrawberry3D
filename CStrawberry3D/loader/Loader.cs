using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assimp;

namespace CStrawberry3D.loader
{
    public class Loader
    {
        public static List<string> MSH = new List<string>{ ".x", ".nff"};
        private static Loader _singleton;
        public static Loader getSingleton()
        {
            if (_singleton == null)
            {
                _singleton = new Loader();
            }
            return _singleton;
        }

        private AssimpImporter _importer = new AssimpImporter();
        private Dictionary<string, Resource> _assets = new Dictionary<string, Resource>();
        public Loader()
        {
            if (_singleton != null)
            {
                throw new Exception("Exception: Use Loader.getSingleton()");
            }
            _singleton = this;

            _createDefaultShapes();
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

        public bool loadAsset(string filePath)
        {
            if (_assets.ContainsKey(filePath))
                return false;
 
            string fileType = filePath.Substring(filePath.LastIndexOf(".")).ToLower();
            if (MSH.Contains(fileType) && _importer.IsImportFormatSupported(fileType))
            {
                var scene = _importer.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs);
                if (scene == null)
                    return false;

                var mesh = new Mesh();

                for (int i = 0; i < scene.MaterialCount; i++)
                {

                    Assimp.Material currMaterial = scene.Materials[i];

                    //currMaterial.HasColorDiffuse;
                    //currMaterial.HasColorAmbient;
                    //currMaterial.HasColorSpecular;
                    //currMaterial.HasColorTransparent;
                    
                    //component.Material.createCustomMaterial(currMaterial.HasColorAmbient, currMaterial.HasColorDiffuse, currMaterial.HasColorSpecular);
                    
                }
                for (int i = 0; i < scene.MeshCount; i++)
                {
                    Assimp.Mesh tmp = scene.Meshes[i];
                    float[] positionArray = _vector2float(tmp.Vertices);
                    float[] normalArray = _vector2float(tmp.Normals);
                    float[] texCoordArray = _vector2float(tmp.GetTextureCoords(0));
                    short[] indexArray = tmp.GetShortIndices();
                    float[] colorArray = _color2float(tmp.GetVertexColors(0));
                    mesh.addEntry(positionArray, indexArray, tmp.MaterialIndex, texCoordArray, normalArray, colorArray);

                }
                _assets[filePath] = mesh;
            }
            else
            {
                return false;
            }
            return true;
        }
        public bool createManualMesh(string assetName, float[] positionArray, short[] indexArray, int materialIndex, float[] texCoordArray = null, float[] normalArray = null, float[] colorArray = null)
        {
            if (hasAsset(assetName))
                return false;
            Mesh asset = new Mesh();
            asset.addEntry(positionArray, indexArray, materialIndex, texCoordArray, normalArray, colorArray);
            if (asset == null)
            {
                return false;
            }
            _assets[assetName] = asset;
            return true;
        }
        private bool _renameAsset(string assetName, string newName)
        {
            if (_assets.ContainsKey(newName) || !_assets.ContainsKey(assetName))
                return false;
            _assets[newName] = _assets[assetName];
            _assets.Remove(assetName);
            return true;
        }
        private float[] _vector2float(Assimp.Vector3D[] vertices)
        {
            if (vertices == null)
                return null;

            List<float> newVertices = new List<float>();
            foreach (Assimp.Vector3D vertex in vertices)
            {
                newVertices.Add(vertex.X);
                newVertices.Add(vertex.Y);
                newVertices.Add(vertex.Z);
            }
            return newVertices.ToArray();
        }
        private float[] _color2float(Assimp.Color4D[] vertices)
        {
            if (vertices == null)
                return null;
            List<float> newVertices = new List<float>();
            foreach (Assimp.Color4D vertex in vertices)
            {
                newVertices.Add(vertex.R);
                newVertices.Add(vertex.G);
                newVertices.Add(vertex.B);
                newVertices.Add(vertex.A);
            }
            return newVertices.ToArray();
        }
        private void _createDefaultShapes()
        {
            loadAsset("cube.nff");
            _renameAsset("cube.nff", "Cube");

            loadAsset("sphere.nff");
            _renameAsset("sphere.nff", "Sphere");
        }
    }
}
