using CStrawberry3D.component;
using CStrawberry3D.shader;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace CStrawberry3D.loader
{
    class Buffer
    {
        public int bufferObject;
        public int itemSize;
        public int numItems;
        public Buffer(int bufferObject_, int itemSize_, int numItems_)
        {
            bufferObject = bufferObject_;
            itemSize = itemSize_;
            numItems = numItems_;
        }
    }
    class Entry
    {
        public Buffer _positionBuffer;
        public Buffer _indexBuffer;
        public Buffer _texCoordBuffer;
        public Buffer _normalBuffer;
        public Buffer _colorBuffer;
        public int _materialIndex;
    }
    public class Mesh:Resource
    {
        private List<Material> _materials = new List<Material>();
        private List<Entry> _entries = new List<Entry>();

        private bool _hasColor;


        public void addEntry(float[] positionArray, short[] indexArray, int materialIndex, float[] texCoordArray = null, float[] normalArray = null, float[] colorArray = null)
        {
            Entry entry = new Entry();
            entry._materialIndex = materialIndex;

            int buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(positionArray.Length * sizeof(float)), positionArray, BufferUsageHint.StaticDraw);
            entry._positionBuffer = new Buffer(buffer, 3, positionArray.Length / 3);

            buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexArray.Length * sizeof(ushort)), indexArray, BufferUsageHint.StaticDraw);
            entry._indexBuffer = new Buffer(buffer, 1, indexArray.Length);

            if (texCoordArray != null)
            {
                buffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texCoordArray.Length * sizeof(float)), texCoordArray, BufferUsageHint.StaticDraw);
                entry._texCoordBuffer = new Buffer(buffer, 2, texCoordArray.Length / 2);
            }
            if (colorArray != null)
            {
                buffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorArray.Length * sizeof(float)), colorArray, BufferUsageHint.StaticDraw);
                entry._colorBuffer = new Buffer(buffer, 4, colorArray.Length / 4);
            }
            if (normalArray != null)
            {
                buffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalArray.Length * sizeof(float)), normalArray, BufferUsageHint.StaticDraw);
                entry._normalBuffer = new Buffer(buffer, 3, normalArray.Length / 3);
            }

            _entries.Add(entry);
        }

        public void addMaterial(Material material)
        {
            _materials.Add(material);
        }

        public void draw(bool isTransparentPass, Matrix4 pMatrix, Matrix4 mvMatrix)
        {
            foreach (Entry entry in _entries)
            {
                Material material = _materials[entry._materialIndex];
                if (material.isTransparent != isTransparentPass)
                    continue;

                material.ready();

                GL.UniformMatrix4(material.uniformIdentifers[Shader.U_PMATRIX_IDENTIFER], false, ref pMatrix);

                ErrorCode error = GL.GetError();
                if (error != ErrorCode.NoError)
                {
                    Console.WriteLine(error);
                }

                GL.UniformMatrix4(material.uniformIdentifers[Shader.U_MVMATRIX_IDENTIFER], false, ref mvMatrix);

                 error = GL.GetError();
                if (error != ErrorCode.NoError)
                {
                    Console.WriteLine(error);
                }

                //if (material.hasPosition)
                //{
                //    GL.BindBuffer(BufferTarget.ArrayBuffer, entry._positionBuffer.bufferObject);
                //    GL.VertexAttribPointer(material.attribIdentifers[Shader.A_VERTEXPOSITION_IDENTIFER], entry._positionBuffer.itemSize, VertexAttribPointerType.Float, false, 0, 0);
                //}
                if (material.hasTexture)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, entry._texCoordBuffer.bufferObject);
                    GL.VertexAttribPointer(material.attribIdentifers[Shader.A_TEXTURECOORD_IDENTIFER], entry._texCoordBuffer.itemSize, VertexAttribPointerType.Float, false, 0, 0);
                }
                if (material.hasColor)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, entry._colorBuffer.bufferObject);
                    GL.VertexAttribPointer(material.attribIdentifers[Shader.A_VERTEXCOLOR_IDENTIFER], entry._colorBuffer.itemSize, VertexAttribPointerType.Float, false, 0, 0);
                }

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, entry._indexBuffer.bufferObject);
                GL.DrawElements(BeginMode.Triangles, entry._indexBuffer.numItems, DrawElementsType.UnsignedShort, 0);
            }
        }
    }
}
