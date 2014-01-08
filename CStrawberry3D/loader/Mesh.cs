using CStrawberry3D.component;
using CStrawberry3D.shader;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using CStrawberry3D.renderer;

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
        private List<Texture> _diffuseTextures = new List<Texture>();

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

        public void addDiffuseTexture(Texture texture)
        {
            _diffuseTextures.Add(texture);
        }

        public void changeMaterial(int index, Effect effect)
        {
            //TODO
            if (index > -1 && index < _materials.Count)
            {
                _materials[index].effect = effect;
            }
        }

        public void draw(bool isTransparentPass, Matrix4 pMatrix, Matrix4 mvMatrix)
        {
            foreach (Entry entry in _entries)
            {
                Material material = _materials[entry._materialIndex];
                if (material.isTransparent != isTransparentPass)
                    continue;

                for (int i = 0; i < material.effect.numPasses; i++)
                {
                    material.effect.setShaderValue(Shader.U_PMATRIX_IDENTIFER, pMatrix);
                    material.effect.setShaderValue(Shader.U_MVMATRIX_IDENTIFER, mvMatrix);
                    mvMatrix.Invert();
                    mvMatrix.Transpose();
                    var nMatrix = mvMatrix;
                    material.effect.setShaderValue(Shader.U_NMATRIX_IDENTIFER, nMatrix);
                    material.effect.setShaderValue(Shader.U_VMATRIX_IDENTIFER, OpenGLRenderer.getSingleton().renderState.viewMatrix);
                    material.effect.setShaderValue(Shader.U_GLOBAL_COLOR_IDENTIFER, material.globalColor);
                    material.effect.setShaderValue(Shader.A_VERTEXPOSITION_IDENTIFER, entry._positionBuffer);
                    material.effect.setShaderValue(Shader.A_VERTEXCOLOR_IDENTIFER, entry._colorBuffer);
                    material.effect.setShaderValue(Shader.A_TEXTURECOORD_IDENTIFER, entry._texCoordBuffer);
                    material.effect.setShaderValue(Shader.A_VERTEXNORMAL_IDENTIFER, entry._normalBuffer);
                    material.effect.setShaderValue(Shader.U_AMBIENT_LIGHT_IDENTIFER, OpenGLRenderer.getSingleton().renderState.ambientLight);
                    material.effect.setShaderValue(Shader.U_NUM_DIRECTIONS_IDENTIFER, OpenGLRenderer.getSingleton().renderState.directionalLights.Count);
                    material.effect.setShaderValue(Shader.U_DIRECTIONS, OpenGLRenderer.getSingleton().renderState.directions);
                    material.effect.setShaderValue(Shader.U_DIRECTIONAL_LIGHTS_IDENTIFER, OpenGLRenderer.getSingleton().renderState.directionalLights);
                    //TODO: too ugly
                    material.effect.setShaderValue(Shader.U_NUM_SAMPLERS_IDENTIFER, _diffuseTextures.Count);
                    //GL.ActiveTexture(TextureUnit.Texture0);
                    for (int index = 0; index < _diffuseTextures.Count; index++ )
                        GL.BindTexture(TextureTarget.Texture2D, _diffuseTextures[index].textureObject);
                    material.effect.setShaderValue(Shader.U_SAMPLERS_IDENTIFER, new int[] {0});

                    material.effect.beginPass(i);

                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, entry._indexBuffer.bufferObject);
                    GL.DrawElements(BeginMode.Triangles, entry._indexBuffer.numItems, DrawElementsType.UnsignedShort, 0);

                    material.effect.endPass();
                }
            }
        }
    }
}
