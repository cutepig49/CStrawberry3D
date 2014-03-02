using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Assimp;

namespace CStrawberry3D.TK
{
    public struct TKMeshDescription
    {
        public bool HasVertices { get; set; }
        public bool HasVertexColors { get; set; }
        public bool HasTextureCoords { get; set; }
        public bool HasNormals { get; set; }
    }
    public class TKMeshEntry
    {
        public static TKMeshEntry Create(Vector3D[] vertices, uint[] indices, Color4D[] vertexColors, Vector3D[] textureCoords, Vector3D[] normals, TKMeshDescription desc)
        {
            var _vertices = new List<Vector3>();
            var _vertexColors = new List<Vector4>();
            var _normals = new List<Vector3>();
            var _textureCoords = new List<Vector3>();
            if (desc.HasVertices)
            {
                foreach(var v in vertices)
                {
                    _vertices.Add(new Vector3(v.X, v.Y, v.Z));
                }
            }
            if (desc.HasNormals)
            {
                foreach(var n in normals)
                {
                    _normals.Add(new Vector3(n.X, n.Y, n.Z));
                }
            }
            if (desc.HasTextureCoords)
            {
                foreach(var t in textureCoords)
                {
                    _textureCoords.Add(new Vector3(t.X, t.Y, t.Z));
                }
            }
            if (desc.HasVertexColors)
            {
                foreach(var c in vertexColors)
                {
                   _vertexColors.Add(new Vector4(c.R, c.G, c.B, c.A));
                }
            }
            return new TKMeshEntry(_vertices.ToArray(), indices, _vertexColors.ToArray(), _normals.ToArray(), _textureCoords.ToArray(), desc);
        }
        static float[] _Vector3ToFloatArray(Vector3[] vectors)
        {
            var floatArray = new List<float>();
            foreach (var v in vectors)
            {
                floatArray.Add(v.X);
                floatArray.Add(v.Y);
                floatArray.Add(v.Z);
            }
            return floatArray.ToArray();
        }
        static float[] _Vector4ToFloatArray(Vector4[] vectors)
        {
            var floatArray = new List<float>();
            foreach (var v in vectors)
            {
                floatArray.Add(v.X);
                floatArray.Add(v.Y);
                floatArray.Add(v.Z);
                floatArray.Add(v.W);
            }
            return floatArray.ToArray();
        }
        public uint[] Indices { get; private set; }
        public Vector3[] Vertices { get; private set; }
        public Vector4[] VertexColors { get; private set; }
        public Vector3[] Normals { get; private set; }
        public Vector3[] TextureCoords { get; private set; }
        public TKMeshDescription Description { get; private set; }
        public TKVertexBuffer PositionBuffer { get; private set; }
        public TKVertexBuffer IndexBuffer { get; private set; }
        public TKVertexBuffer TextureCoordBuffer { get; private set; }
        public TKVertexBuffer NormalBuffer { get; private set; }
        public TKVertexBuffer ColorBuffer { get; private set; }
        TKMeshEntry(Vector3[] vertices, uint[] indices, Vector4[] vertexColors, Vector3[] normals, Vector3[] textureCoords, TKMeshDescription desc)
        {
            Description = desc;

            var verticesArray = _Vector3ToFloatArray(vertices);
            var verticesBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verticesArray.Length * sizeof(float)), verticesArray, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            PositionBuffer = TKVertexBuffer.Create(verticesBuffer, 3, verticesArray.Length / 3);


            var indicesBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            IndexBuffer = TKVertexBuffer.Create(indicesBuffer, 1, indices.Length);

            if (desc.HasNormals)
            {
                var normalsBuffer = GL.GenBuffer();
                var normalsArray = _Vector3ToFloatArray(normals);
                GL.BindBuffer(BufferTarget.ArrayBuffer, normalsBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalsArray.Length * sizeof(float)), normalsArray, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                NormalBuffer = TKVertexBuffer.Create(normalsBuffer, 3, normalsArray.Length / 3);
            }

            if (desc.HasTextureCoords)
            {
                var texCoordsBuffer = GL.GenBuffer();
                var texCoordsArray = _Vector3ToFloatArray(textureCoords);
                GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordsBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texCoordsArray.Length * sizeof(float)), texCoordsArray, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                TextureCoordBuffer = TKVertexBuffer.Create(texCoordsBuffer, 3, texCoordsArray.Length / 3);
            }

