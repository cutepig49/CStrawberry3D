//using Assimp;
//using System;
//using System.Collections.Generic;
//using CStrawberry3D.component;
//using OpenTK;
//using System.Drawing;
//using Tao.DevIl;
//using CStrawberry3D.TK;

//namespace CStrawberry3D.Loader
//{
//    public class Loader
//    {
//        public static List<string> MESH = new List<string>{ ".x", ".nff", ".obj", ".max"};
//        private static Loader _singleton;
//        public static Loader getSingleton()
//        {
//            if (_singleton == null)
//            {
//                _singleton = new Loader();
//            }
//            return _singleton;
//        }

//        private AssimpImporter _importer = new AssimpImporter();
//        private Dictionary<string, Resource> _assets = new Dictionary<string, Resource>();
//        public Loader()
//        {
//            if (_singleton != null)
//            {
//                throw new Exception("Exception: Use Loader.getSingleton()");
//            }
//            _singleton = this;

//            //Il.ilInit();
//            //Ilu.iluInit();
//            //Ilut.ilutInit();

//            //Il.ilEnable(Il.IL_ORIGIN_SET);
//            //Il.ilOriginFunc(Il.IL_ORIGIN_LOWER_LEFT);

//            //Il.ilEnable(Il.IL_TYPE_SET);
//            //Il.ilTypeFunc(Il.IL_UNSIGNED_BYTE);

//            //Il.ilEnable(Il.IL_FORMAT_SET);
//            //Il.ilFormatFunc(Il.IL_RGB);

//            //Ilut.ilutRenderer(Ilut.ILUT_OPENGL);

//            //_createDefaultShapes();
//        }
//        public bool hasAsset(string assetName)
//        {
//            return _assets.ContainsKey(assetName);
//        }
//        public object getAsset(string assetName)
//        {
//            return _assets[assetName];
//        }
//        public Mesh getMesh(string assetName)
//        {
//            return (TKMesh)_assets[assetName];
//        }

//        protected int nextPowerOfTwo(int n)
//        {
//            double power = 0;
//            while (n > Math.Pow(2, power))
//                power++;
//            return (int)Math.Pow(2, power);
//        }

//        public bool loadAsset(string filePath)
//        {
//            if (_assets.ContainsKey(filePath))
//                return false;
 
//            string fileType = filePath.Substring(filePath.LastIndexOf(".")).ToLower();
//            if (MESH.Contains(fileType) && _importer.IsImportFormatSupported(fileType))
//            {
//                var scene = _importer.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs);
//                if (scene == null)
//                    return false;

//                var mesh = new Mesh();

//                for (int i = 0; i < scene.MaterialCount; i++)
//                {

//                    Assimp.Material currMaterial = scene.Materials[i];

//                    component.Material material;
//                    switch (currMaterial.ShadingMode)
//                    {
//                        case ShadingMode.Phong:
                            
//                            material = new TexturedPhongMaterial();
//                            break;
//                        case ShadingMode.Gouraud:
//                        default:
//                            material = new GlobalColorMaterial(new Vector4(1,0,1,1));
//                            break;
//                    }
//                    mesh.addMaterial(material);
//                    List<TextureSlot> diffuseTextures = new List<TextureSlot>();
//                    for (int index = 0; i < currMaterial.GetTextureCount(TextureType.Diffuse);i++ )
//                    {
//                        diffuseTextures.Add(currMaterial.GetTexture(TextureType.Diffuse, index));
//                    }
//                    foreach (var diffuse in diffuseTextures)
//                    {
//                        var ilImage = Il.ilGenImage();
//                        Il.ilBindImage(ilImage);
//                        Il.ilLoadImage(diffuse.FilePath);
//                        var width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
//                        var tex_width = nextPowerOfTwo(width);
//                        var height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
//                        var tex_height = nextPowerOfTwo(height);
//                        var image_depth = Il.ilGetInteger(Il.IL_IMAGE_DEPTH);
//                        Ilu.iluFlipImage();
//                        Il.ilConvertImage(Il.IL_RGBA, Il.IL_UNSIGNED_BYTE);
//                        var texture = new Texture(width, height, Il.ilGetData());
//                        //var textureObject = Ilut.ilutGLLoadImage(diffuse.FilePath);
//                        //var texture = new Texture(textureObject);
//                        mesh.addDiffuseTexture(texture);
//                    }

//                }
//                for (int i = 0; i < scene.MeshCount; i++)
//                {
//                    Assimp.Mesh tmp = scene.Meshes[i];
//                    float[] positionArray = _vector3ToFloat(tmp.Vertices);
//                    float[] normalArray = _vector3ToFloat(tmp.Normals);
//                    float[] texCoordArray = _vector3ToCoord(tmp.GetTextureCoords(0));
//                    short[] indexArray = tmp.GetShortIndices();
//                    float[] colorArray = _color4ToFloat(tmp.GetVertexColors(0));
//                    mesh.addEntry(positionArray, indexArray, tmp.MaterialIndex, texCoordArray, normalArray, colorArray);
//                }
//                _assets[filePath] = mesh;
//            }
//            else
//            {
//                return false;
//            }
//            return true;
//        }
//        public bool createManualMesh(string assetName, float[] positionArray, short[] indexArray, int materialIndex, float[] texCoordArray = null, float[] normalArray = null, float[] colorArray = null)
//        {
//            if (hasAsset(assetName))
//                return false;
//            Mesh asset = new Mesh();
//            asset.addEntry(positionArray, indexArray, materialIndex, texCoordArray, normalArray, colorArray);
//            if (asset == null)
//            {
//                return false;
//            }
//            _assets[assetName] = asset;
//            return true;
//        }
//        private bool _renameAsset(string assetName, string newName)
//        {
//            if (_assets.ContainsKey(newName) || !_assets.ContainsKey(assetName))
//                return false;
//            _assets[newName] = _assets[assetName];
//            _assets.Remove(assetName);
//            return true;
//        }
//        private float[] _vector3ToCoord(Assimp.Vector3D[] vertices)
//        {
//            if (vertices == null)
//                return null;

//            List<float> newVertices = new List<float>();
//            foreach (var vertex in vertices)
//            {
//                newVertices.Add(vertex.X);
//                newVertices.Add(vertex.Y);
//            }
//            return newVertices.ToArray();
//        }
//        private float[] _vector3ToFloat(Assimp.Vector3D[] vertices)
//        {
//            if (vertices == null)
//                return null;

//            List<float> newVertices = new List<float>();
//            foreach (var vertex in vertices)
//            {
//                newVertices.Add(vertex.X);
//                newVertices.Add(vertex.Y);
//                newVertices.Add(vertex.Z);
//            }
//            return newVertices.ToArray();
//        }
//        private float[] _color4ToFloat(Assimp.Color4D[] vertices)
//        {
//            if (vertices == null)
//                return null;
//            List<float> newVertices = new List<float>();
//            foreach (var vertex in vertices)
//            {
//                newVertices.Add(vertex.R);
//                newVertices.Add(vertex.G);
//                newVertices.Add(vertex.B);
//                newVertices.Add(vertex.A);
//            }
//            return newVertices.ToArray();
//        }
//        private void _createDefaultShapes()
//        {
//            loadAsset("cube.nff");
//            _renameAsset("cube.nff", "Cube");

//            loadAsset("sphere.nff");
//            _renameAsset("sphere.nff", "Sphere");
//        }
//    }
//}
