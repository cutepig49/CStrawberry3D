//using CStrawberry3D.component;
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using System;
//using System.Collections.Generic;
//using CStrawberry3D.TK;

//namespace CStrawberry3D.Loader
//{
//    class Entry
//    {
//        public TKBuffer _positionBuffer;
//        public TKBuffer _indexBuffer;
//        public TKBuffer _texCoordBuffer;
//        public TKBuffer _normalBuffer;
//        public TKBuffer _colorBuffer;
//        public int _materialIndex;
//    }
//    public class Mesh:Resource
//    {
//        private List<TKMaterial> _materials = new List<TKMaterial>();
//        private List<Entry> _entries = new List<Entry>();
//        private List<Texture> _diffuseTextures = new List<Texture>();

//        public void addEntry(float[] positionArray, short[] indexArray, int materialIndex, float[] texCoordArray = null, float[] normalArray = null, float[] colorArray = null)
//        {
//            Entry entry = new Entry();
//            entry._materialIndex = materialIndex;

//            int buffer = GL.GenBuffer();
//            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
//            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(positionArray.Length * sizeof(float)), positionArray, BufferUsageHint.StaticDraw);
//            entry._positionBuffer = new TKBuffer(buffer, 3, positionArray.Length / 3);

//            buffer = GL.GenBuffer();
//            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer);
//            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexArray.Length * sizeof(ushort)), indexArray, BufferUsageHint.StaticDraw);
//            entry._indexBuffer = new TKBuffer(buffer, 1, indexArray.Length);

//            if (texCoordArray != null)
//            {
//                buffer = GL.GenBuffer();
//                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
//                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texCoordArray.Length * sizeof(float)), texCoordArray, BufferUsageHint.StaticDraw);
//                entry._texCoordBuffer = new TKBuffer(buffer, 2, texCoordArray.Length / 2);
//            }
//            if (colorArray != null)
//            {
//                buffer = GL.GenBuffer();
//                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
//                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorArray.Length * sizeof(float)), colorArray, BufferUsageHint.StaticDraw);
//                entry._colorBuffer = new TKBuffer(buffer, 4, colorArray.Length / 4);
//            }
//            if (normalArray != null)
//            {
//                buffer = GL.GenBuffer();
//                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
//                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalArray.Length * sizeof(float)), normalArray, BufferUsageHint.StaticDraw);
//                entry._normalBuffer = new TKBuffer(buffer, 3, normalArray.Length / 3);
//            }

//            _entries.Add(entry);
//        }

//        public void addMaterial(TKMaterial material)
//        {
//            _materials.Add(material);
//        }

//        public void addDiffuseTexture(Texture texture)
//        {
//            _diffuseTextures.Add(texture);
//        }

//        public void changeMaterial(int index, TKEffect effect)
//        {
//            //TODO
//            if (index > -1 && index < _materials.Count)
//            {
//                _materials[index].effect = effect;
//            }
//        }

//        public void draw(bool isTransparentPass, Matrix4 pMatrix, Matrix4 mvMatrix)
//        {
//            foreach (Entry entry in _entries)
//            {
//                TKMaterial material = _materials[entry._materialIndex];
//                if (material.isTransparent != isTransparentPass)
//                    continue;

//                for (int i = 0; i < material.effect.NumPasses; i++)
//                {
//                    material.effect.setShaderValue(TKShader.U_PMATRIX_IDENTIFER, pMatrix);
//                    material.effect.setShaderValue(TKShader.U_MVMATRIX_IDENTIFER, mvMatrix);
//                    mvMatrix.Invert();
//                    mvMatrix.Transpose();
//                    var nMatrix = mvMatrix;
//                    material.effect.setShaderValue(TKShader.U_NMATRIX_IDENTIFER, nMatrix);
//                    material.effect.setShaderValue(TKShader.U_VMATRIX_IDENTIFER, OpenGLRenderer.getSingleton().renderState.viewMatrix);
//                    material.effect.setShaderValue(TKShader.U_GLOBAL_COLOR_IDENTIFER, material.globalColor);
//                    material.effect.setShaderValue(TKShader.A_VERTEXPOSITION_IDENTIFER, entry._positionBuffer);
//                    material.effect.setShaderValue(TKShader.A_VERTEXCOLOR_IDENTIFER, entry._colorBuffer);
//                    material.effect.setShaderValue(TKShader.A_TEXTURECOORD_IDENTIFER, entry._texCoordBuffer);
//                    material.effect.setShaderValue(TKShader.A_VERTEXNORMAL_IDENTIFER, entry._normalBuffer);
//                    material.effect.setShaderValue(TKShader.U_AMBIENT_LIGHT_IDENTIFER, OpenGLRenderer.getSingleton().renderState.ambientLight);
//                    material.effect.setShaderValue(TKShader.U_NUM_DIRECTIONS_IDENTIFER, OpenGLRenderer.getSingleton().renderState.directionalLights.Count);
//                    material.effect.setShaderValue(TKShader.U_DIRECTIONS_IDENTIFER, OpenGLRenderer.getSingleton().renderState.directions);
//                    material.effect.setShaderValue(TKShader.U_DIRECTIONAL_LIGHTS_IDENTIFER, OpenGLRenderer.getSingleton().renderState.directionalLights);
//                    //TODO: too ugly
//                    material.effect.setShaderValue(TKShader.U_NUM_SAMPLERS_IDENTIFER, _diffuseTextures.Count);
//                    //GL.ActiveTexture(TextureUnit.Texture0);
//                    for (int index = 0; index < _diffuseTextures.Count; index++ )
//                        GL.BindTexture(TextureTarget.Texture2D, _diffuseTextures[index].textureObject);
//                    material.effect.setShaderValue(TKShader.U_SAMPLERS_IDENTIFER, new int[] {0});

//                    material.effect.BeginPass(i);

//                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, entry._indexBuffer.BufferObject);
//                    GL.DrawElements(BeginMode.Triangles, entry._indexBuffer.NumItems, DrawElementsType.UnsignedShort, 0);

//                    material.effect.EndPass();
//                }
//            }
//        }
//    }
//}
