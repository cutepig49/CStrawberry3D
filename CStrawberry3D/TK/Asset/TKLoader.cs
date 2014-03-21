using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assimp;
using FreeImageAPI;
using OpenTK.Graphics.OpenGL;
using System.Xml;
using System.IO;

namespace CStrawberry3D.TK
{
    public class TKLoader
    {
        public static readonly List<string> MESH_TYPE = new List<string> { ".x", ".nff", ".obj", ".3ds"};
        public static readonly List<string> IMAGE_TYPE = new List<string> { ".jpg", ".bmp", ".png", ".dds" };
        public static readonly List<string> SKYBOX_TYPE = new List<string> { ".skybox" };
        public static TKLoader Create(TKShaderManager shaderManager)
        {
            return new TKLoader(shaderManager);
        }
        AssimpImporter _importer;
        Dictionary<string, TKAsset> _assets;
        TKShaderManager _shaderManager;
        TKLoader(TKShaderManager shaderManager)
        {
            _importer = new AssimpImporter();
            _assets = new Dictionary<string,TKAsset>();
            _shaderManager = shaderManager;
        }
        public bool HasAsset(string assetName)
        {
            return _assets.ContainsKey(assetName);
        }
        public TKAsset GetAsset(string assetName)
        {
            return _assets[assetName];
        }
        public TKMesh GetMesh(string assetName)
        {
            return (TKMesh)GetAsset(assetName);
        }
        public TKTexture GetTexture(string assetName)
        {
            return (TKTexture)GetAsset(assetName);
        }
        public TKCubeMap GetSkyBox(string assetName)
        {
            return (TKCubeMap)GetAsset(assetName);
        }
        public string ParseType(string filePath)
        {
            return filePath.Substring(filePath.LastIndexOf(".")).ToLower();
        }
        public bool LoadAsset(string filePath)
        {
            if (_assets.ContainsKey(filePath))
            {
                return false;
            }
            if (!File.Exists(filePath))
            {
                return false;
            }

            var fileType = ParseType(filePath);
            if (MESH_TYPE.Contains(fileType) && _importer.IsImportFormatSupported(fileType))
            {
                _LoadMesh(filePath);
            }
            else if (IMAGE_TYPE.Contains(fileType))
            {
                _LoadTexture(filePath);
            }
            else if (SKYBOX_TYPE.Contains(fileType))
            {
                _LoadSkyBox(filePath);
            }
            else
            {
                return false;
            }
            return true;
        }
        bool _LoadSkyBox(string filePath)
        {
            var skybox = TKCubeMap.CreateFromFile(filePath);
            if (skybox != null)
            {
                _assets[filePath] = skybox;
                return true;
            }
            else
            {
                return false;
            }
        }
        bool _LoadMesh(string filePath)
        {
            var mesh = TKMesh.CreateFromFile(_importer, _shaderManager, filePath);
            if (mesh != null)
            {
                _assets[filePath] = mesh;
                return true;
            }
            return false;

            //var scene = _importer.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals);
            //if (scene == null)
            //    return false;
            //var mesh = new TKMesh();
            //for (int i = 0; i < scene.MaterialCount; i++)
            //{
            //    var currMaterial = scene.Materials[i];
            //    TKMaterial material;
            //    if (currMaterial.GetTextureCount(TextureType.Diffuse) > 0) 
            //    {
            //        var texPath = currMaterial.GetTextures(TextureType.Diffuse)[0].FilePath;
            //        _LoadTexture(texPath);
            //    }
            //    switch (currMaterial.ShadingMode)
            //    {
            //        case ShadingMode.Phong:
            //            if (currMaterial.GetTextureCount(TextureType.Diffuse) > 0)
            //                material = new TexturedMaterial(GetTexture(currMaterial.GetTexture(TextureType.Diffuse, 0).FilePath));
            //            else
            //                material = new GlobalColorMaterial(new OpenTK.Vector4(1, 0, 1, 1));
            //        break;
            //        //break;
            //        case ShadingMode.Gouraud:
            //        default:
            //            material = new GlobalColorMaterial(new OpenTK.Vector4(1, 0, 1, 1));
            //            break;
            //    }
            //    mesh.AddMaterial(material);
            //}
            //for (int i = 0; i < scene.MeshCount; i++)
            //{
            //    Mesh tmp = scene.Meshes[i];
            //    var positionArray = _Vector3DToFloat(tmp.Vertices);
            //    var normalArray = _Vector3DToFloat(tmp.Normals);
            //    var texCoordArray = _Vector3DToCoord(tmp.GetTextureCoords(0));
            //    var indexArray = tmp.GetShortIndices();
            //    var colorArray = _Color4DToFloat(tmp.GetVertexColors(0));
            //    mesh.AddEntry(positionArray, indexArray, tmp.MaterialIndex, texCoordArray, normalArray, colorArray);
            //}
            //_assets[filePath] = mesh;
            //return true;
        }
        bool _LoadTexture(string filePath)
        {
            var texture = TKTexture.CreateFromFile(filePath);
            if (texture == null)
            {
                return false;
            }
            _assets[filePath] = texture;
            return true;
        }
        //float[] _Vector3DToCoord(Vector3D[] vertices)
        //{
        //    if (vertices == null)
        //        return null;

        //    List<float> newVertices = new List<float>();
        //    foreach (var vertex in vertices)
        //    {
        //        newVertices.Add(vertex.X);
        //        newVertices.Add(vertex.Y);
        //    }
        //    return newVertices.ToArray();
        //}
        //float[] _Vector3DToFloat(Vector3D[] vertices)
        //{
        //    if (vertices == null)
        //        return null;

        //    List<float> newVertices = new List<float>();
        //    foreach (var vertex in vertices)
        //    {
        //        newVertices.Add(vertex.X);
        //        newVertices.Add(vertex.Y);
        //        newVertices.Add(vertex.Z);
        //    }
        //    return newVertices.ToArray();
        //}
        //float[] _Volor4DToFloat(Color4D[] vertices)
        //{
        //    if (vertices == null)
        //        return null;
        //    List<float> newVertices = new List<float>();
        //    foreach (var vertex in vertices)
        //    {
        //        newVertices.Add(vertex.R);
        //        newVertices.Add(vertex.G);
        //        newVertices.Add(vertex.B);
        //        newVertices.Add(vertex.A);
        //    }
        //    return newVertices.ToArray();
        //}
        //float[] _Color4DToFloat(Assimp.Color4D[] vertices)
        //{
        //    if (vertices == null)
        //        return null;
        //    List<float> newVertices = new List<float>();
        //    foreach (var vertex in vertices)
        //    {
        //        newVertices.Add(vertex.R);
        //        newVertices.Add(vertex.G);
        //        newVertices.Add(vertex.B);
        //        newVertices.Add(vertex.A);
        //    }
        //    return newVertices.ToArray();
        //}
    }
}
