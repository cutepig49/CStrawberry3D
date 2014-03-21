using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;
using CStrawberry3D.Platform;

namespace CStrawberry3D.TK
{
    public class TKShader : IDisposable
    {
        public static TKShader Create(string shaderScript, ShaderType shaderType)
        {
            return new TKShader(shaderScript, shaderType);
        }
        public static TKShader CreateFromFile(string fileName, ShaderType shaderType)
        {
            using (var fileReader = new StreamReader(fileName))
            {
                var shaderScript = fileReader.ReadToEnd();
                return new TKShader(shaderScript, shaderType);
            }
        }
        public int ShaderObject { get; private set; }
        public string ShaderScript { get; private set; }
        public ShaderType ShaderType { get; private set; }
        TKShader(string shaderScript, ShaderType shaderType)
        {
            ShaderScript = shaderScript;
            ShaderType = shaderType;
            ShaderObject = GL.CreateShader(ShaderType);
            GL.ShaderSource(ShaderObject, ShaderScript);
            GL.CompileShader(ShaderObject);

            int result;
            GL.GetShader(ShaderObject, ShaderParameter.CompileStatus, out result);
            if (result != 1)
            {
                Logger.Error(GL.GetShaderInfoLog(ShaderObject));
            }
        }
        public void Dispose()
        {
            GL.DeleteShader(ShaderObject);
        }
    }
}
