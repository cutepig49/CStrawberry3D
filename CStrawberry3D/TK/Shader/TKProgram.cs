using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace CStrawberry3D.TK
{
    public class TKProgram:IDisposable
    {
        public Dictionary<UniformIdentifer, int> UniformIdentifers{get;private set;}
        public Dictionary<AttributeIdentifer, int> AttributeIdentifers { get; private set; }
        public int ProgramObject { get; private set; }
        public TKShader VertexShader { get; private set; }
        public TKShader FragmentShader { get; private set; }
        public TKProgram(string vertexShaderPath, string fragmentShaderPath)
        {
            UniformIdentifers = new Dictionary<UniformIdentifer, int>();
            AttributeIdentifers = new Dictionary<AttributeIdentifer, int>();
            foreach (var identifer in (UniformIdentifer[])Enum.GetValues(typeof(UniformIdentifer)))
            {
                UniformIdentifers.Add(identifer, -1);
            }
            foreach (var identifer in (AttributeIdentifer[])Enum.GetValues(typeof(AttributeIdentifer)))
            {
                AttributeIdentifers.Add(identifer, -1);
            }
            var fileReader = new StreamReader(vertexShaderPath);
            var vertexScript = fileReader.ReadToEnd();
            fileReader.Close();
            fileReader = new StreamReader(fragmentShaderPath);
            var fragmentScript = fileReader.ReadToEnd();
            fileReader.Close();
            VertexShader = new TKShader(vertexScript, ShaderType.VertexShader);
            FragmentShader = new TKShader(fragmentScript, ShaderType.FragmentShader);
            ProgramObject = GL.CreateProgram();
            GL.AttachShader(ProgramObject, VertexShader.ShaderObject);
            GL.AttachShader(ProgramObject, FragmentShader.ShaderObject);
            GL.LinkProgram(ProgramObject);

            int result;
            GL.GetProgram(ProgramObject, GetProgramParameterName.LinkStatus, out result);
            if (result != 1)
            {

                TKRenderer.Singleton.Logger.Error("Could not initialise shaders.");
                return;
            }

            _CacheUniformLocation();
            _CacheAttribLocation();
        }
        public void Apply(Dictionary<AttributeIdentifer, object> attributeValues, Dictionary<UniformIdentifer, object> uniformValues)
        {
            GL.UseProgram(ProgramObject);
            foreach (var key in AttributeIdentifers.Keys)
            {
                if (AttributeIdentifers[key] != -1)
                {
                    TKVertexBuffer buffer;
                    switch (key)
                    {
                        case AttributeIdentifer.aVertexPosition:
                            GL.EnableVertexAttribArray(AttributeIdentifers[key]);
                            buffer = (TKVertexBuffer)attributeValues[key];
                            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.BufferObject);
                            GL.VertexAttribPointer(AttributeIdentifers[key], buffer.ItemSize, VertexAttribPointerType.Float, false, 0, 0);
                            break;
                        case AttributeIdentifer.aVertexColor:
                            GL.EnableVertexAttribArray(AttributeIdentifers[key]);
                            buffer = (TKVertexBuffer)attributeValues[key];
                            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.BufferObject);
                            GL.VertexAttribPointer(AttributeIdentifers[key], buffer.ItemSize, VertexAttribPointerType.Float, false, 0, 0);
                            break;
                        case AttributeIdentifer.aTextureCoord:
                            GL.EnableVertexAttribArray(AttributeIdentifers[key]);
                            buffer = (TKVertexBuffer)attributeValues[key];
                            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.BufferObject);
                            GL.VertexAttribPointer(AttributeIdentifers[key], buffer.ItemSize, VertexAttribPointerType.Float, false, 0, 0);
                            break;
                        case AttributeIdentifer.aVertexNormal:
                            GL.EnableVertexAttribArray(AttributeIdentifers[key]);
                            buffer = (TKVertexBuffer)attributeValues[key];
                            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.BufferObject);
                            GL.VertexAttribPointer(AttributeIdentifers[key], buffer.ItemSize, VertexAttribPointerType.Float, false, 0, 0);
                            break;
                        default:
                            TKRenderer.Singleton.Logger.Error(string.Format("Identifer '{0}' doesn't have a value!", Enum.GetName(typeof(AttributeIdentifer), key)));
                            break;
                    }
                }
            }
            foreach (var key in uniformValues.Keys)
            {
                if (UniformIdentifers[key] != -1)
                {
                    //TODO the same.
                    List<float> floats;
                    List<int> ints;
                    switch (key)
                    {
                        case UniformIdentifer.uPositionIndex:
                            GL.Uniform1(UniformIdentifers[key], (int)uniformValues[key]);
                            break;
                        case UniformIdentifer.uNormalIndex:
                            GL.Uniform1(UniformIdentifers[key], (int)uniformValues[key]);
                            break;
                        case UniformIdentifer.uDiffuseIndex:
                            GL.Uniform1(UniformIdentifers[key], (int)uniformValues[key]);
                            break;
                        case UniformIdentifer.uGlobalColor:
                            GL.Uniform4(UniformIdentifers[key], (Vector4)uniformValues[key]);
                            break;
                        case UniformIdentifer.uPMatrix:
                            var pMatrix = (Matrix4)uniformValues[key];
                            GL.UniformMatrix4(UniformIdentifers[key], false, ref pMatrix);
                            break;
                        case UniformIdentifer.uMVMatrix:
                            var mvMatrix = (Matrix4)uniformValues[key];
                            GL.UniformMatrix4(UniformIdentifers[key], false, ref mvMatrix);
                            break;
                        case UniformIdentifer.uClearColor:
                            var clearColor = (Vector4)uniformValues[key];
                            GL.Uniform4(UniformIdentifers[key], clearColor);
                            break;
                        case UniformIdentifer.uVMatrix:
                            var vMatrix = (Matrix4)uniformValues[key];
                            GL.UniformMatrix4(UniformIdentifers[key], false, ref vMatrix);
                            break;
                        case UniformIdentifer.uSamplers:
                            var samplers = (int[])uniformValues[key];
                            ints = new List<int>();
                            for (int i = 0; i < samplers.Length; i++)
                            {
                                GL.ActiveTexture(TextureUnit.Texture0 + i);
                                GL.BindTexture(TextureTarget.Texture2D, samplers[i]);
                                ints.Add(i);
                            }
                            GL.Uniform1(UniformIdentifers[key], samplers.Length, ints.ToArray());
                            break;
                        case UniformIdentifer.uNumSamplers:
                            var numSamplers = (int)uniformValues[key];
                            GL.Uniform1(UniformIdentifers[key], numSamplers);
                            break;
                        case UniformIdentifer.uNumDirections:
                            var numDirections = (int)uniformValues[key];
                            GL.Uniform1(UniformIdentifers[key], numDirections);
                            break;
                        case UniformIdentifer.uDirections:
                            var directions = (Vector3[])uniformValues[key];
                            floats = new List<float>();
                            foreach (var vec in directions)
                            {
                                floats.Add(vec.X);
                                floats.Add(vec.Y);
                                floats.Add(vec.Z);
                            }
                            GL.Uniform3(UniformIdentifers[key], floats.Count / 3, floats.ToArray());
                            break;
                        case UniformIdentifer.uNMatrix:
                            var nMatrix = (Matrix4)uniformValues[key];
                            GL.UniformMatrix4(UniformIdentifers[key], false, ref nMatrix);
                            break;
                        case UniformIdentifer.uAmbientLight:
                            var ambientLight = (Vector4)uniformValues[key];
                            GL.Uniform4(UniformIdentifers[key], ambientLight);
                            break;
                        case UniformIdentifer.uDirectionalLights:
                            var directionalLights = (Vector4[])uniformValues[key];
                            floats = new List<float>();
                            foreach (var vec in directionalLights)
                            {
                                floats.Add(vec.X);
                                floats.Add(vec.Y);
                                floats.Add(vec.Z);
                                floats.Add(vec.W);
                            }
                            GL.Uniform4(UniformIdentifers[key], floats.Count / 4, floats.ToArray());
                            break;
                        case UniformIdentifer.uDeferredDiffuse:
                            GL.ActiveTexture(TextureUnit.Texture1);
                            GL.BindTexture(TextureTarget.Texture2D, (int)uniformValues[key]);
                            GL.Uniform1(UniformIdentifers[key], 1);
                            break;
                        case UniformIdentifer.uDeferredNormal:
                            GL.ActiveTexture(TextureUnit.Texture2);
                            GL.BindTexture(TextureTarget.Texture2D, (int)uniformValues[key]);
                            GL.Uniform1(UniformIdentifers[key], 2);
                            break;
                        case UniformIdentifer.uDeferredPosition:
                            GL.ActiveTexture(TextureUnit.Texture0);
                            GL.BindTexture(TextureTarget.Texture2D, (int)uniformValues[key]);
                            GL.Uniform1(UniformIdentifers[key], 0);
                            break;
                        default:
                            TKRenderer.Singleton.Logger.Error(string.Format("Identifer '{0}' doesn't have a value!", Enum.GetName(typeof(UniformIdentifer), key)));
                            break;
                    }
                }
            }
        }
        public void Clear()
        {
            foreach (var key in AttributeIdentifers.Keys)
            {
                if (AttributeIdentifers[key] != -1)
                {
                    GL.DisableVertexAttribArray(AttributeIdentifers[key]);
                }
            }
            GL.UseProgram(0);
        }
        void _CacheAttribLocation()
        {
            foreach (var key in new List<AttributeIdentifer>(AttributeIdentifers.Keys))
            {
                AttributeIdentifers[key] = GL.GetAttribLocation(ProgramObject, Enum.GetName(typeof(AttributeIdentifer), key));
            }
        }
        void _CacheUniformLocation()
        {
            foreach (var key in new List<UniformIdentifer>(UniformIdentifers.Keys))
            {
                UniformIdentifers[key] = GL.GetUniformLocation(ProgramObject, Enum.GetName(typeof(UniformIdentifer), key));
            }
        }
        public void Dispose()
        {
            GL.DeleteProgram(ProgramObject);
            VertexShader.Dispose();
            FragmentShader.Dispose();
        }
    }

}
