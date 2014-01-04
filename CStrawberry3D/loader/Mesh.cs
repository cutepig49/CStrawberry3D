using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using CStrawberry3D.component;

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
    public class Mesh:Resource
    {
        private Buffer _positionBuffer;
        private Buffer _indexBuffer;
        private Buffer _texCoordBuffer;
        private Buffer _normalBuffer;
        private Buffer _colorBuffer;

        public Mesh(float[] positionArray, ushort[] indexArray, float[] texCoordArray = null, float[] normalArray = null, float[] colorArray = null):base()
        {
            int buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(positionArray.Length*8*sizeof(float)), positionArray, BufferUsageHint.StaticDraw);
            _positionBuffer = new Buffer(buffer, 3, positionArray.Length / 3);

            buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexArray.Length * sizeof(ushort)), indexArray, BufferUsageHint.StaticDraw);
            _indexBuffer = new Buffer(buffer, 1, indexArray.Length);

            if (texCoordArray != null)
            {
                buffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texCoordArray.Length * 8 * sizeof(float)), texCoordArray, BufferUsageHint.StaticDraw);
                _texCoordBuffer = new Buffer(buffer, 2, texCoordArray.Length / 2);
            }
            if (colorArray != null)
            {
                buffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorArray.Length * 8 * sizeof(float)), colorArray, BufferUsageHint.StaticDraw);
                _colorBuffer = new Buffer(buffer, 4, colorArray.Length / 4);
            }
            if (normalArray != null)
            {
                buffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalArray.Length * 8 * sizeof(float)), normalArray, BufferUsageHint.StaticDraw);
                _normalBuffer = new Buffer(buffer, 3, normalArray.Length / 3);
            }
        }
        public void ready(Material material, Matrix4 pMatrix, Matrix4 mvMatrix)
        {
            GL.UniformMatrix4(material.uniformIdentifers[Shader.U_PMATRIX_IDENTIFER], false, ref pMatrix);
            GL.UniformMatrix4(material.uniformIdentifers[Shader.U_MVMATRIX_IDENTIFER], false, ref mvMatrix);

            if (material.hasPosition)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _positionBuffer.bufferObject);
                GL.VertexAttribPointer(material.attribIdentifers[Shader.A_VERTEXPOSITION_IDENTIFER], _positionBuffer.itemSize, VertexAttribPointerType.Float, false, 0, 0);
            }
            if (material.hasTexture)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer.bufferObject);
                GL.VertexAttribPointer(material.attribIdentifers[Shader.A_TEXTURECOORD_IDENTIFER], _texCoordBuffer.itemSize, VertexAttribPointerType.Float, false, 0, 0);
            }
            if (material.hasColor)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer.bufferObject);
                GL.VertexAttribPointer(material.attribIdentifers[Shader.A_VERTEXCOLOR_IDENTIFER], _colorBuffer.itemSize, VertexAttribPointerType.Float, false, 0, 0);
            }
        }
        public void draw()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer.bufferObject);
            GL.DrawElements(BeginMode.Triangles, _indexBuffer.numItems, DrawElementsType.UnsignedShort, 0);
        }
    }
}