            if (desc.HasVertexColors)
            {
                var colorsBuffer = GL.GenBuffer();
                var colorsArray = _Vector4ToFloatArray(vertexColors);
                GL.BindBuffer(BufferTarget.ArrayBuffer, colorsBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorsArray.Length * sizeof(float)), colorsArray, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                ColorBuffer = TKVertexBuffer.Create(colorsBuffer, 4, colorsArray.Length / 4);
            }
        }
    }
    public class TKMesh:TKAsset
    {
        public static TKMesh CreateFromFile(AssimpImporter importer, string fileName)
        {
            var scene = importer.ImportFile(fileName, PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals);
            if (scene == null)
            {
                return null;
            }
            var entries = new List<TKMeshEntry>();
            for (var i = 0; i < scene.MeshCount; i++)
            {
                var tmp = scene.Meshes[i];
                var desc = new TKMeshDescription
                {
                    HasVertices = tmp.HasVertices,
                    HasNormals = tmp.HasNormals,
                    HasVertexColors = tmp.HasVertexColors(0),
                    HasTextureCoords = tmp.HasTextureCoords(0)
                };
                entries.Add(TKMeshEntry.Create(tmp.Vertices, tmp.GetIndices(), tmp.GetVertexColors(0), tmp.GetTextureCoords(0), tmp.Normals, desc));
            }
            return TKMesh.Create(entries.ToArray());
        }
        public static TKMesh Create(TKMeshEntry[] entries)
        {
            return new TKMesh(entries);
        }
        public List<TKMeshEntry> Entries { get; private set; }
        TKMesh(TKMeshEntry[] entries)
        {
            Entries = new List<TKMeshEntry>(entries);
        }
        //public void AddEntry(float[] positionArray, short[] indexArray, int materialIndex, float[] texCoordArray = null, float[] normalArray = null, float[] colorArray = null)
        //{
        //    var entry = new TKEntry();
        //    entry.MaterialIndex = materialIndex;

        //    var buffer = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
        //    GL.BufferData(BufferTarget.ArrayBuffer,(IntPtr)(positionArray.Length * sizeof(float)), positionArray, BufferUsageHint.StaticDraw);
        //    entry.PositionBuffer = new TKVertexBuffer(buffer, 3, positionArray.Length / 3);

        //    buffer = GL.GenBuffer();
        //    GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer);
        //    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexArray.Length * sizeof(short)), indexArray, BufferUsageHint.StaticDraw);
        //    entry.IndexBuffer = new TKVertexBuffer(buffer, 1, indexArray.Length);

        //    if (texCoordArray != null)
        //    {
        //        buffer = GL.GenBuffer();
        //        GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
        //        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texCoordArray.Length * sizeof(float)), texCoordArray, BufferUsageHint.StaticDraw);
        //        entry.TexCoordBuffer = new TKVertexBuffer(buffer, 2, texCoordArray.Length / 2);
        //    }
        //    if (colorArray != null)
        //    {
        //        buffer = GL.GenBuffer();
        //        GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
        //        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorArray.Length * sizeof(float)), colorArray, BufferUsageHint.StaticDraw);
        //        entry.ColorBuffer = new TKVertexBuffer(buffer, 4, colorArray.Length / 4);
        //    }
        //    if (normalArray != null)
        //    {
        //        buffer = GL.GenBuffer();
        //        GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
        //        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalArray.Length * sizeof(float)), normalArray, BufferUsageHint.StaticDraw);
        //        entry.NormalBuffer = new TKVertexBuffer(buffer, 3, normalArray.Length / 3);
        //    }
        //    Entries.Add(entry);
        //}
        //public void AddMaterial(TKMaterial material)
        //{
        //    Materials.Add(material);
        //}
        //public void Draw(bool isTransparentPass, Matrix4 mvMatrix)
        //{
        //    foreach (var entry in Entries)
        //    {
        //        var material = Materials[entry.MaterialIndex];
        //        if (material.IsTransprent != isTransparentPass)
        //            continue;

        //        for (int i = 0; i < material.Effect.NumPasses; i++)
        //        {
        //            material.Apply(i, entry, mvMatrix);

        //            GL.BindBuffer(BufferTarget.ElementArrayBuffer, entry.IndexBuffer.BufferObject);
        //            GL.DrawElements(PrimitiveType.Triangles, entry.IndexBuffer.NumItems, DrawElementsType.UnsignedShort, 0);

        //            material.Clear();
        //        }
        //    }
        //}
    }
}
