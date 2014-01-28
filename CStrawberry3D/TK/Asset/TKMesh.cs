using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace CStrawberry3D.TK
{
    public struct TKEntry
    {
        public TKVertexBuffer PositionBuffer { get; set; }
        public TKVertexBuffer IndexBuffer { get; set; }
        public TKVertexBuffer TexCoordBuffer { get; set; }
        public TKVertexBuffer NormalBuffer { get; set; }
        public TKVertexBuffer ColorBuffer { get; set; }
        public int MaterialIndex { get; set; }
    }
    public class TKMesh:TKAsset
    {
        public List<TKMaterial> Materials { get; private set; }
        public List<TKEntry> Entries { get; private set; }
        public TKMesh()
        {
            Materials = new List<TKMaterial>();
            Entries = new List<TKEntry>();
        }
        public void AddEntry(float[] positionArray, short[] indexArray, int materialIndex, float[] texCoordArray = null, float[] normalArray = null, float[] colorArray = null)
        {
            var entry = new TKEntry();
            entry.MaterialIndex = materialIndex;

            var buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer,(IntPtr)(positionArray.Length * sizeof(float)), positionArray, BufferUsageHint.StaticDraw);
            entry.PositionBuffer = new TKVertexBuffer(buffer, 3, positionArray.Length / 3);

            buffer = GL.GenBuffer();
            GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexArray.Length * sizeof(short)), indexArray, BufferUsageHint.StaticDraw);
            entry.IndexBuffer = new TKVertexBuffer(buffer, 1, indexArray.Length);

            if (texCoordArray != null)
            {
                buffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texCoordArray.Length * sizeof(float)), texCoordArray, BufferUsageHint.StaticDraw);
                entry.TexCoordBuffer = new TKVertexBuffer(buffer, 2, texCoordArray.Length / 2);
            }
            if (colorArray != null)
            {
                buffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorArray.Length * sizeof(float)), colorArray, BufferUsageHint.StaticDraw);
                entry.ColorBuffer = new TKVertexBuffer(buffer, 4, colorArray.Length / 4);
            }
            if (normalArray != null)
            {
                buffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalArray.Length * sizeof(float)), normalArray, BufferUsageHint.StaticDraw);
                entry.NormalBuffer = new TKVertexBuffer(buffer, 3, normalArray.Length / 3);
            }
            Entries.Add(entry);
        }
        public void AddMaterial(TKMaterial material)
        {
            Materials.Add(material);
        }
        public void Draw(bool isTransparentPass, Matrix4 mvMatrix)
        {
            foreach (var entry in Entries)
            {
                var material = Materials[entry.MaterialIndex];
                if (material.IsTransprent != isTransparentPass)
                    continue;

                for (int i = 0; i < material.Effect.NumPasses; i++)
                {
                    material.Apply(i, entry, mvMatrix);

                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, entry.IndexBuffer.BufferObject);
                    GL.DrawElements(PrimitiveType.Triangles, entry.IndexBuffer.NumItems, DrawElementsType.UnsignedShort, 0);

                    material.Clear();
                }
            }
        }
    }
}
