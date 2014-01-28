using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace CStrawberry3D.TK
{
    public class TKShader : IDisposable
    {
        public int ShaderObject { get; private set; }
        public string ShaderScript { get; private set; }
        public ShaderType ShaderType { get; private set; }
        public TKShader(string shaderScript, ShaderType shaderType)
        {
            ShaderScript = shaderScript;
            ShaderType = shaderType;
            ShaderObject = GL.CreateShader(ShaderType);
            GL.ShaderSource(ShaderObject, ShaderScript);
            GL.CompileShader(ShaderObject);

            int result;
            GL.GetShader(ShaderObject, ShaderParameter.CompileStatus, out result);
            if (result != 1)
                TKRenderer.Singleton.Logger.Error(GL.GetShaderInfoLog(ShaderObject));
        }
        public void Dispose()
        {
            GL.DeleteShader(ShaderObject);
        }
    }
}
